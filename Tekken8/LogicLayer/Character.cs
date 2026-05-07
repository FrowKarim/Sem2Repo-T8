using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer
{
    public class Character
    {
        public int Id { get; set; }

        [JsonProperty("characterName")]
        public string Name { get; set; }

        [JsonProperty("editUrl")]
        public string EditUrl { get; set; }

        [JsonProperty("game")]
        public string Game { get; set; }

        [JsonProperty("framesNormal")]
        public List<Move> Moves { get; set; }

        [JsonProperty("stances")]
        public List<string> Stances { get; set; }

    }
}

