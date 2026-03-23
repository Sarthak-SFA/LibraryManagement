namespace LibraryManagementSystem.Persistence;

public sealed class BookIssue
{
    public int Id { get; set; }

    public int BookId { get; set; }
    public Book? Book { get; set; }

    public int MemberId { get; set; }
    public Member? Member { get; set; }

    public DateOnly? IssueDate { get; set; }
    public DateOnly? ReturnDate { get; set; }
    public DateOnly? RenewDate { get; set; }
    public DateOnly? RenewReturnDate { get; set; }
}