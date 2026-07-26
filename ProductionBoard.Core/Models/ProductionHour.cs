namespace ProductionBoard.Core.Models
{
    public class ProductionHour
    {
        public int Id { get; set; }

        public int ProductionBoardId { get; set; }

        public int HourNumber { get; set; }

        public string HourLabel { get; set; }

        public int TargetQuantity { get; set; }

        public int ActualQuantity { get; set; }

        public int ScrapQuantity { get; set; }

        public string Comment { get; set; }

        public string StopType { get; set; }

        public int StopDurationMinutes { get; set; }

        public int TargetCumulative { get; set; }

        public int ActualCumulative { get; set; }

        public int ScrapCumulative { get; set; }
    }
}