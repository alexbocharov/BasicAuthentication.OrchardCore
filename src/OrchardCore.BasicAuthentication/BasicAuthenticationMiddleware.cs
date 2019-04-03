using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Users;

namespace OrchardCore.BasicAuthentication
{
    public class BasicAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                string authorization = context.Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authorization))
                {
                    if (authorization.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                    {
                        var encodedCredentials = authorization.Substring("Basic ".Length).Trim();
                        if (!string.IsNullOrEmpty(encodedCredentials))
                        {
                            var decodedCredentials = Convert.FromBase64String(encodedCredentials);
                            var credentials = Encoding.ASCII.GetString(decodedCredentials).Split(':');

                            var userName = credentials[0];
                            var password = credentials[1];

                            var userManager = context.RequestServices.GetRequiredService<UserManager<IUser>>();
                            var signInManager = context.RequestServices.GetRequiredService<SignInManager<IUser>>();

                            var user = await userManager.FindByNameAsync(userName);
                            if (await userManager.CheckPasswordAsync(user, password))
                            {
                                context.User = await signInManager.CreateUserPrincipalAsync(user);
                            }
                        }
                    }
                }
            }

            await _next.Invoke(context);
        }
    }
}
