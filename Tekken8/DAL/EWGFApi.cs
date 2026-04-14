using System;
using System.Collections.Generic;
using System.Text;
using LogicLayer;

namespace DAL
{
    public class EWGFApi : IEWGFApi
    {
        public async Task<string> GetBattleDataAsync(string battleId)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.ewgf.gg/external/battles/2YEH85DaLhE6");
            request.Headers.Add("Bearer", "ewgf_f2f48e1c962c4256a20b0d0f32dc539d");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
