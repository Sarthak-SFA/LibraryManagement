using LibraryManagementSystem.Core.Dtos;
using LibraryManagementSystem.Core.Request;
using LibraryManagementSystem.Services.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibraryManagementSystem.Web.Endpoints;

public static class MemberEndpoints
{
    public static IEndpointRouteBuilder MapMemberEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        
        IEndpointRouteBuilder memberGroup = endpoints.MapMasterGroup().MapGroup("Members");


        memberGroup.MapGet("", GetAllMembers);
        memberGroup.MapGet("{id:int}", GetMemberById);
        memberGroup.MapGet("type/{memberType}", GetMembersByType);
        memberGroup.MapPost("add" , AddMember);

        return endpoints;
    }

    private static Ok<IEnumerable<MemberDto>> GetAllMembers(MemberService service)
    {
        return TypedResults.Ok(service.GetAll());
    }

    private static IResult GetMemberById(MemberService service, int id)
    {
        MemberDto? member = service.GetById(id);

        return member == null ? TypedResults.NotFound():
            TypedResults.Ok(member);
    }

    private static Ok<IEnumerable<MemberDto>> GetMembersByType(MemberService service, string memberType)
    {
        return TypedResults.Ok(service.GetByType(memberType));
    }

    public static IResult AddMember(MemberService service, CreateMemberRequest request)
    {
        MemberDto? member = service.AddMember(request);
        return member == null ? TypedResults.NotFound() :  TypedResults.Ok(member);
    }
}