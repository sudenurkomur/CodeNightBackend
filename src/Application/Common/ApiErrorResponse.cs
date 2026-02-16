namespace CodeNight.Application.Common;

public class ApiErrorResponse
{
    public ApiError Error { get; set; } = new();
}

public class ApiError
{
    public string Code { get; set; } = null!;
    public string Message { get; set; } = null!;
    public List<ApiErrorDetail> Details { get; set; } = new();
    public string RequestId { get; set; } = null!;
}

public class ApiErrorDetail
{
    public string Field { get; set; } = null!;
    public string Issue { get; set; } = null!;
}
