using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MinimalBlazorAdmin.Client.Services;
using MinimalBlazorAdmin.Shared;

namespace MinimalBlazorAdmin.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            
            // authentication
            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
                options.AddPolicy(Policies.IsUser, Policies.IsUserPolicy());
            });
            
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();
            
            // web storage
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddScoped<IAuthService, AuthService>();
            
            await builder.Build().RunAsync();
        }
    }
}
