using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductionBoard.Core.Models;
using ProductionBoard.Core.Services;

namespace ProductionBoard.Tests.Services
{
    [TestClass]
    public class ProductionCalculationServiceTests
    {
        [TestMethod]
        public void CalculateCumulativeValues_CalculatesAllTotals()
        {
            IList<ProductionHour> hours =
                new List<ProductionHour>
                {
                    new ProductionHour
                    {
                        HourNumber = 1,
                        TargetQuantity = 55,
                        ActualQuantity = 50,
                        ScrapQuantity = 1
                    },
                    new ProductionHour
                    {
                        HourNumber = 2,
                        TargetQuantity = 60,
                        ActualQuantity = 58,
                        ScrapQuantity = 2
                    }
                };

            ProductionCalculationService service =
                new ProductionCalculationService();

            service.CalculateCumulativeValues(hours);

            Assert.AreEqual(55, hours[0].TargetCumulative);
            Assert.AreEqual(50, hours[0].ActualCumulative);
            Assert.AreEqual(1, hours[0].ScrapCumulative);

            Assert.AreEqual(115, hours[1].TargetCumulative);
            Assert.AreEqual(108, hours[1].ActualCumulative);
            Assert.AreEqual(3, hours[1].ScrapCumulative);
        }
    }
}