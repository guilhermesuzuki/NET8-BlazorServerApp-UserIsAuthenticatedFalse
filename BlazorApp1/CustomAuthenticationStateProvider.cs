using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorApp1;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthorizationService _authorizationService;

    public CustomAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService)
    {
        _httpContextAccessor = httpContextAccessor;
        _authorizationService = authorizationService;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Is the user authenticated? {_httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated}");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Connection {_httpContextAccessor.HttpContext?.Connection.Id}");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Is websocket request? {_httpContextAccessor.HttpContext?.WebSockets.IsWebSocketRequest}");
        Console.WriteLine();

        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "email@dummy.com"));
        identity.AddClaim(new Claim(ClaimTypes.Name, "dummy"));
        identity.AddClaim(new Claim(ClaimTypes.Role, "User"));

        var principal = new ClaimsPrincipal(identity);

        return Task.FromResult(new AuthenticationState(principal));
    }

    public async Task<bool> IsUserInPolicyAsync(string policyName)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            return false;
        }

        var authorizationResult = await _authorizationService.AuthorizeAsync(user, policyName);
        return authorizationResult.Succeeded;
    }
}
