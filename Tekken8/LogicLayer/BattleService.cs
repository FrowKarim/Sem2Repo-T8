namespace LogicLayer
{
    public class BattleService
    {
        private readonly IEWGFApi _ewgfApi;

        public BattleService(IEWGFApi ewgfApi)
        {
            _ewgfApi = ewgfApi;
        }

        public async Task<string> GetBattleDataAsync(string battleId)
        {
            return await _ewgfApi.GetBattleDataAsync(battleId);
        }
    }
}

