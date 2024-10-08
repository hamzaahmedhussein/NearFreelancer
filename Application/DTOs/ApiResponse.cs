namespace Connect.Application.DTOs
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; } = default!;
        public List<string>? Errors { get; set; } = new List<string>();

        public ApiResponse() { }

        // Success response constructor
        public ApiResponse(int statusCode, string message, T data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
            Errors = new List<string>();
        }

        // Error response constructor
        public ApiResponse(int statusCode, string message, List<string> errors)
        {
            StatusCode = statusCode;
            Message = message;
            Errors = errors;
            Data = default!;
        }
    }

}
