namespace LogicLayer.Models
{
    public class PlayerBattleSummary
    {
        public string TekkenId { get; set; }
        public string PlayerName { get; set; }
        public string Region { get; set; }

        public int TotalBattles { get; set; }
        public double OverallWinRate { get; set; }
        public double AverageTekkenPower { get; set; }
        public double AverageRoundsWon { get; set; }

        public string MostPlayedCharacter { get; set; }
        public int MostPlayedCharacterGames { get; set; }

        public string BestCharacter { get; set; }
        public double BestCharacterWinRate { get; set; }

        public string WorstMatchupCharacter { get; set; }
        public double WorstMatchupWinRate { get; set; }

        public string FavoriteBattleType { get; set; }
        public string MostCommonRank { get; set; }
    }
}