using LibraryManagementSystem.Core.Dtos;
using LibraryManagementSystem.Services.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibraryManagementSystem.Web.Endpoints;

public static class IssueEndpoints
{
    public static IEndpointRouteBuilder MapIssueEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        IEndpointRouteBuilder issueGroup = endpoints.MapMasterGroup().MapGroup("issues");

        issueGroup.MapGet("", GetAllIssuedBooks);

        return endpoints;
    }

    private static Ok<IEnumerable<IssueDto>> GetAllIssuedBooks(IssueService service)
    {
        return TypedResults.Ok(service.GetAll());
    }
}