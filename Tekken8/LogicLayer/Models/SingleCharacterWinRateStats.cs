using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer.Models
{
    public class SingleCharacterWinRateStats
    {
        public string CharacterName { get; set; }
        public int TotalGames { get; set; }
        public int Wins { get; set; }
        public double WinRate { get; set; }
        public bool HasSufficientData { get; set; }
    }
}