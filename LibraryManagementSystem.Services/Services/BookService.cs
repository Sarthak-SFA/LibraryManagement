using System.Collections.ObjectModel;
using LibraryManagementSystem.Core.Dtos;
using LibraryManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;

using LibraryManagementSystem.Core.Requests;

namespace LibraryManagementSystem.Services.Services;

public sealed class BookService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<BookService> _logger;

    public BookService(AppDbContext dbContext, ILogger<BookService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger;

    }

    public IEnumerable<BookDto> GetAll()
    {
        IList<BookDto> books = _dbContext.Book
            .Include(b => b.Category)
            .Select(b => new BookDto(
                b.Id,
                b.BookName,
                b.AuthorName,
                b.PublisherName,
                b.BookPrice,
                b.CategoryId
            ))
            .ToArray();

        return new ReadOnlyCollection<BookDto>(books);
    }

    public BookDto? GetById(int id)
    {
        return _dbContext.Book
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

        IList<BookDto> books = _dbContext.Book
            .Include(b => b.Category)
            .Where(b =>
                b.BookName!.ToLower().Contains(keyword) ||
                b.AuthorName!.ToLower().Contains(keyword))
            .Select(b => new BookDto(
                b.Id,
                b.BookName,
                b.AuthorName,
                b.PublisherName,
                b.BookPrice,
                b.CategoryId
            ))
            .ToArray();
        return new ReadOnlyCollection<BookDto>(books);
    }

    public IEnumerable<BookDto> GetAllByCategory(int categoryId)
    {
        IList<BookDto> books = _dbContext.Book
            .Where(b => b.CategoryId == categoryId)
            .Select(b => new BookDto(
                b.Id,
                b.BookName,
                b.AuthorName,
                b.PublisherName,
                b.BookPrice,
                b.CategoryId
            ))
            .ToArray();
        return new ReadOnlyCollection<BookDto>(books);
    }

    public BookDto? AddBook(int categoryId, CreateBookRequest request)
    {
        try
        {
            Category? category = _dbContext.Category.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
            {
                return null;
            }

            Book? book = _dbContext.Book.FirstOrDefault(b => b.BookName == request.BookName
                                                             && b.AuthorName == request.AuthorName
                                                             && b.PublisherName == request.PublisherName
                                                             && b.BookPrice == request.BookPrice);
            if (book is not null)
            {
                throw new ConflictException($"Book with CategoryId {categoryId} already exists.");
            }

            book = new Book
            {
                BookName = request.BookName,
                AuthorName = request.AuthorName,
                PublisherName = request.PublisherName,
                BookPrice = request.BookPrice,
                CategoryId = categoryId,

            };

            _dbContext.Add(book);
            _dbContext.SaveChanges();

            return new BookDto(book.Id,
                book.BookName,
                book.AuthorName,
                book.PublisherName,
                book.BookPrice,
                book.CategoryId);
        }
        catch (ConflictException ex)
        {
            _logger.LogError(ex,
                "Error while adding a book with name {BookName}. Problem in execution of sql query.",
                request.BookName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while adding book with the name {@book}.", request);
        }
        return null;
    }


}