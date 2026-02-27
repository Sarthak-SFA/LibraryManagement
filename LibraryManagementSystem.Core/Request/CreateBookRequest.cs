namespace LibraryManagementSystem.Core.Requests;

public sealed class CreateBookRequest
{
    
    public string? BookName { get; init; } 

    public string? AuthorName { get; init; }

    public string? PublisherName { get; init; }

    public decimal BookPrice { get; init; } 
    public int CategoryId { get; init; }
    
  
};