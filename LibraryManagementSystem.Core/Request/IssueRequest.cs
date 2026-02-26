namespace LibraryManagementSystem.Core.Request;

public sealed class IssueRequest
{
    public int BookId { get; set; }
    public int MemberId { get; set; }
}