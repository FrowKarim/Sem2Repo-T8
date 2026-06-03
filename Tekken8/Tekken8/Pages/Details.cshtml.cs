using DAL;
using LogicLayer.Models;
using LogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Tekken8.Pages
{
    public class DetailsModel : PageModel
    {
        public Character SingleCharacter { get; set; }
        
        private readonly IConfiguration _configuration;

        public DetailsModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet(int characterID)
        {
            CharacterService characterService = new CharacterService(new CharacterRepo(_configuration));
            SingleCharacter = characterService.GetCharacterById(characterID);
        }
    }
}
