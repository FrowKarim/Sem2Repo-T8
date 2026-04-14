using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LogicLayer;

namespace Tekken8.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BattleService _battleService;

        public IndexModel(BattleService battleService)
        {
            _battleService = battleService;
        }

        public async Task OnGetAsync()
        {
            // Call GetBattleDataAsync
            //var battleData = await _battleService.GetBattleDataAsync("2YEH85DaLhE6");
        }
    }
}
