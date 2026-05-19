using DAL;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Tekken8.Pages
{
    public class DetailsModel : PageModel
    {
        private Character _singleCharacter;
        
        private readonly IConfiguration _configuration;

        public DetailsModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet(int characterID)
        {


            CharacterService characterService = new CharacterService(new CharacterRepo(_configuration));
            //CharacterService cs = new CharacterService(new CharacterRepo());
            //string CharacterId = Request.Query["characterID"].ToString();
            //Character getSingleCharacter = cs.GetCharacter(CharacterId);
            
            string CharacterId = Request.Query["characterID"].ToString();

            _singleCharacter = characterService.GetCharacterById(characterID);
        }

        public Character GetSingleCharacter()
        {
            return _singleCharacter;
        }
    }
}
