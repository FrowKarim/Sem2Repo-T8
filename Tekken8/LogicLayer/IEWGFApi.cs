using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer
{
    public interface IEWGFApi
    {
        public Task<List<Battle>> GetBattleDataAsync(string battleId);

       
    }
}
