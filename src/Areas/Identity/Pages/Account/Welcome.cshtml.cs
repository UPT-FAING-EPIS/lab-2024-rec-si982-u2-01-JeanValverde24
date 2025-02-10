using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shorten.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class WelcomeModel : PageModel
    {
        public string Email { get; set; } = string.Empty;

        public void OnGet(string email)
        {
            Email = email;
        }
    }
}