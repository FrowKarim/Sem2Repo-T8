using LogicLayer.Models;
using LogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tekken8.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly UserService _userService;

        public ProfileModel(UserService userService)
        {
            _userService = userService;
        }

        public User CurrentUser { get; set; }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string TekkenID { get; set; }

        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToPage("/Login");
            }

            CurrentUser = _userService.GetUserById(userId.Value);

            if (CurrentUser == null)
            {
                return RedirectToPage("/Login");
            }

            Name = CurrentUser.Username;   
            TekkenID = CurrentUser.TekkenID; 

            return Page();
        }

        public IActionResult OnPost()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToPage("/Login");
            }

            CurrentUser = _userService.GetUserById(userId.Value);

            if (CurrentUser == null)
            {
                return RedirectToPage("/Login");
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                ModelState.AddModelError(string.Empty, "Name is required.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            CurrentUser.Username = Name;      
            CurrentUser.TekkenID = TekkenID;  

            _userService.UpdateUser(CurrentUser);

            return RedirectToPage();
        }
    }
}