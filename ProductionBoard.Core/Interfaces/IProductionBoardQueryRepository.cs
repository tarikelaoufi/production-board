using System.Collections.Generic;

using FilterModel =
    global::ProductionBoard.Core.Models.ProductionBoardSearchFilter;

using SummaryModel =
    global::ProductionBoard.Core.Models.ProductionBoardSummary;

namespace ProductionBoard.Core.Interfaces
{
    public interface IProductionBoardQueryRepository
    {
        IList<SummaryModel> Search(FilterModel filter);

        int? FindAdjacentBoardId(
            int currentBoardId,
            bool moveNext,
            string teamNameRestriction);
    }
}
