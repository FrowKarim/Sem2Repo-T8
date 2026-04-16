using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer
{
    public interface IEWGFApi
    {
        public Task<Battle> GetBattleDataAsync(string battleId);

       
    }
}
