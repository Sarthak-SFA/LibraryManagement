using LibraryManagementSystem.Core.Dtos;
using LibraryManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services.Services;

public sealed class BookService
{
    private readonly AppDbContext _dbContext;

    public BookService(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public IEnumerable<BookDto> GetAll()
    {
        return _dbContext.Books
            .Include(b => b.Category)
            .Select(b => new BookDto(
                b.Id,
                b.BookName,
                b.AuthorName,
                b.PublisherName,
                b.BookPrice,
                b.CategoryId
            ))
            .ToList();
    }

    public BookDto? GetById(int id)
    {
        return _dbContext.Books
            .Include(b => b.Category)
            .Where(b => b.Id == id)
            .Select(b => new BookDto(
                b.Id,
                b.BookName,
                b.AuthorName,
                b.PublisherName,
                b.BookPrice,
                b.CategoryId
            ))
            .FirstOrDefault();
    }

    public IEnumerable<BookDto> Search(string keyword)
    {
        keyword = keyword.ToLower();

        return _dbContext.Books
            .Include(b => b.Category)
            .Where(b =>
                b.BookName.ToLower().Contains(keyword) ||
                b.AuthorName.ToLower().Contains(keyword))
            .Select(b => new BookDto(
                b.Id,
                b.BookName,
                b.AuthorName,
                b.PublisherName,
                b.BookPrice,
                b.CategoryId
            ))
            .ToList();
    }

    public IEnumerable<BookDto> GetAllByCategory(int categoryId)
    {
        return _dbContext.Books
            .Where(b => b.CategoryId == categoryId)
            .Select(b => new BookDto(
                b.Id,
                b.BookName,
                b.AuthorName,
                b.PublisherName,
                b.BookPrice,
                b.CategoryId
            ))
            .ToList();
    }
}