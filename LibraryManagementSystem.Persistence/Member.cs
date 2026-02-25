namespace LibraryManagementSystem.Persistence;

public sealed class Member
{
    public int Id { get; set; }
    public required string MemberName { get; set; }
    public required string MemberType { get; set; }

    public IList<BookIssue> BookIssued { get; set; } = [];
}