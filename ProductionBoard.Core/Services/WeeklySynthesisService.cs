using System;
using System.Collections.Generic;
using System.Linq;

using global::ProductionBoard.Core.Interfaces;

using FilterModel =
    global::ProductionBoard.Core.Models.WeeklySynthesisFilter;

using ResultModel =
    global::ProductionBoard.Core.Models.WeeklySynthesisResult;

namespace ProductionBoard.Core.Services
{
    public sealed class WeeklySynthesisService
    {
        private readonly IWeeklySynthesisRepository _repository;

        public WeeklySynthesisService(
            IWeeklySynthesisRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            _repository = repository;
        }

        public ResultModel GetWeeklySynthesis(
            FilterModel filter)
        {
            FilterModel cleanFilter =
                filter ?? new FilterModel();

            cleanFilter.WeekStart =
                StartOfWeek(
                    cleanFilter.WeekStart == DateTime.MinValue
                        ? DateTime.Today
                        : cleanFilter.WeekStart);

            cleanFilter.TeamName =
                NormalizeOptionalText(cleanFilter.TeamName);

            cleanFilter.LineName =
                NormalizeOptionalText(cleanFilter.LineName);

            cleanFilter.ProductName =
                NormalizeOptionalText(cleanFilter.ProductName);

            ResultModel result =
                _repository.GetWeeklySynthesis(cleanFilter);

            if (result == null)
            {
                result = new ResultModel();
            }

            result.WeekStart = cleanFilter.WeekStart;
            result.WeekEnd = cleanFilter.WeekStart.AddDays(6);

            result.TotalTarget =
                result.Rows.Sum(row => row.TargetQuantity);

            result.TotalActual =
                result.Rows.Sum(row => row.ActualQuantity);

            result.TotalScrap =
                result.Rows.Sum(row => row.ScrapQuantity);

            result.BestTeamName =
                result.Rows
                    .GroupBy(row => row.TeamName)
                    .Select(group => new
                    {
                        TeamName = group.Key,
                        Actual = group.Sum(row => row.ActualQuantity)
                    })
                    .OrderByDescending(item => item.Actual)
                    .ThenBy(item => item.TeamName)
                    .Select(item => item.TeamName)
                    .FirstOrDefault()
                ?? "—";

            return result;
        }

        public IList<string> GetTeams()
        {
            return _repository.GetTeams();
        }

        public IList<string> GetProductionLines()
        {
            return _repository.GetProductionLines();
        }

        public IList<string> GetProducts()
        {
            return _repository.GetProducts();
        }

        private static DateTime StartOfWeek(DateTime date)
        {
            int difference =
                ((int)date.DayOfWeek + 6) % 7;

            return date.Date.AddDays(-difference);
        }

        private static string NormalizeOptionalText(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value.Trim();
        }
    }
}
