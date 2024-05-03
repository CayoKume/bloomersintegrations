using Hangfire;
using Microsoft.Net.Http.Headers;

namespace BloomersIntegrationsManager.Domain.Extensions
{
    public static class AppUseExtensions
    {
        public static IApplicationBuilder UseApplication(this IApplicationBuilder app, string? serverName)
        {
            app.UseCors(policy =>
                policy.WithOrigins("https://localhost:7083", "https://localhost:7083")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithHeaders(HeaderNames.ContentType)
            );

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            //app.UseHangfireDashboard();

            //RecurringJobsExtensions.AddRecurringJobs(serverName);

            return app;
        }
    }
}
