using LogicLayer;
using LogicLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Tekken8.Pages
{
    public class BattleTrackerModel : PageModel
    {
        private readonly BattleService _battleService;

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

        public bool HasSearched { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            HasSearched = true;

            if (string.IsNullOrWhiteSpace(TekkenID))
            {
                ModelState.AddModelError(string.Empty, "Please enter a TekkenID.");
                return Page();
            }

            TekkenID = TekkenID.Trim();

            if (!Regex.IsMatch(TekkenID, "^[A-Za-z0-9]{6,20}$"))
            {
                ModelState.AddModelError(string.Empty, "Please enter a valid TekkenID.");
                return Page();
            }

            try
            {
                Battles = await _battleService.GetBattleDataAsync(TekkenID);

                if (Battles == null || !Battles.Any())
                {
                    ModelState.AddModelError(string.Empty, "No battles found for this TekkenID.");
                    return Page();
                }

                WinRate = _battleService.CalculateTotalWinRate(Battles, TekkenID);
                OwnCharacterWinRates = _battleService.GetOwnCharacterWinRates(Battles, TekkenID);
                OpponentCharacterWinRates = _battleService.GetOpponentCharacterWinRates(Battles, TekkenID);
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "Could not retrieve battle data from the API.");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while loading battle data.");
            }

            return Page();
        }
    }
}