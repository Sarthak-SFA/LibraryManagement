using LibraryManagementSystem.Core.Dtos;
using LibraryManagementSystem.Core.Requests;
using LibraryManagementSystem.Services.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibraryManagementSystem.Web.Endpoints;

public static class BookEndpoints
{
    public static IEndpointRouteBuilder MapBookEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        IEndpointRouteBuilder bookGroup = endpoints.MapMasterGroup().MapGroup("Books");
        RouteGroupBuilder searchGroup = endpoints.MapMasterGroup().MapGroup("Search");
        RouteGroupBuilder categoryGroup = endpoints.MapMasterGroup().MapGroup("categories");

        bookGroup.MapGet("", GetAllBooks);
        bookGroup.MapGet("{id:int}", GetBookById);
        searchGroup.MapGet("{keyword}", GetAllSearch);
        categoryGroup.MapGet("{categoryId:int}/books", GetCategoryBooks);
        bookGroup.MapPost("category/{categoryId:int}", AddBook);


        return endpoints;
    }

    private static Ok<IEnumerable<BookDto>> GetAllBooks(BookService service)
    {
        return TypedResults.Ok(service.GetAll());
    }

    private static IResult GetBookById(BookService service, int id)
    {
        BookDto? book = service.GetById(id);

       return book == null ? TypedResults.NotFound():
       TypedResults.Ok(book);
    }

    private static Ok<IEnumerable<BookDto>> GetAllSearch(BookService service, string keyword)
    {
        return TypedResults.Ok(service.Search(keyword));
    }

    private static Ok<IEnumerable<BookDto>> GetCategoryBooks(BookService service, int categoryId)
    {
        return TypedResults.Ok(service.GetAllByCategory(categoryId));
    }
    
    public static IResult AddBook(BookService service, int categoryId,CreateBookRequest request)
    {
        BookDto? book  = service.AddBook(categoryId, request);
        return book == null ? TypedResults.NotFound() : TypedResults.Ok(book);
    }
}