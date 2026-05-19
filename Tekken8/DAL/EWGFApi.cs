using LogicLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace DAL
{
    public class EWGFApi : IEWGFApi
    {
        string connectionString;

        public EWGFApi(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("EWGFApi")!;

        }



        public async Task<List<Battle>> GetBattleDataAsync(string battleId)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.ewgf.gg/external/battles/{battleId}");
            request.Headers.Add("Authorization", connectionString);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            var battle = JsonConvert.DeserializeObject<BattleList>(json);

            return battle.Battles;
        }
    }
}
