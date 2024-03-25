namespace BloomersIntegrationsManager.Domain.Extensions
{
    public static class BuilderExtensions
    {
        public static WebApplicationBuilder AddArchitectures (this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors();

            return builder;
        }
    }
}
