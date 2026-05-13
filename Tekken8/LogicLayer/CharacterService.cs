using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer
{
    public class CharacterService
    {
        private readonly ICharacterRepo _characterRepo;

        public CharacterService(ICharacterRepo characterRepo)
        {
            _characterRepo = characterRepo;
        }

        public List<Character> GetAllCharacters()
        {
            return _characterRepo.GetAllCharacters();
        }

        public Character GetCharacterById(int characterId)
        {
            return _characterRepo.GetCharacterById(characterId);
        }

        public List<Move> GetCharacterMovesById(int characterId)
        {
            return _characterRepo.GetCharacterMovesById(characterId);
        }
    }
}
