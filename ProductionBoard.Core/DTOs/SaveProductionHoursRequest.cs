using System.Collections.Generic;

public sealed class SaveProductionHoursRequest
{
    public int BoardId { get; set; }

    public IList<ProductionHourUpdateDto> Hours { get; set; }
}