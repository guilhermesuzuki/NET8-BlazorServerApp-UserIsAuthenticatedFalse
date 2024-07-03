
namespace BlazorApp1;

public class CustomEndpointFilter : IEndpointFilter
{
    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        next(context);
        return new ValueTask<object?>();
    }
}
