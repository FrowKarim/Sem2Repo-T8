using LogicLayer.Interfaces;
using LogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer.Services
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
        public Move? GetMoveById(int moveId)
        {
            if (moveId <= 0)
            {
                throw new ArgumentException("Move id must be greater than 0.", nameof(moveId));
            }

            return _characterRepo.GetMoveById(moveId);
        }

        public Move? GetMoveByCharacterAndMoveId(int characterId, int moveId)
        {
            if (characterId <= 0)
            {
                throw new ArgumentException("Character id must be greater than 0.", nameof(characterId));
            }

            if (moveId <= 0)
            {
                throw new ArgumentException("Move id must be greater than 0.", nameof(moveId));
            }

            var moves = _characterRepo.GetCharacterMovesById(characterId);
            return moves.FirstOrDefault(m => m.Id == moveId);
        }

    }
}
