using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Auxiliar.Infra.CrossCutting.HealthChecks.Providers
{
    public class CustomHealthChecks : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken
        cancellationToken = default)
        {
            return Task.FromResult(new HealthCheckResult(status: HealthStatus.Unhealthy,
                                                         description: "API com problemas"));
        }
    }
}