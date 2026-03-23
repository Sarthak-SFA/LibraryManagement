using LibraryManagementSystem.Core.Dtos;
using LibraryManagementSystem.Core.Request;
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
        issueGroup.MapPut("update", RenewBook);


        return endpoints;
    }

    private static Ok<IEnumerable<IssueDto>> GetAllIssuedBooks(IssueService service)
    {
        return TypedResults.Ok(service.GetAll());
    }

    private static IResult AddIssueRequest(IssueService IssueService, CreateBookIssueRequest request)
    {
        try
        {
            var bookIssue = IssueService.AddIssueRequest(request);

            return bookIssue == null
                ? TypedResults.BadRequest("Unable to create book issue request. See Logs")
                : TypedResults.Ok(bookIssue);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.Message);
        }
    }

    private static IResult RenewBook(IssueService service, RenewBookRequest request)
    {
        var result = service.RenewBook(request);

        return result == null
            ? TypedResults.BadRequest("Unable to renew book")
            : TypedResults.Ok(result);
    }
}