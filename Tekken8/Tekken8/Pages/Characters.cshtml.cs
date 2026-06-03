using DAL;
using LogicLayer;
using LogicLayer.Models;
using LogicLayer.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Tekken8.Pages
{
    public class CharactersModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public CharactersModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Character> Characters { get; set; } = new();

        public void OnGet()
        {
            var characterService = new CharacterService(new CharacterRepo(_configuration));
            Characters = characterService.GetAllCharacters();
        }

    }
}
