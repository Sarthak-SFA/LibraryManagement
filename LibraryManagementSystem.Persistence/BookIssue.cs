namespace LibraryManagementSystem.Persistence;

public sealed class BookIssue
{
    public int Id { get; set; }

    public int BookId { get; set; }
    public Book? Book { get; set; }

    public int MemberId { get; set; }
    public Member? Member { get; set; }

    public DateTime IssueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public DateTime? RenewDate { get; set; }
    public DateTime? RenewReturnDate { get; set; }
}