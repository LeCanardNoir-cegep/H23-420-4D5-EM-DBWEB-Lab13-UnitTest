using Microsoft.AspNetCore.Mvc.Rendering;

namespace Labo13.ViewModels
{
    public class CreerScoreTrouVM
    {
        public int Score { get; set; }
        public string NomGolfeur { get; set; } = null!;

        public List<SelectListItem> NomsGolfeurs { get; } = new List<SelectListItem>
        {
            new SelectListItem{Value = "Tiger Woods", Text = "Tiger Woods"},
            new SelectListItem{Value = "Patrick (Mii)", Text = "Patrick (Mii)"},
            new SelectListItem{Value = "Arnold Palmer", Text = "Arnold Palmer"},
            new SelectListItem{Value = "Jack Nicklaus", Text = "Jack Nicklaus"},
            new SelectListItem{Value = "Annika Sörenstam", Text = "Annika Sörenstam"},
        };
    }
}
