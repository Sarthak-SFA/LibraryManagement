using LibraryManagementSystem.Core.Dtos;
using LibraryManagementSystem.Core.Requests;
using LibraryManagementSystem.Persistence;
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
        issueGroup.MapPost("add", AddIssueRequest);

        return endpoints;
    }

    private static Ok<IEnumerable<IssueDto>> GetAllIssuedBooks(IssueService service)
    {
        return TypedResults.Ok(service.GetAll());
    }

    private static IResult AddIssueRequest(IssueService service, CreateBookIssueRequest request)
    {
        IssueDto? bookIssue = service.AddIssueRequest(request);

        return bookIssue == null
            ? TypedResults.BadRequest("Unable to Create Book Issue Request")
            : TypedResults.Ok(bookIssue);
    }

}