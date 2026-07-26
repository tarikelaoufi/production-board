using System;

namespace ProductionBoard.Core.Models
{
    public sealed class ProductionBoardSearchFilter
    {
        public DateTime? BoardDate { get; set; }

        public string TeamName { get; set; }

        public string ShiftName { get; set; }

        public string LineName { get; set; }

        public string ProductName { get; set; }

        public int MaxResults { get; set; } = 50;
    }
}
