using System;
using System.Collections.Generic;
using LogicLayer;

namespace DAL
{
    public class CharacterRepo : ICharacterRepo
    {           
        public List<Character> GetAllCharacters()
        {
            List<Character> CharacterList = new List <Character>();


            //CharacterList.Add()


            return CharacterList;
            
            throw new NotImplementedException();
        }

        public Character GetCharacterById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Move> GetCharacterMovesById(int id)
        {
            Character character = GetCharacterById(id);
            return character.Moves;
        }
    }
}

