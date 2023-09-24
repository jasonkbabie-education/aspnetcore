// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using MvcSandbox.Middleware;

namespace MvcSandbox;

public class Startup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddRazorPages();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app)
    {
        app.UseMiddleware<FeatureSwitchMiddleware>();
        app.UseDeveloperExceptionPage();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseMiddleware<FeatureSwitchAuthMiddleware>();
        static void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
        {
            //endpoints.MapGet("/MapGet", () => "MapGet");

            //endpoints.MapControllers();
            //endpoints.MapControllerRoute(
            //    Guid.NewGuid().ToString(),
            //    "{controller=Home}/{action=Index}/{id?}");

            endpoints.MapRazorPages();
        }

        app.UseEndpoints(builder =>
        {
            builder.MapDefaultControllerRoute();
            ConfigureEndpoints(builder);
            //var group = builder.MapGroup("/group");
            //ConfigureEndpoints(group);
        });
    }

    public static void Main(string[] args)
    {
        var host = CreateWebHostBuilder(args)
            .Build();

        host.Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        new WebHostBuilder()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureLogging(factory =>
            {
                factory
                    .AddConsole()
                    .AddDebug();
            })
            .ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile("appsettings.json");
            })
            .UseKestrel()
            .UseStartup<Startup>();
}

