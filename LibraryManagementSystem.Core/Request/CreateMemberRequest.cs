namespace LibraryManagementSystem.Core.Request;

public sealed class CreateMemberRequest

{
    public string? MemberName { get; init; } 
    public string? MemberType { get; init; } 
    public int MemberTypeID{ get; init; }
}