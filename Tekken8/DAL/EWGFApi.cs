using LogicLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class EWGFApi : IEWGFApi
    {
        public async Task<Battle> GetBattleDataAsync(string battleId)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.ewgf.gg/external/battles/2YEH85DaLhE6");
            request.Headers.Add("Authorization", "Bearer ewgf_f2f48e1c962c4256a20b0d0f32dc539d");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            var battle = JsonConvert.DeserializeObject<Battle>(json);

            return battle;
        }
    }
}
