using GamePicker;
using GamePickerWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web.Helpers;

namespace GamePickerWeb.Pages
{
    public class GamePickerModel : PageModel
    {
        [BindProperty]
        public int SupportedPlayers { get; set; } = 1;

        [BindProperty]
        public bool EnforcePlayerCount { get; set; } = false;

        [BindProperty]
        public Platform SupportedPlatforms { get; set; } = 0;

        [BindProperty]
        public string RequriedTags { get; set; } = string.Empty;

        [BindProperty(Name ="Selected Game")]
        public string PickedGame { get; private set; } = "No Game Selected";

        [BindProperty]
        public string[] SelectedGames { get; set; }

        public SelectList SelectedGamesList { get; set; }

        private readonly GamePickerWebContext _context;

        public GamePickerModel(GamePickerWebContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            if (_context != null)
            {
                SelectedGamesList = new SelectList(_context.GameModel.Select(game => game.Title));
            }
        }

        public void OnPost()
        {
            if (SelectedGames.Length == 0)
            {
                PickedGame = RandomGamePicker.SelectRandomGameEqualWeights(
                    SupportedPlayers,
                    EnforcePlayerCount,
                    (int)SupportedPlatforms,
                    RequriedTags)?.Title 
                    ?? String.Empty;
            }
            else
            {
                PickedGame = RandomGamePicker.SelectRandomGameEqualWeights(SelectedGames)?.Title
                    ?? String.Empty;
            }

            if (_context != null)
            {
                SelectedGamesList = new SelectList(_context.GameModel.Select(game => game.Title));
            }
        }
    }
}
