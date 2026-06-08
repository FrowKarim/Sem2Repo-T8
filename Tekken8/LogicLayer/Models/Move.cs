using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer.Models
{
    public class Move
    {
        [JsonProperty("moveNumber")]
        public int MoveNumber { get; set; }

        [JsonProperty("command")]
        public string Command { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("hitLevel")]
        public string HitLevel { get; set; }

        [JsonProperty("damage")]
        public string Damage { get; set; }

        [JsonProperty("startup")]
        public string Startup { get; set; }

        [JsonProperty("block")]
        public string Block { get; set; }

        [JsonProperty("hit")]
        public string Hit { get; set; }

        [JsonProperty("counterHit")]
        public string CounterHit { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("wavuId")]
        public string WavuId { get; set; }

        [JsonProperty("tags")]
        public Tags Tags { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("video")]
        public string Video { get; set; }

        [JsonProperty("recovery")]
        public string Recovery { get; set; }

        [JsonProperty("transitions")]
        public List<string> Transitions { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        
    }
}
