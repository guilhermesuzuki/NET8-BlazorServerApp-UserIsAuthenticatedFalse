using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BlazorApp1.Pages
{
    [ValidateAntiForgeryToken]
    public class Login2Model : PageModel
    {
        public Login2Model(): base()
        {
            
        }

        public void OnGet()
        {

        }

        public async Task OnPost()
        {
            var properties = new AuthenticationProperties() { IsPersistent = true };
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "email@dummy.com"));
            identity.AddClaim(new Claim(ClaimTypes.Name, "dummy"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "User"));

            var principal = new ClaimsPrincipal(identity);

            await this.HttpContext.SignInAsync("Two", principal, properties);
            Redirect("/counter-2");
        }
    }
}
