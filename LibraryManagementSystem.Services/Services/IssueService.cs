using System.Collections.ObjectModel;
using LibraryManagementSystem.Core.Dtos;
using LibraryManagementSystem.Core.Request;
using LibraryManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Services.Services;

public sealed class IssueService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<IssueService> _logger;


    public IssueService(AppDbContext dbContext, ILogger<IssueService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger;

    }

    public IEnumerable<IssueDto> GetAll()
    {
        IList<IssueDto> bookissue = _dbContext.BookIssue
            .Include(x => x.Book)
            .Include(x => x.Member)
            .Select(x => new IssueDto(
                x.Id,
                x.Book!.BookName,
                x.Member!.MemberName,
                x.IssueDate,
                x.ReturnDate,
                x.RenewDate,
                x.RenewReturnDate
            ))
            .ToArray();
        return new ReadOnlyCollection<IssueDto>(bookissue);
    }

    public IssueDto? AddIssueRequest(CreateBookIssueRequest request)
    {
        try
        {
            DateTime issueDate = DateTime.Now.Date;
            DateTime returnDate = issueDate.AddDays(15);

            BookIssue? bookIssue = _dbContext.BookIssue.FirstOrDefault(bi =>
                bi.BookId == request.BookId &&
                bi.MemberId == request.MemberId &&
                bi.RenewDate == null &&
                bi.RenewReturnDate == null);

            if (bookIssue != null)
                throw new Exception("This book is already issued to the member.");

            if (!_dbContext.Member.Any(m => m.Id == request.MemberId))
                throw new ConflictException($"Member with ID {request.MemberId} does not exist.");

            bookIssue = new BookIssue
            {
                BookId = request.BookId,
                MemberId = request.MemberId,
                IssueDate = issueDate,
                ReturnDate = returnDate,
                RenewDate = null,
                RenewReturnDate = null
            };

            _dbContext.BookIssue.Add(bookIssue);
            _dbContext.SaveChanges();

            return new IssueDto(
                bookIssue.Id,
                _dbContext.Book.FirstOrDefault(b => b.Id == bookIssue.BookId)?.BookName,
                _dbContext.Member.FirstOrDefault(m => m.Id == bookIssue.MemberId)?.MemberName,
                bookIssue.IssueDate,
                bookIssue.ReturnDate,
                bookIssue.RenewDate,
                bookIssue.RenewReturnDate
            );
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error while creating a Book.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating a BookIssue.");
        }

        return null;
    }
    
    public IssueDto? RenewBook(int bookId, int memberId)
    {
        try
        {
            BookIssue? bookIssue = _dbContext.BookIssue
                .Include(x => x.Book)
                .Include(x => x.Member)
                .FirstOrDefault(bi =>
                    bi.BookId == bookId &&
                    bi.MemberId == memberId &&
                    bi.ReturnDate != null);

            if (bookIssue == null)
                throw new Exception("No active issue found for this book and member.");

          
            if (bookIssue.RenewDate != null)
                throw new Exception("Book can only be renewed once.");

            if (bookIssue.ReturnDate != null)
            {
                DateTime renewDate = bookIssue.ReturnDate.Value;
                DateTime renewReturnDate = renewDate.AddDays(15);

                bookIssue.RenewDate = renewDate;
                bookIssue.RenewReturnDate = renewReturnDate;
            }

            _dbContext.SaveChanges();

            return new IssueDto(
                bookIssue.Id,
                bookIssue.Book?.BookName,
                bookIssue.Member?.MemberName,
                bookIssue.IssueDate,
                bookIssue.ReturnDate,
                bookIssue.RenewDate,
                bookIssue.RenewReturnDate
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while renewing book.");
        }

        return null;
    }
    
    public IEnumerable<IssueDto> GetBooksByReturnDate(DateTime returnDate)
    {
        IList<IssueDto> books = _dbContext.BookIssue
            .Include(x => x.Book)
            .Include(x => x.Member)
            .Where(x => x.ReturnDate != null && x.ReturnDate.Value.Date == returnDate.Date)
            .Select(x => new IssueDto(
                x.Id,
                x.Book!.BookName,
                x.Member!.MemberName,
                x.IssueDate,
                x.ReturnDate,
                x.RenewDate,
                x.RenewReturnDate
            ))
            .ToArray();

        return new ReadOnlyCollection<IssueDto>(books);
    }
}