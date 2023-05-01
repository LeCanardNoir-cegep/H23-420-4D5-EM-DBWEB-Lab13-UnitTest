using Labo13.Data;
using Labo13.Models;
using Labo13.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Labo13.Controllers
{
    public class GolfController : Controller
    {
        private readonly Labo13Context _context;

        public GolfController(Labo13Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<VwDetailsScoreGolfeur> scores = await _context.VwDetailsScoreGolfeurs.ToListAsync();
            return View(scores);
        }

        public async Task<IActionResult> ScoresTrou()
        {
            List<ScoreTrou> scoresTrou = await _context.ScoreTrous.ToListAsync();
            return View(scoresTrou);
        }

        public IActionResult CreateScoreTrou()
        {
            CreerScoreTrouVM cstvm = new CreerScoreTrouVM() { Score = 0 };
            return View(cstvm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateScoreTrou(CreerScoreTrouVM cstvm)
        {
            if (ModelState.IsValid)
            {
                Golfeur? golfeur = await _context.Golfeurs.FirstOrDefaultAsync(x => x.Nom == cstvm.NomGolfeur);
                if(golfeur == null)
                {
                    ModelState.AddModelError("NomGolfeur", "Ce golfeur n'existe pas.");
                    return View(cstvm);
                }
                string query = "EXEC Golf.USP_InsertScoreTrou @GolfeurID, @Score";
                List<SqlParameter> parameters = new List<SqlParameter>() {
                    new SqlParameter{ ParameterName = "@GolfeurID", Value = golfeur.GolfeurId},
                    new SqlParameter{ ParameterName = "@Score", Value = cstvm.Score }
                };
                await _context.Database.ExecuteSqlRawAsync(query, parameters.ToArray());
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Certains champs sont invalides.");
            return View(cstvm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteScoreTrou(int id)
        {
            ScoreTrou? scoreTrou = await _context.ScoreTrous.FirstOrDefaultAsync(x => x.ScoreTrouId == id);
            if(scoreTrou == null)
            {
                return RedirectToAction("ScoresTrou");
            }
            _context.ScoreTrous.Remove(scoreTrou);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
