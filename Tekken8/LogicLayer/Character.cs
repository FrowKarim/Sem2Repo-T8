using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer
{
    internal class Character
    {
        public int Id { get; set; }

        public string name { get; set; }

        public List <Move> moveList = new List <Move>();

    }
}

