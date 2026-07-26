using System;

namespace ProductionBoard.Core.Models
{
    public sealed class WeeklySynthesisFilter
    {
        public DateTime WeekStart { get; set; }

        public string TeamName { get; set; }

        public string LineName { get; set; }

        public string ProductName { get; set; }
    }
}
