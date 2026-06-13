using DAL;
using LogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LogicLayer;

namespace Tekken8.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BattleService _battleService;

        public IndexModel(IConfiguration configuration)
        {
          //_battleService = new BattleService(new EWGFApi(configuration)); Dit crashed door de caching drm doe ik het niet in battletracker.cshtml.cs
        }

        public async Task OnGetAsync()
        {
            // Call GetBattleDataAsync
            //TESTING
            //var battleData = await _battleService.GetBattleDataAsync("2YEH85DaLhE6");
        }
    }
}
