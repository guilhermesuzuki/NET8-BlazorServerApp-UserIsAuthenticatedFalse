using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BlazorApp1;

public class CustomSignalRLoggingMiddleware : IHubFilter
{
    public CustomSignalRLoggingMiddleware()
    {
        
    }

    public async Task OnConnectedAsync(HubCallerContext context, Func<HubCallerContext, Task> next)
    {
        await next(context);
    }

    public async Task OnDisconnectedAsync(HubCallerContext context, Exception exception, Func<HubCallerContext, Exception, Task> next)
    {
        await next(context, exception);
    }

    public async Task<object> InvokeMethodAsync(HubInvocationContext invocationContext, Func<HubInvocationContext, Task<object>> next)
    {
        return await next(invocationContext);
    }
}
