using System.Collections.ObjectModel;
using LibraryManagementSystem.Core.Dtos;
using LibraryManagementSystem.Core.Request;
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
                m.MemberName!,
                m.MemberType!,
                m.MemberTypeID
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
                m.MemberName!,
                m.MemberType!,
                m.MemberTypeID
            ))
            .FirstOrDefault();
    }


    public IEnumerable<MemberDto> GetByType(string memberType)
    {
        IList<MemberDto> member = _dbContext.Member
            .Where(m => m.MemberType == memberType)
            .Select(m => new MemberDto(
                m.Id,
                m.MemberName!,
                m.MemberType!,
                m.MemberTypeID
            ))
            .ToArray();
        return new ReadOnlyCollection<MemberDto>(member);
    }

    public MemberDto? AddMember( CreateMemberRequest request)
    {
        try
        {
            Member? member = _dbContext.Member.FirstOrDefault(m =>
                m.MemberName == request.MemberName && m.MemberType == request.MemberType && m.MemberTypeID == request.MemberTypeID);
            
            if (member is not null)
            {
                throw new ConflictException($"Member with name {request.MemberName} already exists.");
            }

            member = new Member
            {
                MemberName = request.MemberName,
                MemberType = request.MemberType,
                MemberTypeID = request.MemberTypeID
            };

            _dbContext.Add(member);
            _dbContext.SaveChanges();

            return new MemberDto(
                member.Id,
                member.MemberName!,
                member.MemberType!,
                member.MemberTypeID);
        }
        catch (ConflictException ex)
        {
            _logger.LogError(ex,
                "Error while adding member with name {MemberName}. Problem in execution of Sql query.",
                request.MemberName);
        }
        catch (Exception e)
        {
            _logger.LogError (e,
                "Error while adding member with name {@member}.}",
                request
                );
        }
        return null;
    }
    
}