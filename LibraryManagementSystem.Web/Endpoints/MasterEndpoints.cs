namespace LibraryManagementSystem.Web.Endpoints;

public static class MasterEndpoints
{
    public static IEndpointRouteBuilder MapMasterGroup(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGroup("master");
    }
}