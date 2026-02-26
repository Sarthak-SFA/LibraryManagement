using LibraryManagementSystem.Core.Dtos;
using LibraryManagementSystem.Services.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibraryManagementSystem.Web.Endpoints;

public static class CategoryEndpoints
{
    public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        RouteGroupBuilder categorygroup = endpoints.MapMasterGroup().MapGroup("Categories");

        categorygroup.MapGet("", GetAllCategories);
        //categorygroup.MapGet("{id:int}", GetCategoryById);
        categorygroup.MapGet("{id:int}", GetCategory);
        

        return endpoints;
    }

    private static Ok<IEnumerable<CategoryDto>> GetAllCategories(CategoryService service)
    {
        return TypedResults.Ok(service.GetAll());
    }

    /*private static IResult GetCategoryById(CategoryService service, int id)
    {
        var category = service.GetById(id);

        if (category is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(category);
    }*/
    
    private static IResult GetCategory(CategoryService service, int id)
    {
        CategoryWithBooksDto? category = service.GetCategory(id);

        return category == null ? TypedResults.NotFound() : TypedResults.Ok(category);
    }
    
    
}