using Microsoft.AspNetCore.Http.HttpResults;
using LibraryManagementSystem.Core.Dtos;
using LibraryManagementSystem.Persistence;
using LibraryManagementSystem.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Web.Endpoints;

public static class BookEndpoints
{
    public static IEndpointRouteBuilder MapBookEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        
        RouteGroupBuilder bookGroup = endpoints.MapMasterGroup().MapGroup("Books");
        IEndpointRouteBuilder searchGroup = endpoints.MapMasterGroup().MapGroup("Search");

        bookGroup.MapGet("", GetAllBooks);
           

        searchGroup.MapGet("search/{keyword}", Search);

        return endpoints;
    }
    private static Ok<IEnumerable<BookDto>> GetAllBooks(BookService service)
    {
        return TypedResults.Ok(service.GetAll());
    }

    private static Ok<IEnumerable<BookDto>> Search(BookService service,[FromQuery] string keyword)
    {
        return TypedResults.Ok(service.Search( keyword));
    }

}