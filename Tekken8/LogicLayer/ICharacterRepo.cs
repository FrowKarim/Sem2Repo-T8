using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace LogicLayer
{
    public interface ICharacterRepo
    {
        public List<Character> GetAllCharacters();

        public Character GetCharacterById(int id);

        public List<Move> GetCharacterMovesById(int id);

    }
}
