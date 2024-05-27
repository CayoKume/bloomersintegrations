namespace BloomersIntegrationsManager.Domain.Extensions
{
    public static class BuilderExtensions
    {
        public static WebApplicationBuilder AddArchitectures (this WebApplicationBuilder builder, string? serverName)
        {
            if (serverName == "SRV-VM-APP03")
            {
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
                builder.Services.AddCors();
            }
            
            return builder;
        }
    }
}
