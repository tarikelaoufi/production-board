using System;
using System.Collections.Generic;
using global::ProductionBoard.Core.Interfaces;
using global::ProductionBoard.Core.Models;

using BoardModel =
    global::ProductionBoard.Core.Models.ProductionBoard;

using HourModel =
    global::ProductionBoard.Core.Models.ProductionHour;

namespace ProductionBoard.Core.Services
{
    public sealed class ProductionBoardService
    {
        private readonly IProductionBoardRepository _repository;
        private readonly ProductionCalculationService _calculationService;

        public ProductionBoardService(
            IProductionBoardRepository repository,
            ProductionCalculationService calculationService)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (calculationService == null)
            {
                throw new ArgumentNullException(nameof(calculationService));
            }

            _repository = repository;
            _calculationService = calculationService;
        }

        public BoardModel GetById(int boardId)
        {
            if (boardId <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(boardId),
                    "The production board ID must be greater than zero.");
            }

            BoardModel board = _repository.GetById(boardId);

            if (board == null)
            {
                return null;
            }

            EnsureHoursCollection(board);
            _calculationService.CalculateCumulativeValues(board.Hours);

            return board;
        }

        public BoardModel GetOrCreate(
            DateTime boardDate,
            string teamName,
            string shiftName,
            string lineName,
            string productName)
        {
            ValidateRequiredText(teamName, nameof(teamName));
            ValidateRequiredText(shiftName, nameof(shiftName));
            ValidateRequiredText(lineName, nameof(lineName));
            ValidateRequiredText(productName, nameof(productName));

            string cleanTeamName = teamName.Trim();
            string cleanShiftName = shiftName.Trim();
            string cleanLineName = lineName.Trim();
            string cleanProductName = productName.Trim();

            BoardModel existingBoard = _repository.Find(
                boardDate.Date,
                cleanTeamName,
                cleanShiftName,
                cleanLineName,
                cleanProductName);

            if (existingBoard != null)
            {
                EnsureHoursCollection(existingBoard);

                _calculationService.CalculateCumulativeValues(
                    existingBoard.Hours);

                return existingBoard;
            }

            BoardModel board = new BoardModel
            {
                BoardDate = boardDate.Date,
                TeamName = cleanTeamName,
                ShiftName = cleanShiftName,
                LineName = cleanLineName,
                ProductName = cleanProductName,
                Hours = CreateInitialHours(
                    cleanShiftName,
                    cleanLineName)
            };

            int boardId = _repository.Create(board);

            BoardModel createdBoard = _repository.GetById(boardId);

            if (createdBoard == null)
            {
                board.Id = boardId;
                createdBoard = board;
            }

            EnsureHoursCollection(createdBoard);

            _calculationService.CalculateCumulativeValues(
                createdBoard.Hours);

            return createdBoard;
        }

        public BoardModel SaveHour(HourModel hour)
        {
            ValidateHour(hour);

            _repository.SaveHour(hour);

            BoardModel board =
                _repository.GetById(hour.ProductionBoardId);

            if (board == null)
            {
                throw new InvalidOperationException(
                    "The production board could not be found after saving the hour.");
            }

            EnsureHoursCollection(board);

            _calculationService.CalculateCumulativeValues(
                board.Hours);

            return board;
        }

        private static IList<HourModel> CreateInitialHours(
            string shiftName,
            string lineName)
        {
            string[] hourLabels =
                GetHourLabels(shiftName);

            int[] targets =
                GetHourlyTargets(lineName);

            IList<HourModel> hours =
                new List<HourModel>();

            for (int index = 0; index < 8; index++)
            {
                hours.Add(
                    new HourModel
                    {
                        HourNumber = index + 1,
                        HourLabel = hourLabels[index],
                        TargetQuantity = targets[index],
                        ActualQuantity = 0,
                        ScrapQuantity = 0,
                        Comment = null,
                        StopType = null,
                        StopDurationMinutes = 0
                    });
            }

            return hours;
        }

        private static string[] GetHourLabels(
            string shiftName)
        {
            string normalizedShift =
                NormalizeText(shiftName);

            if (normalizedShift == "morning" ||
                normalizedShift == "matin")
            {
                return new[]
                {
                    "07:00",
                    "08:00",
                    "09:00",
                    "10:00",
                    "11:00",
                    "12:00",
                    "13:00",
                    "14:00"
                };
            }

            if (normalizedShift == "afternoon" ||
                normalizedShift == "apres-midi" ||
                normalizedShift == "après-midi")
            {
                return new[]
                {
                    "15:00",
                    "16:00",
                    "17:00",
                    "18:00",
                    "19:00",
                    "20:00",
                    "21:00",
                    "22:00"
                };
            }

            if (normalizedShift == "night" ||
                normalizedShift == "nuit")
            {
                return new[]
                {
                    "23:00",
                    "00:00",
                    "01:00",
                    "02:00",
                    "03:00",
                    "04:00",
                    "05:00",
                    "06:00"
                };
            }

            throw new ArgumentException(
                "Unknown shift. Accepted values are Morning, Afternoon, Night, Matin, Après-midi or Nuit.",
                nameof(shiftName));
        }

        private static int[] GetHourlyTargets(
            string lineName)
        {
            int multiplier =
                GetLineMultiplier(lineName);

            int[] baseTargets =
            {
                55,
                60,
                60,
                40,
                60,
                60,
                60,
                50
            };

            int[] targets =
                new int[baseTargets.Length];

            for (int index = 0;
                 index < baseTargets.Length;
                 index++)
            {
                targets[index] =
                    baseTargets[index] * multiplier;
            }

            return targets;
        }

        private static int GetLineMultiplier(
            string lineName)
        {
            string normalizedLine =
                NormalizeText(lineName);

            if (normalizedLine.EndsWith("1"))
            {
                return 1;
            }

            if (normalizedLine.EndsWith("2"))
            {
                return 2;
            }

            if (normalizedLine.EndsWith("3"))
            {
                return 4;
            }

            throw new ArgumentException(
                "Unknown production line. The line name must end with 1, 2 or 3.",
                nameof(lineName));
        }

        private static void ValidateHour(
            HourModel hour)
        {
            if (hour == null)
            {
                throw new ArgumentNullException(nameof(hour));
            }

            if (hour.ProductionBoardId <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(hour.ProductionBoardId),
                    "A valid production board ID is required.");
            }

            if (hour.HourNumber < 1 ||
                hour.HourNumber > 8)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(hour.HourNumber),
                    "The hour number must be between 1 and 8.");
            }

            if (string.IsNullOrWhiteSpace(hour.HourLabel))
            {
                throw new ArgumentException(
                    "The hour label is required.",
                    nameof(hour.HourLabel));
            }

            if (hour.TargetQuantity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(hour.TargetQuantity),
                    "Target quantity cannot be negative.");
            }

            if (hour.ActualQuantity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(hour.ActualQuantity),
                    "Actual quantity cannot be negative.");
            }

            if (hour.ScrapQuantity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(hour.ScrapQuantity),
                    "Scrap quantity cannot be negative.");
            }

            if (hour.StopDurationMinutes < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(hour.StopDurationMinutes),
                    "Stop duration cannot be negative.");
            }

            if (hour.Comment != null &&
                hour.Comment.Length > 1000)
            {
                throw new ArgumentException(
                    "The comment cannot exceed 1000 characters.",
                    nameof(hour.Comment));
            }

            if (hour.StopType != null &&
                hour.StopType.Length > 100)
            {
                throw new ArgumentException(
                    "The stop type cannot exceed 100 characters.",
                    nameof(hour.StopType));
            }
        }

        private static void EnsureHoursCollection(
            BoardModel board)
        {
            if (board.Hours == null)
            {
                board.Hours =
                    new List<HourModel>();
            }
        }

        private static string NormalizeText(
            string value)
        {
            return value
                .Trim()
                .ToLowerInvariant();
        }

        private static void ValidateRequiredText(
            string value,
            string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "A value is required.",
                    parameterName);
            }
        }
    }
}