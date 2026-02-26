namespace LibraryManagementSystem.Persistence;

public sealed class Book
{
    public int Id { get; set; }
    public required string? BookName { get; set; }
    public required string? AuthorName { get; set; }
    public required string? PublisherName { get; set; }
    public decimal BookPrice { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }
    
    public IList<BookIssue> BookIssued { get; set; } = [];
    
}