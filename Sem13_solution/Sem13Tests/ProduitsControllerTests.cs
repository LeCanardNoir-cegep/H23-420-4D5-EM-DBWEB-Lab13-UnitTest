using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Sem13.Controllers;
using Sem13.Data;
using Sem13.Models;
using Sem13.ViewModels;

namespace Sem13Test
{
    public class ProduitsControllerTests
    {
        /*[Fact]
        public void ActionSimple_RetourneListeTroisMots_AvecViewResult()
        {
            // Instancier le contrôleur
            ProduitsController controller = new ProduitsController();

            // Appeler l'action à tester et stocker son output
            IActionResult resultat = controller.ActionSimple();

            // Attraper le "ViewResult" et vérifier que c'est bel et bien un ViewResult
            // (ce qui générerait la vue Razor)
            ViewResult viewResult = Assert.IsType<ViewResult>(resultat);

            // Attraper la liste de string dans le ViewResult (et vérifier que c'est une liste de string)
            List<string> model = Assert.IsAssignableFrom<List<string>>(viewResult.Model);

            // Vérifier qu'il y en a trois dans la liste (par exemple)
            Assert.Equal(3, model.Count);
        }*/

        [Fact]
        public async Task Index_SetProduitsNull_Problem()
        {
            Mock<Sem13Context> mockContext = new Mock<Sem13Context>();

            ProduitsController controller = new ProduitsController(mockContext.Object);
            IActionResult resultat = await controller.Index();

            ObjectResult objectResult = Assert.IsType<ObjectResult>(resultat);

            Assert.IsAssignableFrom<ProblemDetails>(objectResult.Value);
        }


        [Fact]
        public async Task Index_ListeProduitsValides()
        {
            Mock<Sem13Context> mockContext = new Mock<Sem13Context>();
            List<Produit> produits = new List<Produit>() {
                new Produit() {
                    ProduitId = 1, Categorie = "Meuble", EstDiscontinue = false,
                    Nom = "Table brune", QteStock = 3, Prix = 399.99M
                },
                new Produit() {
                    ProduitId = 2, Categorie = "Meuble", EstDiscontinue = false,
                    Nom = "Chaise brune", QteStock = 29, Prix = 49.99M
                }
            };
            mockContext.Setup(x => x.Produits).ReturnsDbSet(produits);

            ProduitsController controller = new ProduitsController(mockContext.Object);

            IActionResult resultat = await controller.Index();

            ViewResult viewResult = Assert.IsType<ViewResult>(resultat);

            List<Produit> model = Assert.IsAssignableFrom<List<Produit>>(viewResult.Model);

            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task CreatePost_InsertionProduitValide()
        {
            List<Produit> produits = new List<Produit>() {
                new Produit() {
                    ProduitId = 1, Categorie = "Meuble", EstDiscontinue = false,
                    Nom = "Table brune", QteStock = 3, Prix = 399.99M
                },
            };

            Mock<Sem13Context> mockContext = new Mock<Sem13Context>();
            Mock<DbSet<Produit>> mockDbSet = new Mock<DbSet<Produit>>();

            mockDbSet.Setup(x => x.Add(It.IsAny<Produit>())).Callback<Produit>(x => produits.Add(x));
            mockContext.Setup(x => x.Produits).Returns(mockDbSet.Object);

            CreationProduitVM cpvm = new CreationProduitVM()
            {
                Nom = "Divan 2 places", PrixEntier = 499, PrixDecimal = 99,
                QteStock = 3, EstDiscontinue = false, Categorie = "Meuble"
            };

            ProduitsController controller = new ProduitsController(mockContext.Object);
            IActionResult resultat = await controller.Create(cpvm);

            Assert.Equal(2, produits.Count);
            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(resultat);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task CreatePost_InsertionProduitInvalide()
        {
            Mock<Sem13Context> mockContext = new Mock<Sem13Context>();

            CreationProduitVM cpvm = new CreationProduitVM()
            {
                Nom = "Divan 2 places", PrixEntier = 499, PrixDecimal = 99,
                QteStock = -7, EstDiscontinue = false, Categorie = "Meuble"
            };

            ProduitsController controller = new ProduitsController(mockContext.Object);
            controller.ModelState.AddModelError("QteStock", "Range");

            IActionResult resultat = await controller.Create(cpvm);
            ViewResult viewResult = Assert.IsType<ViewResult>(resultat);
            Assert.IsType<CreationProduitVM>(viewResult.Model);

            ModelStateDictionary modelState = viewResult.ViewData.ModelState;

            Assert.True(modelState.ContainsKey(""));
            Assert.Equal("Un ou plusieurs champs sont invalides.", modelState[""].Errors[0].ErrorMessage);
        }
    }
}