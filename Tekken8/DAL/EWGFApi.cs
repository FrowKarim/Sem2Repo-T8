using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using LogicLayer.Models;
using LogicLayer.Interfaces;
using System.Net.Http;

namespace DAL
{
    public class EWGFApi : IEWGFApi
    {
        private readonly string _apiKey;

        public EWGFApi(IConfiguration configuration)
        {
            _apiKey = configuration.GetConnectionString("EWGFApi")!;
        }

        public async Task<List<Battle>> GetBattleDataAsync(string tekkenId)
        {
            if (string.IsNullOrWhiteSpace(tekkenId))
                throw new ArgumentException("TekkenID cannot be empty.", nameof(tekkenId));

            tekkenId = tekkenId.Trim();

            try
            {
                using var client = new HttpClient();
                using var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    $"https://api.ewgf.gg/external/battles/{tekkenId}");

                request.Headers.Add("Authorization", _apiKey);

                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        return new List<Battle>();

                    throw new HttpRequestException($"EWGF API returned status code {(int)response.StatusCode} ({response.StatusCode}).");
                }

                var json = await response.Content.ReadAsStringAsync();
                var battleList = JsonConvert.DeserializeObject<BattleList>(json);

                return battleList?.Battles ?? new List<Battle>();
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (TaskCanceledException ex)
            {
                throw new HttpRequestException("The EWGF API request timed out.", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Failed to parse EWGF API response.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve battle data from the EWGF API.", ex);
            }
        }
    }
}