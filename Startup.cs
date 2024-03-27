using System;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using CIS424_API;

[assembly: OwinStartup(typeof(Startup))]

namespace CIS424_API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);

            // Configure basic authentication
            ConfigureBasicAuth(app);
        }

        private void ConfigureBasicAuth(IAppBuilder app)
        {
            app.Use(async (context, next) =>
            {
                string authHeader = context.Request.Headers["Authorization"];

                if (authHeader != null && authHeader.StartsWith("Basic"))
                {
                    // Extract credentials
                    string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                    byte[] decodedBytes = Convert.FromBase64String(encodedUsernamePassword);
                    string decodedCredentials = System.Text.Encoding.UTF8.GetString(decodedBytes);
                    string[] usernamePasswordArray = decodedCredentials.Split(':');
                    string username = usernamePasswordArray[0];
                    string password = usernamePasswordArray[1];

                    // Check if credentials are valid (hardcoded for demonstration)
                    if (IsUserValid(username, password))
                    {
                        await next.Invoke();
                        return;
                    }
                }

                // Send authentication challenge
                context.Response.StatusCode = 401;
                context.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"My Realm\"");
                await context.Response.WriteAsync("Unauthorized");
            });
        }

        // Method to validate username and password (hardcoded for demonstration)
        private bool IsUserValid(string username, string password)
        {
            // Hardcoded username and password for demonstration purposes
            return username == "admin" && password == "password";
        }
    }
}
