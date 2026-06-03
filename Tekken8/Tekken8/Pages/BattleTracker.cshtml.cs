using DAL;
using LogicLayer.Models;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tekken8.Pages
{
    public class BattleTrackerModel : PageModel
    {
        private readonly BattleService _battleService;

        public BattleTrackerModel(IConfiguration configuration)
        {
            _battleService = new BattleService(new EWGFApi(configuration));
        }

        [BindProperty]
        public string TekkenID { get; set; }

        public List<Battle> Battles { get; set; } = new();

        public double WinRate { get; set; }

        // Win rate grouped by the character the player used
        public List<SingleCharacterWinRateStats> OwnCharacterWinRates { get; set; } = new();

        // Win rate grouped by opponent character
        public List<SingleCharacterWinRateStats> OpponentCharacterWinRates { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(TekkenID))
                return Page();

            Battles = await _battleService.GetBattleDataAsync(TekkenID);

            try
            {
                WinRate = _battleService.CalculateWinRate(Battles, TekkenID);
                OwnCharacterWinRates = _battleService.GetOwnCharacterWinRates(Battles, TekkenID);
                OpponentCharacterWinRates = _battleService.GetOpponentCharacterWinRates(Battles, TekkenID);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return Page();
        }
    }
}