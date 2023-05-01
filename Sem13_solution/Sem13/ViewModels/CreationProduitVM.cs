using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Sem13.ViewModels
{
    public class CreationProduitVM
    {
        [Required(ErrorMessage = "Spécifiez un nom pour le produit.")]
        [StringLength(100, ErrorMessage = "Le nom du produit est trop long.")]
        public string Nom { get; set; } = null!;

        [Required(ErrorMessage = "Spécifiez un prix pour le produit")]
        [Range(0, int.MaxValue, ErrorMessage = "Le prix doit être supérieur ou égal à 0")]
        public int PrixEntier { get; set; }

        [Required(ErrorMessage = "Spécifiez un prix décimal pour le produit")]
        [Range(0, 99, ErrorMessage = "La portion décimale du prix doit être entre 0 et 99.")]
        public int PrixDecimal { get; set; }

        [Required(ErrorMessage = "Spécifiez une quantité initale pour le stock")]
        [Range(0, int.MaxValue, ErrorMessage = "La quantité doit être supérieure ou égale à 0")]
        public int QteStock { get; set; }

        public bool EstDiscontinue { get; set; }

        [Required(ErrorMessage = "Sélectionnez une catégorie pour le produit.")]
        public string Categorie { get; set; } = null!;

        public List<SelectListItem> Categories { get; } = new List<SelectListItem>
        {
            new SelectListItem{ Value = "Divertissement", Text = "Divertissement"},
            new SelectListItem{ Value = "Électronique", Text = "Électronique"},
            new SelectListItem{ Value = "Meuble", Text = "Meuble"},
            new SelectListItem{ Value = "Vêtement", Text = "Vêtement"},
            new SelectListItem{ Value = "Bijou", Text = "Bijou"}
        };
    }
}
