// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace MvcSandbox.Middleware;

public class FeatureSwitchMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate next = next;

    public async Task Invoke(HttpContext context, IConfiguration config)
    {
        if (context.Request.Path.Value.Contains("/features"))
        {
            var switches = config.GetSection("FeatureSwitches");
            var report = switches.GetChildren().Select(x => $"{x.Key} : {x.Value}");
            await context.Response.WriteAsync(string.Join(Environment.NewLine, report));
        }
        else
        {
            await next?.Invoke(context);
        }
    }
}
