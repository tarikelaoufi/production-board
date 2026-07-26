using System;

namespace ProductionBoard.Core.Models
{
    public sealed class ProductionBoardSummary
    {
        public int Id { get; set; }

        public DateTime BoardDate { get; set; }

        public string TeamName { get; set; }

        public string ShiftName { get; set; }

        public string LineName { get; set; }

        public string ProductName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
