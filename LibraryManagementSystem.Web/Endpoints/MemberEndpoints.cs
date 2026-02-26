using LibraryManagementSystem.Core.Dtos;
using LibraryManagementSystem.Services.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibraryManagementSystem.Web.Endpoints;

public static class MemberEndpoints
{
    public static IEndpointRouteBuilder MapMemberEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        
        RouteGroupBuilder memberGroup = endpoints.MapMasterGroup().MapGroup("Members");
       

        memberGroup.MapGet("", GetAllMembers);                   
        memberGroup.MapGet("{id:int}", GetMemberById);           
        memberGroup.MapGet("type/{memberType}", GetMembersByType);

        return endpoints;
    }

    private static Ok<IEnumerable<MemberDto>> GetAllMembers(MemberService service)
    {
        return TypedResults.Ok(service.GetAll());
    }

    private static IResult GetMemberById(MemberService service, int id)
    {
        var member = service.GetById(id);

        if (member is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(member);
    }

    private static Ok<IEnumerable<MemberDto>> GetMembersByType(MemberService service, string memberType)
    {
        return TypedResults.Ok(service.GetByType(memberType));
    }
}