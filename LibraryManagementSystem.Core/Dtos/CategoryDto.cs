namespace LibraryManagementSystem.Core.Dtos;

public sealed class CategoryDto(
    int cId,
    string cCategoryType)

{
    public int Id { get; } = cId;
    public string CategoryType { get; } = cCategoryType;
}