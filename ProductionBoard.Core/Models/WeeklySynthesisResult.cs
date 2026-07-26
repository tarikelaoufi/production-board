using System;
using System.Collections.Generic;

namespace ProductionBoard.Core.Models
{
    public sealed class WeeklySynthesisResult
    {
        public WeeklySynthesisResult()
        {
            Rows = new List<WeeklyTeamDayResult>();
        }

        public DateTime WeekStart { get; set; }

        public DateTime WeekEnd { get; set; }

        public IList<WeeklyTeamDayResult> Rows { get; set; }

        public int TotalTarget { get; set; }

        public int TotalActual { get; set; }

        public int TotalScrap { get; set; }

        public string BestTeamName { get; set; }

        public decimal OverallEfficiencyPercent
        {
            get
            {
                if (TotalTarget <= 0)
                {
                    return 0m;
                }

                return Math.Round(
                    TotalActual * 100m / TotalTarget,
                    1,
                    MidpointRounding.AwayFromZero);
            }
        }
    }
}
