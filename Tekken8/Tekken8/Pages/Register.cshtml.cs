using LogicLayer;
using LogicLayer.Models;
using LogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Tekken8.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserService _userService;

        public RegisterModel(UserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public string TekkenID { get; set; }
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var user = new User
                {
                    Username = Input.Username,
                    Email = Input.Email,
                    PasswordHash = Input.Password,
                    TekkenID = Input.TekkenID,
                    IsAdmin = false
                };

                _userService.AddUser(user);

                SuccessMessage = "Account created successfully. You can now log in.";
                ModelState.Clear();
                Input = new InputModel();

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}