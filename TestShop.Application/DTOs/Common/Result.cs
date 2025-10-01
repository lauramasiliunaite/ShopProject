namespace TestShop.Application.DTOs.Common
{
    public class Result<T>
    {
        public bool Success { get; init; }
        public T? Data { get; init; }
        public ErrorResponse? Error { get; init; }

        private Result(bool success, T? data, ErrorResponse? error)
        {
            Success = success; Data = data; Error = error;
        }

        public static Result<T> Ok(T data) => new(true, data, null);
        public static Result<T> Fail(string code, string message, IDictionary<string, string[]>? details = null)
            => new(false, default, new ErrorResponse(code, message, details));
    }
}
