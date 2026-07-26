namespace ProductionBoard.Core.DTOs
{
    public sealed class ProductionHourSaveItem
    {
        public int HourNumber { get; set; }

        public int ActualQuantity { get; set; }

        public int ScrapQuantity { get; set; }

        public string Comment { get; set; }
    }
}
