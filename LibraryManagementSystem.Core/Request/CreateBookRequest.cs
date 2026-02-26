namespace LibraryManagementSystem.Core.Requests;

public sealed class CreateBookRequest(

    string BookName,
    string AuthorName,
    string PublisherName,
    decimal BookPrice,
    int CategoryId
)
{
    
    public string? BookName { get; } = BookName;

    public string? AuthorName { get; } = AuthorName;

    public string? PublisherName { get; } = PublisherName;

    public decimal BookPrice { get; } = BookPrice;
    
    public int CategoryId { get; } = CategoryId;
};