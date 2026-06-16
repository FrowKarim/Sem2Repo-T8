using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LogicLayer.Models
{
    public class BattleList
    {
       
        [JsonProperty("data")]
        public List<Battle> Battles { get; set; }
    }
}
