namespace OpenFamilyMapAPI.Core.Data;

public sealed class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public int TotalCount { get; set; }
    public int TotalPages => 
        PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);

    public PagedResult<TResult> Select<TResult>(Func<T, TResult> selector)
    {
        return new PagedResult<TResult>
        {
            Items = Items.Select(selector).ToList(),
            PageNumber = PageNumber,
            PageSize = PageSize,
            TotalCount = TotalCount
        };
    }
}
