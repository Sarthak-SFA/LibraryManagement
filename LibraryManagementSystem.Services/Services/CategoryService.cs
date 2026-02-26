using System.Collections.Immutable;
using LibraryManagementSystem.Core.Dtos;
using LibraryManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services.Services;

public sealed class CategoryService
{
    private readonly AppDbContext _dbContext;

    public CategoryService(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }


    public IEnumerable<CategoryDto> GetAll()
    {
        return _dbContext.Category
            .Select(c => new CategoryDto(
                c.Id,
                c.CategoryType
            ))
            .ToList();
    }


    public CategoryDto? GetById(int id)
    {
        return _dbContext.Category
            .Where(c => c.Id == id)
            .Select(c => new CategoryDto(
                c.Id,
                c.CategoryType
            ))
            .FirstOrDefault();
    }


    public CategoryWithBooksDto? GetCategory(int id)
    {
        var category = _dbContext.Category
            .Include(c => c.Book)
            .FirstOrDefault(c => c.Id == id);

        if (category is null) return null;

        var books = category.Book.Select(b => new BookDto
                (b.Id, b.BookName, b.AuthorName, b.PublisherName, b.BookPrice, category.Id))
            .ToList()
            .ToImmutableList();

        CategoryWithBooksDto dto = new(category.Id, category.CategoryType, books);

        return dto;
    }
}