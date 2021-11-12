using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ReceiptApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient
            {
                //BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
                BaseAddress = new Uri("https://63cndordf2.execute-api.eu-west-1.amazonaws.com/v1/")
            });

            builder.Services.AddTransient<ReceiptApp.Services.ReceiptQuery>();
            builder.Services.AddTransient<ReceiptApp.Services.ReceiptCommand>();
            builder.Services.AddTransient<ReceiptApp.Services.JobsQuery>();

            builder.Services.AddOidcAuthentication(options =>
            {
                // Configure your authentication provider options here.
                // For more information, see https://aka.ms/blazor-standalone-auth
                builder.Configuration.Bind("Local", options.ProviderOptions);
            });


            builder.Services.AddSingleton(new Configuration.AuthenticationConfig
            {
                Authority = builder.Configuration["Local:Authority"],
                ClientId = builder.Configuration["Local:ClientId"],
            });

            builder.Services.AddTransient<Services.TokenService>();
            builder.Services.AddTransient<Services.ReceiptApi>();

            await builder.Build().RunAsync();
        }
    }
}
