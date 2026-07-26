namespace ProductionBoard.Core.DTOs
{
    public sealed class ProductionHoursSaveResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public int SavedCount { get; set; }

        public string UpdatedAt { get; set; }

        public static ProductionHoursSaveResult Failed(
            string message)
        {
            return new ProductionHoursSaveResult
            {
                Success = false,
                Message = message ??
                    "The production hours could not be saved.",
                SavedCount = 0,
                UpdatedAt = string.Empty
            };
        }
    }
}
