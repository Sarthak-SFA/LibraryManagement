namespace LibraryManagementSystem.Core.Request;

public sealed class RenewBookRequest
{
    public int BookId { get; init; }
    public int MemberId { get; init; }
}