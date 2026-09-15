namespace E_Commerce.Entities.DTO.ResponseAPIs
{
public class ApiResponse<T>
{
public bool Success { get; set; }
public int StatusCode { get; set; } = 200;
public string Message { get; set; }
public T Data { get; set; }
public object Errors { get; set; } // يقبل Dictionary أو List

// Helper Method للنجاح
public static ApiResponse<T> SuccessResponse(T data, string message = "Success", int statusCode = 200)
{
return new ApiResponse<T>
{
Success = true,
StatusCode = statusCode,
Message = message,
Data = data
};
}

// Helper Method للفشل
public static ApiResponse<T> FailureResponse(string message, int statusCode = 400, object errors = null)
{
return new ApiResponse<T>
{
Success = false,
StatusCode = statusCode,
Message = message,
Errors = errors
};
}
}
}