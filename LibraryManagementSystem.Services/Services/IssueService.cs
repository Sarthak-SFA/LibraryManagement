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
                x.Book!.Id,
                x.Member!.Id,
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
            var member = _dbContext.Member
                .FirstOrDefault(m => m.Id == request.MemberId);

            if (member == null) throw new ConflictException($"Member with ID {request.MemberId} not found.");

            var book = _dbContext.Book
                .FirstOrDefault(b => b.Id == request.BookId);

            if (book == null) throw new ConflictException($"Book with ID {request.BookId} does not exist.");

            var alreadyIssued = _dbContext.BookIssue
                .Any(b => b.BookId == request.BookId &&
                          b.MemberId == request.MemberId);

            if (alreadyIssued) throw new ConflictException("This member already has this book issue.");

            var issuedBooksCount = _dbContext.BookIssue
                .Count(b => b.MemberId == request.MemberId && b.ReturnDate != null);


            BookIssue bookIssue = new()
            {
                BookId = request.BookId,
                MemberId = request.MemberId,
                IssueDate = DateOnly.FromDateTime(DateTime.Today),
                ReturnDate = DateOnly.FromDateTime(DateTime.Today).AddDays(15),
                RenewDate = null,
                RenewReturnDate = null
            };

            _dbContext.BookIssue.Add(bookIssue);
            _dbContext.SaveChanges();

            IssueDto CreateBookIssue = new(
                bookIssue.Id,
                book.Id,
                member.Id,
                bookIssue.IssueDate,
                bookIssue.ReturnDate,
                bookIssue.RenewDate,
                bookIssue.RenewReturnDate
            );

            return CreateBookIssue;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex,
                "Error while creating Book Issue for MemberId {MemberId} and BookId {BookId}.",
                request.MemberId, request.BookId);
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "Error while creating Book Issue.");
        }

        return null;
    }

    public IssueDto? RenewBook(RenewBookRequest request)
    {
        try
        {
            var bookIssue = _dbContext.BookIssue
                .Include(x => x.Book)
                .Include(x => x.Member)
                .FirstOrDefault(b =>
                    b.BookId == request.BookId &&
                    b.MemberId == request.MemberId &&
                    b.RenewDate == null);

            if (bookIssue == null) throw new ConflictException("No issue record found for this book and member.");


            if (bookIssue.RenewDate != null) throw new ConflictException("Book is already renewed once.");

            if (bookIssue.ReturnDate == null) throw new ConflictException("Return date missing, cannot renew.");


            var renewDate = DateOnly.FromDateTime(DateTime.Today);
            var renewReturnDate = renewDate.AddDays(15);

            bookIssue.RenewDate = renewDate;
            bookIssue.RenewReturnDate = renewReturnDate;

            _dbContext.SaveChanges();

            return new IssueDto(
                bookIssue.Id,
                bookIssue.Book!.Id,
                bookIssue.Member!.Id,
                bookIssue.IssueDate,
                bookIssue.ReturnDate,
                bookIssue.RenewDate,
                bookIssue.RenewReturnDate
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "Error while renewing book for MemberId {MemberId} and BookId {BookId}",
                request.MemberId, request.BookId);
        }

        return null;
    }
}