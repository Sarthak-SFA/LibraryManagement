namespace LibraryManagementSystem.Core.Dtos;

public sealed class IssueDto(
    int xId,
    int? BookId,
    int? MemberId,
    DateOnly? xIssueDate,
    DateOnly? xReturnDate,
    DateOnly? xRenewDate,
    DateOnly? xRenewReturnDate)
{
    public int Id { get; } = xId;

    public int? BookId { get; } = BookId;

    public int? MemberId { get; } = MemberId;

    public DateOnly? IssueDate { get; } = xIssueDate;

    public DateOnly? ReturnDate { get; } = xReturnDate;

    public DateOnly? RenewDate { get; } = xRenewDate;

    public DateOnly? RenewReturnDate { get; } = xRenewReturnDate;
}