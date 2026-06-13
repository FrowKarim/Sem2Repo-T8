using LogicLayer;
using LogicLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tekken8.Pages
{
    public class ComparePlayersModel : PageModel
    {
        private readonly BattleService _battleService;

        public ComparePlayersModel(BattleService battleService)
        {
            _battleService = battleService;
        }

        [BindProperty(SupportsGet = true)]
        public string Player1TekkenId { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string Player2TekkenId { get; set; } = string.Empty;

        public BattleComparisonResult? ComparisonResult { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrWhiteSpace(Player1TekkenId) || string.IsNullOrWhiteSpace(Player2TekkenId))
            {
                ModelState.AddModelError(string.Empty, "Both player TekkenIDs are required.");
                return Page();
            }

            try
            {
                var player1Battles = await _battleService.GetBattleDataAsync(Player1TekkenId);
                var player2Battles = await _battleService.GetBattleDataAsync(Player2TekkenId);

                ComparisonResult = _battleService.CompareBattleData(
                    player1Battles,
                    Player1TekkenId,
                    player2Battles,
                    Player2TekkenId);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return Page();
        }
    }
}