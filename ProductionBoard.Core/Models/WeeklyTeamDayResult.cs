using System;

namespace ProductionBoard.Core.Models
{
    public sealed class WeeklyTeamDayResult
    {
        public DateTime BoardDate { get; set; }

        public string TeamName { get; set; }

        public int TargetQuantity { get; set; }

        public int ActualQuantity { get; set; }

        public int ScrapQuantity { get; set; }

        public decimal EfficiencyPercent
        {
            get
            {
                if (TargetQuantity <= 0)
                {
                    return 0m;
                }

                return Math.Round(
                    ActualQuantity * 100m / TargetQuantity,
                    1,
                    MidpointRounding.AwayFromZero);
            }
        }
    }
}
