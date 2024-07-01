using BlazorApp1.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddAuthentication()
.AddCookie(options =>
{
    options.LoginPath = "/login1";
    options.Cookie.Name = "cookie-1";
    options.Cookie.HttpOnly = false;
    options.ExpireTimeSpan = new TimeSpan(0, 0, 3, 0);
    options.Events = new()
    {
        OnSigningIn = context =>
        {
            return Task.CompletedTask;
        }
    };

    options.ForwardDefaultSelector = context =>
    {
        return "Cookies";
    };
})
.AddCookie("Two", "Two", options =>
{
    options.LoginPath = "/login2";
    options.Cookie.Name = "cookie-2";
    options.Cookie.HttpOnly = false;
    options.ExpireTimeSpan = new TimeSpan(0, 0, 3, 0);
    options.Events = new()
    {
        OnSigningIn = context =>
        {
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CustomPolicy1", policy => policy.RequireAuthenticatedUser().AddAuthenticationSchemes("Cookies"));
    options.AddPolicy("CustomPolicy2", policy => policy.RequireAuthenticatedUser().AddAuthenticationSchemes("Two"));

    var policy = new AuthorizationPolicy(
        new List<IAuthorizationRequirement>() { new AssertionRequirement(context => 
        { 
            return true; 
        })},
        new List<string> { }
        );

    options.DefaultPolicy = policy;
});

builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

app.UseAuthentication();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.MapRazorPages();

app.Run();
