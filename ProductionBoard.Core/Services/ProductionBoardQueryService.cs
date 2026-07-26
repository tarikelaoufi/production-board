using System;
using System.Collections.Generic;

using global::ProductionBoard.Core.Interfaces;

using FilterModel =
    global::ProductionBoard.Core.Models.ProductionBoardSearchFilter;

using SummaryModel =
    global::ProductionBoard.Core.Models.ProductionBoardSummary;

namespace ProductionBoard.Core.Services
{
    public sealed class ProductionBoardQueryService
    {
        private readonly IProductionBoardQueryRepository _repository;

        public ProductionBoardQueryService(
            IProductionBoardQueryRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            _repository = repository;
        }

        public IList<SummaryModel> Search(
            FilterModel filter)
        {
            FilterModel cleanFilter =
                filter ?? new FilterModel();

            cleanFilter.TeamName =
                NormalizeOptionalText(
                    cleanFilter.TeamName);

            cleanFilter.ShiftName =
                NormalizeOptionalText(
                    cleanFilter.ShiftName);

            cleanFilter.LineName =
                NormalizeOptionalText(
                    cleanFilter.LineName);

            cleanFilter.ProductName =
                NormalizeOptionalText(
                    cleanFilter.ProductName);

            if (cleanFilter.MaxResults <= 0)
            {
                cleanFilter.MaxResults = 50;
            }

            if (cleanFilter.MaxResults > 200)
            {
                cleanFilter.MaxResults = 200;
            }

            return _repository.Search(cleanFilter);
        }

        public int? FindPreviousBoardId(
            int currentBoardId,
            string teamNameRestriction)
        {
            ValidateBoardId(currentBoardId);

            return _repository.FindAdjacentBoardId(
                currentBoardId,
                false,
                NormalizeOptionalText(
                    teamNameRestriction));
        }

        public int? FindNextBoardId(
            int currentBoardId,
            string teamNameRestriction)
        {
            ValidateBoardId(currentBoardId);

            return _repository.FindAdjacentBoardId(
                currentBoardId,
                true,
                NormalizeOptionalText(
                    teamNameRestriction));
        }

        private static string NormalizeOptionalText(
            string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value.Trim();
        }

        private static void ValidateBoardId(
            int boardId)
        {
            if (boardId <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(boardId),
                    "The production board ID must be greater than zero.");
            }
        }
    }
}
