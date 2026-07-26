using System.Collections.Generic;

using FilterModel =
    global::ProductionBoard.Core.Models.WeeklySynthesisFilter;

using ResultModel =
    global::ProductionBoard.Core.Models.WeeklySynthesisResult;

namespace ProductionBoard.Core.Interfaces
{
    public interface IWeeklySynthesisRepository
    {
        ResultModel GetWeeklySynthesis(FilterModel filter);

        IList<string> GetTeams();

        IList<string> GetProductionLines();

        IList<string> GetProducts();
    }
}
