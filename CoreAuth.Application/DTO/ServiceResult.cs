using System.Text.Json.Serialization;

namespace CoreAuth.Application.DTO;

public class ServiceResult<T>
{
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = [];
    [JsonIgnore] public int StatusCode { get; set; }
    [JsonIgnore] public bool IsSuccess { get; set; }

    public static ServiceResult<T> Success(T data, int statusCode)
    {
        return new ServiceResult<T>
        {
            Data = data,
            StatusCode = statusCode,
            IsSuccess = true
        };
    }

    public static ServiceResult<T> Success(int statusCode)
    {
        return new ServiceResult<T>
        {
            StatusCode = statusCode,
            IsSuccess = true
        };
    }

    public static ServiceResult<T> Fail(List<string> errors, int statusCode)
    {
        return new ServiceResult<T>
        {
            Errors = errors,
            StatusCode = statusCode,
            IsSuccess = false
        };
    }

    public static ServiceResult<T> Fail(string error, int statusCode)
    {
        return new ServiceResult<T>
        {
            Errors = new List<string> { error },
            StatusCode = statusCode,
            IsSuccess = false
        };
    }
}

public class ServiceResult
{
    public List<string> Errors { get; set; } = [];
    [JsonIgnore] public int StatusCode { get; set; }
    [JsonIgnore] public bool IsSuccess { get; set; }

    public static ServiceResult Success(int statusCode)
    {
        return new ServiceResult
        {
            StatusCode = statusCode,
            IsSuccess = true
        };
    }

    public static ServiceResult Fail(List<string> errors, int statusCode)
    {
        return new ServiceResult
        {
            Errors = errors,
            StatusCode = statusCode,
            IsSuccess = false
        };
    }

    public static ServiceResult Fail(string error, int statusCode)
    {
        return new ServiceResult
        {
            Errors = new List<string> { error },
            StatusCode = statusCode,
            IsSuccess = false
        };
    }
}