namespace FileWeb.DTO.ViewModels
{
    public class ReponseVM
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public ReponseVM(bool success, int statusCode, string title, string message)
        {
            Success = success;
            StatusCode = statusCode;
            Title = title;
            Message = message;
        }
    }
}
