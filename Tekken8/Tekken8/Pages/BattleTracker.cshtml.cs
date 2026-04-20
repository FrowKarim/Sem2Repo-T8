using DAL;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tekken8.Pages
{
    public class BattleTrackerModel : PageModel
    {
        private readonly BattleService _battleService;

        public BattleTrackerModel()
        {
            _battleService = new BattleService(new EWGFApi());
        }

        [BindProperty]
        public string TekkenID { get; set; }

        public List<Battle> Battles { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(TekkenID))
            {
                return Page();
            }

            Battles = await _battleService.GetBattleDataAsync(TekkenID);

            return Page();
        }
    }
}