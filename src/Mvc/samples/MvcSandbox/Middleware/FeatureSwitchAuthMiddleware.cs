// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Components;

namespace MvcSandbox.Middleware;

public class FeatureSwitchAuthMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate next = next;

    public async Task Invoke(HttpContext context, IConfiguration config)
    {
        var endpoint = context.GetEndpoint()?.Metadata.GetMetadata<RouteAttribute>();
        if (endpoint is not null)
        {
            var featureSwitch = config.GetSection("FeatureSwitches")
                .GetChildren().FirstOrDefault(x => x.Key == endpoint.Template);
            if (featureSwitch is not null && !bool.Parse(featureSwitch.Value))
            {
                context.SetEndpoint(new Endpoint((httpcontext) =>
                {
                    httpcontext.Response.StatusCode = StatusCodes.Status404NotFound;
                    return Task.CompletedTask;
                },
                EndpointMetadataCollection.Empty, "Feature Not Found"));
            }
        }
        else
        {
            await next?.Invoke(context);
        }
    }
}
