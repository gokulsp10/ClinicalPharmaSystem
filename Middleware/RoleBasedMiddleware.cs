using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

public class RoleBasedMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string[] _allowedPaths;

    public RoleBasedMiddleware(RequestDelegate next, params string[] allowedPaths)
    {
        _next = next;
        _allowedPaths = allowedPaths.Select(path => path.StartsWith("/") ? path : "/" + path).ToArray();
    }

    public async Task Invoke(HttpContext context)
    {
        // Check if the requested path is in the allowed paths
        var requestedPath = context.Request.Path;
        if (_allowedPaths.Any(path => requestedPath.StartsWithSegments(path)))
        {
            // Allow access to the allowed paths without role-based check
            await _next(context);
            return;
        }

        // Check if the user is authenticated
        if (context.User.Identity.IsAuthenticated)
        {
            // Check if the user has the required role
            if (context.User.IsInRole("Doctor") || context.User.IsInRole("Admin") || context.User.IsInRole("Pharmacy") || context.User.IsInRole("Lab Technician"))
            {
                // User has the required role, allow access to the route
                await _next(context);
            }
            else
            {
                // User does not have the required role, redirect or deny access
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("Access Denied. You don't have the required role.");
            }
        }
        else
        {
            // User is not authenticated, redirect to the login page
            context.Response.Redirect("/Home/Login");
        }
    }
}
