using System.Collections.Generic;

namespace ProductionBoard.Core.DTOs
{
    public sealed class ProductionHoursSaveRequest
    {
        public ProductionHoursSaveRequest()
        {
            Hours =
                new List<ProductionHourSaveItem>();
        }

        public int BoardId { get; set; }

        public IList<ProductionHourSaveItem> Hours { get; set; }
    }
}
