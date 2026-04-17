using DAL;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tekken8.Pages
{
    public class BattleTrackerModel : PageModel
    {
        private readonly BattleService _battleService;
        public string TekkenID { get; set; }
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Handle form submission logic here
            // For example, you can process the form data and update the battle tracker
            // After processing, redirect to the same page or another page
            BattleService battleService = new BattleService(new EWGFApi());
            var battleData = await battleService.GetBattleDataAsync("TekkenID");
            return RedirectToPage("/BattleTracker");
        }
    }
}
