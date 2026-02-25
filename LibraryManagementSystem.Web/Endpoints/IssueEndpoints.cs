using LibraryManagementSystem.Core.Dtos;
using LibraryManagementSystem.Core.Request;
using Microsoft.AspNetCore.Http.HttpResults;
using LibraryManagementSystem.Services.Services;

namespace LibraryManagementSystem.Web.Endpoints;

public static class IssueEndpoints
{
    public static IEndpointRouteBuilder MapIssueEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        
        RouteGroupBuilder issueGroup = endpoints.MapMasterGroup().MapGroup("issues");
      
        issueGroup.MapGet("", GetAllIssuedBooks);  

        return endpoints;
    }
    private static Ok<IEnumerable<IssueDto>> GetAllIssuedBooks(IssueService service)
    {
        return TypedResults.Ok(service.GetAll());
    }


}