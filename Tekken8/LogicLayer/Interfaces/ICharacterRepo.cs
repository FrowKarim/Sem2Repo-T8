using LogicLayer.Models;
using System.Collections.Generic;

namespace LogicLayer.Interfaces
{
    public interface ICharacterRepo
    {
        List<Character> GetAllCharacters();
        Character GetCharacterById(int id);
        List<Move> GetCharacterMovesById(int id);
        Move? GetMoveById(int moveId);
    }
}