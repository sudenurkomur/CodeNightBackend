namespace CodeNight.Application.Common;

public class ApiResponse<T>
{
    public T Data { get; set; } = default!;
    public MetaInfo Meta { get; set; } = new();
}

public class MetaInfo
{
    public string? AsOfDate { get; set; }
    public string? NextCursor { get; set; }
    public string? Window { get; set; }
}
