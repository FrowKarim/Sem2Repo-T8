using LogicLayer;
using LogicLayer.Models;
using LogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tekken8.Pages
{
    public class MoveDetailsModel : PageModel
    {
        private readonly CharacterService _characterService;
        private readonly CommentService _commentService;

        public MoveDetailsModel(CharacterService characterService, CommentService commentService)
        {
            _characterService = characterService;
            _commentService = commentService;
        }

        public Character? Character { get; set; }
        public Move? Move { get; set; }
        public List<Comment> Comments { get; set; } = new();

        [BindProperty]
        public string NewCommentText { get; set; } = string.Empty;

        public bool IsLoggedIn
        {
            get
            {
                return HttpContext.Session.GetInt32("UserId").HasValue;
            }
        }

        public IActionResult OnGet(int characterId, int moveId)
        {
            Character = _characterService.GetCharacterById(characterId);

            if (Character == null || Character.Id == 0)
            {
                return NotFound();
            }

            Move = Character.Moves?.FirstOrDefault(m => m.Id == moveId);

            if (Move == null)
            {
                return NotFound();
            }

            Comments = _commentService.GetCommentsByMoveId(moveId);
            return Page();
        }

        public IActionResult OnPost(int characterId, int moveId)
        {
            Character = _characterService.GetCharacterById(characterId);

            if (Character == null || Character.Id == 0)
            {
                return NotFound();
            }

            Move = Character.Moves?.FirstOrDefault(m => m.Id == moveId);

            if (Move == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToPage("/Login");
            }

            if (string.IsNullOrWhiteSpace(NewCommentText))
            {
                ModelState.AddModelError(string.Empty, "Comment cannot be empty.");
                Comments = _commentService.GetCommentsByMoveId(moveId);
                return Page();
            }

            var comment = new Comment
            {
                MoveId = moveId,
                UserId = userId.Value,
                CommentText = NewCommentText.Trim()
            };

            _commentService.AddComment(comment);

            return RedirectToPage("/MoveDetails", new { characterId = characterId, moveId = moveId });
        }
    }
}