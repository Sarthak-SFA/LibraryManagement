namespace LibraryManagementSystem.Core.Dtos;

public sealed class IssueDto(
    int xId,
    string name,
    string memberName,
    DateTime xIssueDate,
    DateTime? xReturnDate,
    DateTime? xRenewDate,
    DateTime? xRenewReturnDate)
{
    public int Id { get; } = xId;

    public string? BookName { get; } = name;

    public string? MemberName { get; } = memberName;

    public DateTime IssueDate { get; } = xIssueDate;

    public DateTime? ReturnDate { get; } = xReturnDate;

    public DateTime? RenewDate { get; } = xRenewDate;
    
    public DateTime? RenewReturnDate { get; } = xRenewReturnDate;
}