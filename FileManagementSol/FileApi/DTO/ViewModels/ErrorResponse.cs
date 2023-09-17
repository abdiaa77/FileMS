namespace FileApi.DTO.ViewModels
{
    public class ErrorResponse
    {
        public int? StatusCode { get; set; }
        public string? Detail { get; set; }
        public string? StackTrace { get; set; }
        public string? Source { get; set; }
    }
}
