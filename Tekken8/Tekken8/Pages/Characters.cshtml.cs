using Microsoft.AspNetCore.Mvc.RazorPages;
using LogicLayer;

namespace Tekken8.Pages
{
    public class CharactersModel : PageModel
    {
        public List<Character> Characters { get; set; } = new();

        public void OnGet()
        {
            // Load characters - replace with actual data retrieval from database or service
            Characters = GetAllCharacters();
        }

        public List<Character> GetAllCharacters()
        {
            // This is placeholder data. Replace with actual retrieval from a service/database
            return new List<Character>
            {
                new Character
                {
                    Id = 1,
                    Name = "Kazuya"
                    
                },
                new Character
                {
                    Id = 2,
                    Name = "Jin"
                },
                new Character
                {
                    Id = 3,
                    Name = "Paul Phoenix"
                },
                new Character
                {
                    Id = 4,
                    Name = "Nina Williams"
                },
                new Character
                {
                    Id = 5,
                    Name = "King"
                },
                new Character
                {
                    Id = 6,
                    Name = "Yoshimitsu"
                }
            };
        }
    }
}
