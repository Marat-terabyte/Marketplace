using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace APIGateway
{
    public class CorsConfig
    {
        public string[]? Origins { get; set; }
    }
}
