using LogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace LogicLayer.Interfaces
{
    public interface ICharacterRepo
    {
        public List<Character> GetAllCharacters();

        public Character GetCharacterById(int id);

        public List<Move> GetCharacterMovesById(int id);

    }
}
