using AspNetCoreRateLimit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PaymentGateway.Common
{
    public static class RateLimitConfig
    {
        public static void AddRateLimit(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddOptions();
            service.AddMemoryCache();
            service.AddInMemoryRateLimiting();
            service.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
            service.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));

            service.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            service.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            service.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        }
    }
}
