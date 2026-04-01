using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tekken8.Pages
{
    public class CharactersModel : PageModel
    {               
        public List<string> ImagePaths { get; set; }

        public void OnGet()
        {
            var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            if (Directory.Exists(imagesPath))
            {
                ImagePaths = Directory.GetFiles(imagesPath)
                    .Where(f => IsImageFile(f))
                    .Select(f => "/images/" + Path.GetFileName(f))
                    .ToList();
            }
            else
            {
                ImagePaths = new List<string>();
            }
        }

        private bool IsImageFile(string filePath)
        {
            var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(filePath).ToLower();
            return imageExtensions.Contains(extension);
        }
    }
}
