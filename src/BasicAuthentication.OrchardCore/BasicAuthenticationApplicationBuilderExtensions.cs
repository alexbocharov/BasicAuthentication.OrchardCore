using Microsoft.AspNetCore.Builder;

namespace BasicAuthentication.OrchardCore
{
    public static class BasicAuthenticationApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseBasicAuthentication(this IApplicationBuilder app)
            => app.UseMiddleware<BasicAuthenticationMiddleware>();
    }
}
