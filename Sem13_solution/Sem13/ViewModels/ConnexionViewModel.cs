using System.ComponentModel.DataAnnotations;

namespace Sem13.ViewModels
{
    public class ConnexionViewModel
    {
        [Required(ErrorMessage = "Veuillez préciser un nom d'utilisateur.")]
        public string Pseudonyme { get; set; } = null!;

        [Required(ErrorMessage = "Veuillez entrer un mot de passe.")]
        public string MotDePasse { get; set; } = null!;
    }
}
