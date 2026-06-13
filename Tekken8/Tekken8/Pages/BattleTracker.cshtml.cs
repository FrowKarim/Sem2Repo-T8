using LogicLayer;
using LogicLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tekken8.Pages
{
    public class BattleTrackerModel : PageModel
    {
        private readonly BattleService _battleService;

        // op deze manier gedaan zodat caching werkt
        public BattleTrackerModel(BattleService battleService)
        {
            
            _battleService = battleService;
        }

        [BindProperty]
        public string TekkenID { get; set; } = string.Empty;

        public List<Battle> Battles { get; set; } = new();

        public double WinRate { get; set; }

        public List<SingleCharacterWinRateStats> OwnCharacterWinRates { get; set; } = new();

        public List<SingleCharacterWinRateStats> OpponentCharacterWinRates { get; set; } = new();

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