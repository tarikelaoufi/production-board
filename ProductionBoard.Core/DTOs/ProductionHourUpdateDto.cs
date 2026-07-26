public sealed class ProductionHourUpdateDto
{
    public int HourNumber { get; set; }

    public int ActualQuantity { get; set; }

    public int ScrapQuantity { get; set; }

    public string Comment { get; set; }
}