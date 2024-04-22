using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace mpp_app_backend.Health
{
    public class InternetHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
        {
            var client = new HttpClient();
            var response = await client.GetAsync("https://www.google.com");
            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy();
            }
            else
            {
                return HealthCheckResult.Unhealthy();
            }
        }
    }
}
