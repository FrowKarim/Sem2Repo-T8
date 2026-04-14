using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer
{
    public class Battle
    {
        [JsonProperty("battle_at")]
        public DateTime BattleAt { get; set; }

        [JsonProperty("battle_type")]
        public string BattleType { get; set; }

        [JsonProperty("game_version")]
        public int GameVersion { get; set; }

        [JsonProperty("winner")]
        public int Winner { get; set; }

        [JsonProperty("stage_id")]
        public int StageId { get; set; }

        [JsonProperty("p1_name")]
        public string P1Name { get; set; }

        [JsonProperty("p1_tekken_id")]
        public string P1TekkenId { get; set; }

        [JsonProperty("p1_char")]
        public string P1Char { get; set; }

        [JsonProperty("p1_region")]
        public string P1Region { get; set; }

        [JsonProperty("p1_tekken_power")]
        public int P1TekkenPower { get; set; }

        [JsonProperty("p1_dan_rank")]
        public string P1DanRank { get; set; }

        [JsonProperty("p1_rounds_won")]
        public int P1RoundsWon { get; set; }

        [JsonProperty("p2_name")]
        public string P2Name { get; set; }

        [JsonProperty("p2_tekken_id")]
        public string P2TekkenId { get; set; }

        [JsonProperty("p2_char")]
        public string P2Char { get; set; }

        [JsonProperty("p2_region")]
        public string P2Region { get; set; }

        [JsonProperty("p2_dan_rank")]
        public string P2DanRank { get; set; }

        [JsonProperty("p2_tekken_power")]
        public int P2TekkenPower { get; set; }

        [JsonProperty("p2_rounds_won")]
        public int P2RoundsWon { get; set; }
    }
}
