namespace LibraryManagementSystem.Core.Dtos;

public class CategoryWithBooksDto(int id, string categorytype, IReadOnlyList<BookDto> books)
{
    public int Id { get; } = id;
    public string CategoryType { get; } = categorytype;


    public IReadOnlyList<BookDto> Books { get; } = books ?? throw new ArgumentNullException(nameof(books));
}