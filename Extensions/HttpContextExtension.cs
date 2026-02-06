using Microsoft.AspNetCore.Http;

namespace Gooios.BuildingBlocks.Extensions;

public static class HttpContextExtension
{
    public static bool IsApiRequest(this HttpContext httpContext)  // Renamed for clarity
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        return httpContext.Request.Path.StartsWithSegments("/api");
    }

    // If you actually need to check for AJAX:
    public static bool IsAjaxRequest(this HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        return httpContext.Request.Headers.XRequestedWith == "XMLHttpRequest";
    }
}