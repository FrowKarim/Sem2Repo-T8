namespace LogicLayer.Models
{
    public class BattleComparisonResult
    {
        public PlayerBattleSummary Player1 { get; set; }
        public PlayerBattleSummary Player2 { get; set; }

        public string BetterWinRatePlayer { get; set; }
        public string BetterTekkenPowerPlayer { get; set; }
        public string BetterRoundsWonPlayer { get; set; }
        public string MoreConsistentCharacterPlayer { get; set; }
    }
}