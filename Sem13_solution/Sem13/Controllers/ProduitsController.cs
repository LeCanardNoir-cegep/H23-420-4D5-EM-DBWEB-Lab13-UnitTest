using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sem13.Data;
using Sem13.Models;
using Sem13.ViewModels;

namespace Sem13.Controllers
{
    public class ProduitsController : Controller
    {
        private readonly Sem13Context _context;

        public ProduitsController(Sem13Context context)
        {
            _context = context;
        }

        /*public IActionResult ActionSimple()
        {
            List<string> mots = new List<string>() { "matelas", "jambon", "selfie stick" };

            return View(mots);
        }*/

        // GET: Produits
        public async Task<IActionResult> Index()
        {
            if(_context.Produits == null)
            {
                return Problem("L'ensemble Produits est null.");
            }

            return View(await _context.Produits.ToListAsync());
        }

        // GET: Produits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Produits == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits
                .FirstOrDefaultAsync(m => m.ProduitId == id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // GET: Produits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreationProduitVM cpvm)
        {
            if (ModelState.IsValid)
            {
                Produit produit = new Produit()
                {
                    ProduitId = 0,
                    Categorie = cpvm.Categorie,
                    Nom = cpvm.Nom,
                    QteStock = cpvm.QteStock,
                    Prix = cpvm.PrixEntier + cpvm.PrixDecimal / 100,
                    EstDiscontinue = cpvm.EstDiscontinue
                };
                _context.Produits.Add(produit);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Un ou plusieurs champs sont invalides.");

            return View(cpvm);
        }

        // GET: Produits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Produits == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }
            return View(produit);
        }

        // POST: Produits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProduitId,Categorie,Nom,Prix,QteStock,EstDiscontinue")] Produit produit)
        {
            if (id != produit.ProduitId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduitExists(produit.ProduitId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(produit);
        }

        // GET: Produits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Produits == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits
                .FirstOrDefaultAsync(m => m.ProduitId == id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // POST: Produits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Produits == null)
            {
                return Problem("Entity set 'Sem13Context.Produits'  is null.");
            }
            var produit = await _context.Produits.FindAsync(id);
            if (produit != null)
            {
                _context.Produits.Remove(produit);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProduitExists(int id)
        {
          return (_context.Produits?.Any(e => e.ProduitId == id)).GetValueOrDefault();
        }

        [Authorize]
        public async Task<IActionResult> IndexAvecAutorisation()
        {
            if (_context.Produits == null)
            {
                return Problem("L'ensemble Produits est null.");
            }

            IIdentity? identite = HttpContext.User.Identity;
            if(identite != null && identite.IsAuthenticated)
            {
                string pseudo = HttpContext.User.FindFirstValue(ClaimTypes.Name);
                Utilisateur? utilisateur = await _context.Utilisateurs.FirstOrDefaultAsync(x => x.Pseudonyme == pseudo);
                if (utilisateur != null)
                {
                    ViewData["utilisateur"] = utilisateur.Pseudonyme;
                    return View(await _context.Produits.ToListAsync());
                }
            }

            return Unauthorized();
        }
    }
}
