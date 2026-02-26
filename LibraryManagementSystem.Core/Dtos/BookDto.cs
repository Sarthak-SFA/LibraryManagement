namespace LibraryManagementSystem.Core.Dtos;

public sealed class BookDto(
    int bId,
    string? bName,
    string? bAuthorName,
    string? bPublisherName,
    decimal bBookPrice,
    int bCategoryId)
{
    public int Id { get; } = bId;

    public string? BookName { get; } = bName;

    public string? AuthorName { get; } = bAuthorName;

    public string? PublisherName { get; } = bPublisherName;

    public decimal BookPrice { get; } = bBookPrice;

    public int CategoryId { get; } = bCategoryId;
}