namespace LibraryManagementSystem.Core.Request;

public sealed class CreateBookIssueRequest
{ 
    public int BookId { get; set; }
    public int MemberId { get; set; }
}