using Microsoft.AspNetCore.Builder;

namespace OrchardCore.BasicAuthentication
{
    public static class BasicAuthenticationApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseBasicAuthentication(this IApplicationBuilder app)
            => app.UseMiddleware<BasicAuthenticationMiddleware>();
    }
}