using System.Collections.ObjectModel;
using LibraryManagementSystem.Core.Dtos;
using LibraryManagementSystem.Persistence;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Services.Services;

public sealed class MemberService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<MemberService> _logger;

    public MemberService(AppDbContext dbContext , ILogger<MemberService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger;
    }

    public IEnumerable<MemberDto> GetAll()
    {
        IList<MemberDto> member = _dbContext.Member
            .Select(m => new MemberDto(
                m.Id,
                m.MemberName,
                m.MemberType
            ))
            .ToArray();
        return new ReadOnlyCollection<MemberDto>(member);
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
        IList<MemberDto> member = _dbContext.Member
            .Where(m => m.MemberType == memberType)
            .Select(m => new MemberDto(
                m.Id,
                m.MemberName,
                m.MemberType
            ))
            .ToArray();
        return new ReadOnlyCollection<MemberDto>(member);
    }
}