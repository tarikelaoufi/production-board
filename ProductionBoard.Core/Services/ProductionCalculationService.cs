using System;
using System.Collections.Generic;
using System.Linq;
using ProductionBoard.Core.Models;

namespace ProductionBoard.Core.Services
{
    public class ProductionCalculationService
    {
        public void CalculateCumulativeValues(
            IList<ProductionHour> hours)
        {
            if (hours == null)
            {
                throw new ArgumentNullException(nameof(hours));
            }

            int targetCumulative = 0;
            int actualCumulative = 0;
            int scrapCumulative = 0;

            foreach (ProductionHour hour in
                     hours.OrderBy(item => item.HourNumber))
            {
                targetCumulative += hour.TargetQuantity;
                actualCumulative += hour.ActualQuantity;
                scrapCumulative += hour.ScrapQuantity;

                hour.TargetCumulative = targetCumulative;
                hour.ActualCumulative = actualCumulative;
                hour.ScrapCumulative = scrapCumulative;
            }
        }
    }
}