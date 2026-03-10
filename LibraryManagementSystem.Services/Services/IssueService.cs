using System.Collections.ObjectModel;
using LibraryManagementSystem.Core.Dtos;
using LibraryManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LibraryManagementSystem.Core.Requests;


namespace LibraryManagementSystem.Services.Services;

public sealed class IssueService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<IssueService> _logger;


    public IssueService(AppDbContext dbContext, ILogger<IssueService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger;
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
            BookIssue? bookIssue = _dbContext.BookIssue.FirstOrDefault(bi =>
                bi.BookId == request.BookId &&
                bi.MemberId == request.MemberId &&
                bi.IssueDate == request.IssueDate &&
                bi.ReturnDate == null &&
                bi.RenewDate == null && 
                bi.RenewReturnDate == null);
            

            if (bookIssue == null)
                throw new Exception("Failed to create a Book issue from the provided request.");

            if (!_dbContext.Member.Any(m => m.Id == bookIssue.MemberId))
                throw new ConflictException($"User with ID {bookIssue.MemberId} does not exist.");

            bookIssue = new BookIssue
            {
                BookId = request.BookId,
                MemberId = request.MemberId,
                IssueDate = request.IssueDate,
                ReturnDate = null,
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
            _logger.LogError(ex,
                "Error while creating a Book.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating a BookIssue.");
        }

        return null;
    }

}