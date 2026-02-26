using LibraryManagementSystem.Core.Dtos;
using LibraryManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services.Services;

public sealed class MemberService
{
    private readonly AppDbContext _dbContext;

    public MemberService(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
  
    public IEnumerable<MemberDto> GetAll()
    {
        return _dbContext.Member
            .Select(m => new MemberDto(
                m.Id,
                m.MemberName,
                m.MemberType
            ))
            .ToList();
    }
    
    public MemberDto? GetById(int id)
    {
        return _dbContext.Member
            .Where(m => m.Id == id)
            .Select(m => new MemberDto(
                m.Id,
                m.MemberName,
                m.MemberType
            ))
            .FirstOrDefault();
    }

 
    public IEnumerable<MemberDto> GetByType(string memberType)
    {
        return _dbContext.Member
            .Where(m => m.MemberType == memberType)
            .Select(m => new MemberDto(
                m.Id,
                m.MemberName,
                m.MemberType
            ))
            .ToList();
    }
}