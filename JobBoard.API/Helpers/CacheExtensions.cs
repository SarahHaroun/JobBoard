using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;


namespace JobBoard.API.Helpers
{
    public static class CacheExtensions   
    {
        public static IServiceCollection AddCacheServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Redis Output Caching  
            services.AddStackExchangeRedisOutputCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
            });

            // Define Cache Policies  
            services.AddOutputCache(options =>
            {
                options.AddPolicy("JobsCache", builder =>
                    builder
                        .Expire(TimeSpan.FromSeconds(60))
                        .Tag("jobs")
                        .SetVaryByQuery("*")); // Cache for 60 seconds, vary by all query params  

                options.AddPolicy("AdminCache", builder =>
                    builder
                        .Expire(TimeSpan.FromSeconds(120))
                        .Tag("admin")
                        .SetVaryByQuery("*")); // Cache for 120 seconds  

                options.AddPolicy("NotificationsCache", builder =>
                    builder
                        .Expire(TimeSpan.FromSeconds(30))
                        .Tag("notifications")
                        .SetVaryByQuery("*")); // Cache for 30 seconds  
            });

            return services;
        }
    }
}
