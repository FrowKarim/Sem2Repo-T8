using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer
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

        //  "moveNumber": 34,
        //"command": "df+3,2,1",
        //"name": "Impaling Knee Twin Thrust",
        //"hitLevel": "m, m, m",
        //"damage": "13, 15, 21",
        //"startup": "i18, ",
        //"block": "-14",
        //"hit": "+8(-1)",
        //"counterHit": "+25",
        //"notes": "* Tornado\n* Balcony Break\n* Combo from 2nd CH\n* Can be delayed 16F",
        //"wavuId": "Kazuya-df+3,2,1",
        //"tags": {
        //  "trn": "",
        //  "bbr": ""
        //},
        //"image": "",
        //"video": "File:t8-p2-kazuya-df+3,2,1.mp4",
        //"recovery": ""
    }
}
