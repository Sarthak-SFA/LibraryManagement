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