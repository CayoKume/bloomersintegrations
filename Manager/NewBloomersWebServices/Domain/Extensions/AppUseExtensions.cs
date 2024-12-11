using Hangfire;
using Microsoft.Net.Http.Headers;

namespace BloomersIntegrationsManager.Domain.Extensions
{
    public static class AppUseExtensions
    {
        public static IApplicationBuilder UseApplication(this IApplicationBuilder app, string? serverName)
        {
            if (serverName == "SRV-VM-APP03")
            {
                app.UseCors(policy =>
                    policy.WithOrigins("http://172.25.1.6:7575", "http://localhost:7575", "http://localhost:5215", "https://localhost:7083", "https://webapplication.newbloomers.com.br:7475")
                    .WithMethods("PUT", "DELETE", "GET", "POST")
                    //.AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithHeaders(HeaderNames.ContentType)
                );

                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHttpsRedirection();
                app.UseAuthorization();
            }
            else
            {
                app.UseHangfireDashboard();
                RecurringJobsExtensions.AddRecurringJobs(serverName);
            }

            return app;
        }
    }
}
