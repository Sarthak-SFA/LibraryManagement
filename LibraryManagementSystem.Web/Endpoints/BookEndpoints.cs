using LibraryManagementSystem.Core.Dtos;
using LibraryManagementSystem.Services.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibraryManagementSystem.Web.Endpoints;

public static class BookEndpoints
{
    public static IEndpointRouteBuilder MapBookEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var bookGroup = endpoints.MapMasterGroup().MapGroup("Books");
        var searchGroup = endpoints.MapMasterGroup().MapGroup("Search");
        var categoryGroup = endpoints.MapMasterGroup().MapGroup("categories");

        bookGroup.MapGet("", GetAllBooks);
        bookGroup.MapGet("{id:int}", GetBookById);
        searchGroup.MapGet("{keyword}", GetAllSearch);
        categoryGroup.MapGet("{categoryId:int}/books", GetCategoryBooks);


        return endpoints;
    }

    private static Ok<IEnumerable<BookDto>> GetAllBooks(BookService service)
    {
        return TypedResults.Ok(service.GetAll());
    }

    private static IResult GetBookById(BookService service, int id)
    {
        var book = service.GetById(id);

        if (book is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(book);
    }

    private static Ok<IEnumerable<BookDto>> GetAllSearch(BookService service, string keyword)
    {
        return TypedResults.Ok(service.Search(keyword));
    }

    private static Ok<IEnumerable<BookDto>> GetCategoryBooks(BookService service, int categoryId)
    {
        return TypedResults.Ok(service.GetAllByCategory(categoryId));
    }
}