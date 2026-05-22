namespace LogicLayer
{
    public class BattleService
    {
        private readonly IEWGFApi _ewgfApi;

        public BattleService(IEWGFApi ewgfApi)
        {
            _ewgfApi = ewgfApi;
        }

        public async Task<List<Battle>> GetBattleDataAsync(string battleId)
        {
            return await _ewgfApi.GetBattleDataAsync(battleId);
        }

        public void CompareBattleData(BattleList Player1, BattleList Player2)
        {

        }

        public double CalculateWinRate(List<Battle> battles, string tekkenId)
        {
            if (string.IsNullOrWhiteSpace(tekkenId))
                throw new ArgumentException("Tekken ID is required.", nameof(tekkenId));

            if (battles == null)
                throw new ArgumentNullException(nameof(battles));

            // dit is voor functional requirements, er moeten minimaal 10 battles zijn om een betrouwbare winrate te kunnen berekenen
            if (battles.Count < 10)
                throw new InvalidOperationException("Not enough battles (minimum 10 required).");


            // Zorgt ervoor dat alleen battles worden geteld die tekkenId als P1 of P2 hebben, en telt alleen de wins voor dat tekkenId
            // Dit moet omdat in de dataset tekkenId zowel in P1TekkenId als P2TekkenId kan voorkomen, afhankelijk van wie de speler is in die battle
            int wins = battles.Count(b =>
                (string.Equals(b.P1TekkenId, tekkenId, StringComparison.OrdinalIgnoreCase) && b.Winner == 1) ||
                (string.Equals(b.P2TekkenId, tekkenId, StringComparison.OrdinalIgnoreCase) && b.Winner == 2));

            return (double)wins / battles.Count * 100;
        }
    }
}

