using System.ComponentModel.DataAnnotations;

namespace Sem13.ViewModels
{
    public class InscriptionViewModel
    {
        [Required(ErrorMessage = "Un nom d'utilisateur est requis.")]
        public string Pseudonyme { get; set; } = null!;

        [Required(ErrorMessage = "Un mot de passe est requis.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Le mot de passe doit avoir entre 6 et 50 caractères.")]
        [DataType(DataType.Password)]
        public string MotDePasse { get; set; } = null!;

        [Required(ErrorMessage = "Veuillez confirmer le mot de passe.")]
        [DataType(DataType.Password)]
        [Compare(nameof(MotDePasse), ErrorMessage = "Les deux mots de passe sont différents.")]
        public string MotDePasseConfirmation { get; set; } = null!;

        [Required(ErrorMessage = "Une adresse courriel est requise.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
    }
}
