using LibraryManagementSystem.Core.Dtos;
using LibraryManagementSystem.Core.Request;
using LibraryManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services.Services;

public sealed class IssueService
{
    private readonly AppDbContext _dbContext;

    public IssueService(AppDbContext dbContext)
    {
        _dbContext = dbContext?? throw new ArgumentNullException(nameof(dbContext)); 
    }
    public void Issue(IssueRequest request)
    {
        var member = _dbContext.Member.Find(request.MemberId)
                     ?? throw new Exception("Member not found");

        var activeCount = _dbContext.BookIssue
            .Count(x => x.MemberId == request.MemberId && x.ReturnDate == null);

        if (member.MemberType == "Premium" && activeCount >= 4)
            throw new ConflictException("Premium member max 4 books");

        if (member.MemberType == "Standard" && activeCount >= 2)
            throw new ConflictException("Standard member max 2 books");

        var alreadyIssued = _dbContext.BookIssue
            .Any(x => x.BookId == request.BookId && x.ReturnDate == null);

        if (alreadyIssued)
            throw new ConflictException("Book already issued");

        _dbContext.BookIssue.Add(new BookIssue
        {
            BookId = request.BookId,
            MemberId = request.MemberId,
            IssueDate = DateTime.Now
        });

        _dbContext.SaveChanges();
    }

    public void Renew(RenewRequest request)
    {
        var issue = _dbContext.BookIssue.Find(request.IssueId)
                    ?? throw new Exception("Issue not found");

        if (issue.RenewDate != null)
            throw new ConflictException("Book can renew only once");

        issue.RenewDate = DateTime.Now;
        issue.RenewReturnDate = DateTime.Now.AddDays(7);

        _dbContext.SaveChanges();
    }

    public IEnumerable<IssueDto> GetAll()
    {
        return _dbContext.BookIssue
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
            .ToList();
    }
}