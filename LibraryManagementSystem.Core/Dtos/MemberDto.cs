namespace LibraryManagementSystem.Core.Dtos;

public record MemberDto(
    int Id,
    string MemberName,
    string MemberType,
    int MemberTypeID
    )
{
    public int Id { get; } = Id;
    public string? MemberName { get; } = MemberName;
    public string? MemberType { get; } = MemberType;
    public int MemberTypeID{ get; } = MemberTypeID;
}
