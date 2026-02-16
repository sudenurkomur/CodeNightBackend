using System.Text;

namespace CodeNight.Application.Common;

public class CursorPaginationParams
{
    public int Limit { get; set; } = 25;
    public string? Cursor { get; set; }

    public int GetOffset()
    {
        if (string.IsNullOrEmpty(Cursor))
            return 0;

        try
        {
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(Cursor));
            return int.TryParse(decoded, out var offset) ? offset : 0;
        }
        catch
        {
            return 0;
        }
    }

    public static string? EncodeCursor(int offset, int limit, int totalFetched)
    {
        if (totalFetched < limit)
            return null;

        var nextOffset = offset + limit;
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(nextOffset.ToString()));
    }
}
