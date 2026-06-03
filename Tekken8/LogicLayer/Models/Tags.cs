using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer.Models
{
    public class Tags
    {
        [JsonProperty("hb")]
        public string Hb { get; set; }

        [JsonProperty("pc")]
        public string Pc { get; set; }

        [JsonProperty("spk")]
        public string Spk { get; set; }

        [JsonProperty("fbr")]
        public string Fbr { get; set; }

        [JsonProperty("ps")]
        public string Ps { get; set; }

        [JsonProperty("hs")]
        public string Hs { get; set; }

        [JsonProperty("bbr")]
        public string Bbr { get; set; }

        [JsonProperty("ra")]
        public string Ra { get; set; }

        [JsonProperty("rbr")]
        public string Rbr { get; set; }

        [JsonProperty("wc")]
        public string Wc { get; set; }

        [JsonProperty("kne")]
        public string Kne { get; set; }

        [JsonProperty("hed")]
        public string Hed { get; set; }

        [JsonProperty("trn")]
        public string Trn { get; set; }

        [JsonProperty("js")]
        public string Js { get; set; }

        [JsonProperty("fs")]
        public string Fs { get; set; }

        [JsonProperty("hom")]
        public string Hom { get; set; }

        [JsonProperty("cs")]
        public string Cs { get; set; }

        [JsonProperty("he")]
        public string He { get; set; }
    }
}
