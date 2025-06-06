namespace CinemaBooking.API.Middlewares;

public static class DdosProtectionMiddlewareExtensions
{
    public static IApplicationBuilder UseDdosProtection(this IApplicationBuilder app)
    {
        return app.UseMiddleware<DdosProtectionMiddleware>();
    }
    //ab -n 1000 -c 50 http://localhost:5290/api/User/all
    // This command to test ddos 
}