using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using LogicLayer.Models;
using LogicLayer.Interfaces;

namespace DAL
{
    public class EWGFApi : IEWGFApi
    {
        private readonly string _connectionString;

        public EWGFApi(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("EWGFApi")!;
        }

        public async Task<List<Battle>> GetBattleDataAsync(string battleId)
        {
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.ewgf.gg/external/battles/{battleId}");

            request.Headers.Add("Authorization", _connectionString);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var battleList = JsonConvert.DeserializeObject<BattleList>(json);

            return battleList?.Battles ?? new List<Battle>();
        }
    }
}