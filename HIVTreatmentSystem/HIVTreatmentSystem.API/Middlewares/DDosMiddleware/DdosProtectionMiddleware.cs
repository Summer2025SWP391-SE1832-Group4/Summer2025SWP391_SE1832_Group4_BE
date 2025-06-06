using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading.Tasks;

namespace CinemaBooking.API.Middlewares
{
    public class DdosProtectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<DdosProtectionMiddleware> _logger;

        private const int RequestLimit = 10; // Max requests from an IP
        private static readonly TimeSpan RequestLimitTimeSpan = TimeSpan.FromSeconds(10); // Time window
        private static readonly ConcurrentDictionary<string, (DateTime Time, int Count)> RequestCounts = new(); // Tracks requests per IP

        public DdosProtectionMiddleware(RequestDelegate next, ILogger<DdosProtectionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();

            if (ipAddress == null)
            {
                await _next(context);
                return;
            }

            var now = DateTime.UtcNow;

            // Use `AddOrUpdate` to ensure atomic updates
            var entry = RequestCounts.AddOrUpdate(ipAddress, 
                _ => (now, 1),  // If IP does not exist, create a new entry
                (_, oldEntry) => (now - oldEntry.Time < RequestLimitTimeSpan) 
                    ? (oldEntry.Time, oldEntry.Count + 1)  // If within limit, increment count
                    : (now, 1) // Otherwise, reset counter
            );

            if ((now - entry.Time) < RequestLimitTimeSpan && entry.Count > RequestLimit)
            {
                _logger.LogWarning($"IP {ipAddress} is blocked due to excessive requests.");
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                await context.Response.WriteAsync("Too many requests. Please try again later.");
                return;
            }

            await _next(context);
        }
    }

    
}
