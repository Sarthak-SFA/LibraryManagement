namespace LibraryManagementSystem.Core.Requests;

public sealed class CreateBookIssueRequest
{ 
    public int BookId { get; init; }
    
    public int MemberId { get; init; }
    
    public DateTime IssueDate { get; init; }

    public DateTime? ReturnDate { get; init; }
    public DateTime? RenewDate { get; init; }
    public DateTime? RenewReturnDate { get; init; }



}