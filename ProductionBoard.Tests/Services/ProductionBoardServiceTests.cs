using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using global::ProductionBoard.Core.Interfaces;
using global::ProductionBoard.Core.Services;

using BoardModel =
    global::ProductionBoard.Core.Models.ProductionBoard;

using HourModel =
    global::ProductionBoard.Core.Models.ProductionHour;

namespace ProductionBoard.Tests.Services
{
    [TestClass]
    public class ProductionBoardServiceTests
    {
        [TestMethod]
        public void GetOrCreate_MorningLineOne_CreatesEightHours()
        {
            FakeProductionBoardRepository repository =
                new FakeProductionBoardRepository();

            ProductionBoardService service =
                new ProductionBoardService(
                    repository,
                    new ProductionCalculationService());

            BoardModel board =
                service.GetOrCreate(
                    new DateTime(2026, 7, 19),
                    "Team A",
                    "Morning",
                    "Production Line 1",
                    "Product A");

            Assert.IsNotNull(board);
            Assert.AreEqual(8, board.Hours.Count);

            Assert.AreEqual(
                "07:00",
                board.Hours[0].HourLabel);

            Assert.AreEqual(
                "14:00",
                board.Hours[7].HourLabel);

            Assert.AreEqual(
                55,
                board.Hours[0].TargetQuantity);

            Assert.AreEqual(
                445,
                board.Hours[7].TargetCumulative);
        }

        [TestMethod]
        public void GetOrCreate_NightLineTwo_AppliesCorrectHoursAndTargets()
        {
            FakeProductionBoardRepository repository =
                new FakeProductionBoardRepository();

            ProductionBoardService service =
                new ProductionBoardService(
                    repository,
                    new ProductionCalculationService());

            BoardModel board =
                service.GetOrCreate(
                    new DateTime(2026, 7, 19),
                    "Team B",
                    "Night",
                    "Production Line 2",
                    "Product B");

            Assert.AreEqual(
                "23:00",
                board.Hours[0].HourLabel);

            Assert.AreEqual(
                "06:00",
                board.Hours[7].HourLabel);

            Assert.AreEqual(
                110,
                board.Hours[0].TargetQuantity);

            Assert.AreEqual(
                890,
                board.Hours[7].TargetCumulative);
        }

        [TestMethod]
        public void SaveHour_RecalculatesActualAndScrapCumulativeValues()
        {
            FakeProductionBoardRepository repository =
                new FakeProductionBoardRepository();

            ProductionBoardService service =
                new ProductionBoardService(
                    repository,
                    new ProductionCalculationService());

            BoardModel board =
                service.GetOrCreate(
                    new DateTime(2026, 7, 19),
                    "Team A",
                    "Morning",
                    "Production Line 1",
                    "Product A");

            HourModel firstHour = board.Hours[0];
            firstHour.ActualQuantity = 50;
            firstHour.ScrapQuantity = 2;

            BoardModel updatedBoard =
                service.SaveHour(firstHour);

            Assert.AreEqual(
                50,
                updatedBoard.Hours[0].ActualCumulative);

            Assert.AreEqual(
                2,
                updatedBoard.Hours[0].ScrapCumulative);
        }

        private sealed class FakeProductionBoardRepository
            : IProductionBoardRepository
        {
            private BoardModel _board;
            private int _nextBoardId = 1;
            private int _nextHourId = 1;

            public BoardModel GetById(int boardId)
            {
                if (_board == null ||
                    _board.Id != boardId)
                {
                    return null;
                }

                return _board;
            }

            public BoardModel Find(
                DateTime boardDate,
                string teamName,
                string shiftName,
                string lineName,
                string productName)
            {
                if (_board == null)
                {
                    return null;
                }

                bool matches =
                    _board.BoardDate.Date == boardDate.Date &&
                    _board.TeamName == teamName &&
                    _board.ShiftName == shiftName &&
                    _board.LineName == lineName &&
                    _board.ProductName == productName;

                return matches
                    ? _board
                    : null;
            }

            public int Create(BoardModel board)
            {
                board.Id = _nextBoardId++;
                board.CreatedAt = DateTime.UtcNow;

                if (board.Hours == null)
                {
                    board.Hours =
                        new List<HourModel>();
                }

                foreach (HourModel hour in board.Hours)
                {
                    hour.Id = _nextHourId++;
                    hour.ProductionBoardId = board.Id;
                }

                _board = board;

                return board.Id;
            }

            public void SaveHour(HourModel hour)
            {
                if (_board == null)
                {
                    throw new InvalidOperationException(
                        "No production board exists.");
                }

                HourModel storedHour = null;

                foreach (HourModel currentHour in _board.Hours)
                {
                    if (currentHour.HourNumber ==
                        hour.HourNumber)
                    {
                        storedHour = currentHour;
                        break;
                    }
                }

                if (storedHour == null)
                {
                    hour.Id = _nextHourId++;
                    _board.Hours.Add(hour);
                    return;
                }

                storedHour.HourLabel =
                    hour.HourLabel;

                storedHour.TargetQuantity =
                    hour.TargetQuantity;

                storedHour.ActualQuantity =
                    hour.ActualQuantity;

                storedHour.ScrapQuantity =
                    hour.ScrapQuantity;

                storedHour.Comment =
                    hour.Comment;

                storedHour.StopType =
                    hour.StopType;

                storedHour.StopDurationMinutes =
                    hour.StopDurationMinutes;
            }
        }
    }
}