using LogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer.Interfaces
{
    public interface IEWGFApi
    {
        public Task<List<Battle>> GetBattleDataAsync(string battleId);

       
    }
}
