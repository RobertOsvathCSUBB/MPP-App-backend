using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using mpp_app_backend.Context;

namespace mpp_app_backend.Health
{
    public class HealthCheck : IHealthCheck
    {
        private readonly DataContext _context;

        public HealthCheck(DataContext context)
        {
            _context = context;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
        {
            try
            {
                _context.Database.ExecuteSqlRaw("SELECT 1");
                return Task.FromResult(HealthCheckResult.Healthy());
            }
            catch (Exception)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy());
            }
        }
    }
}
