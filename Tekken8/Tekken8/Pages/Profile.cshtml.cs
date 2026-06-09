using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tekken8.Pages
{
    public class ProfileModel : PageModel
    {
        public int? UserId { get; set; }
        public string Username { get; set; }
        public string IsAdmin { get; set; }

        public IActionResult OnGet()
        {
            UserId = HttpContext.Session.GetInt32("UserId");

            if (!UserId.HasValue)
            {
                return RedirectToPage("/Login");
            }

            Username = HttpContext.Session.GetString("Username");
            IsAdmin = HttpContext.Session.GetString("IsAdmin");

            return Page();
        }
    }
}