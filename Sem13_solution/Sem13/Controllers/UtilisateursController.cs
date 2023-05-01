using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Sem13.Data;
using Sem13.Models;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using Sem13.ViewModels;

namespace Sem13.Controllers
{
    public class UtilisateursController : Controller
    {
        readonly Sem13Context _context;
        public UtilisateursController(Sem13Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["utilisateur"] = "Visiteur";
            IIdentity? identite = HttpContext.User.Identity;
            if (identite != null && identite.IsAuthenticated)
            {
                string pseudo = HttpContext.User.FindFirstValue(ClaimTypes.Name);
                Utilisateur? utilisateur = await _context.Utilisateurs.FirstOrDefaultAsync(x => x.Pseudonyme == pseudo);
                if (utilisateur != null)
                {
                    ViewData["utilisateur"] = utilisateur.Pseudonyme;
                }
            }
            return View();
        }

        public IActionResult Inscription()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Inscription(InscriptionViewModel ivm)
        {
            bool existeDeja = await _context.Utilisateurs.AnyAsync(x => x.Pseudonyme == ivm.Pseudonyme);
            if (existeDeja)
            {
                ModelState.AddModelError("Pseudonyme", "Ce pseudonyme est déjà pris.");
                return View(ivm);
            }
            string query = "EXEC Utilisateurs.USP_CreerUtilisateur @Pseudonyme, @MotDePasse, @NAS, @Email";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter{ParameterName = "@Pseudonyme", Value = ivm.Pseudonyme},
                new SqlParameter{ParameterName = "@MotDePasse", Value = ivm.MotDePasse},
                new SqlParameter{ParameterName = "@Email", Value = ivm.Email}
            };
            try
            {
                await _context.Database.ExecuteSqlRawAsync(query, parameters.ToArray());
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Une erreur est survenue. Veuillez réessayez.");
                return View(ivm);
            }
            return RedirectToAction("Connexion", "Utilisateurs");
        }

        public IActionResult Connexion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Connexion(ConnexionViewModel cvm)
        {
            string query = "EXEC Utilisateurs.USP_AuthUtilisateur @Pseudonyme, @MotDePasse";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter{ParameterName = "@Pseudonyme", Value = cvm.Pseudonyme},
                new SqlParameter{ParameterName = "@MotDePasse", Value = cvm.MotDePasse}
            };
            Utilisateur? utilisateur = (await _context.Utilisateurs.FromSqlRaw(query, parameters.ToArray()).ToListAsync()).FirstOrDefault();
            if (utilisateur == null)
            {
                // Premier paramètre vide car erreur pas associée à une propriété spécifique
                ModelState.AddModelError("", "Nom d'utilisateur ou mot de passe invalide");
                return View(cvm);
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, utilisateur.UtilisateurId.ToString()),
                new Claim(ClaimTypes.Name, utilisateur.Pseudonyme)
            };

            ClaimsIdentity identite = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identite);
            await HttpContext.SignInAsync(principal);

            return RedirectToAction("Index", "Utilisateurs");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Deconnexion()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Utilisateurs");
        }
    }
}
