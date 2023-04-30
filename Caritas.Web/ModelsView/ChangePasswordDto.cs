using System.ComponentModel.DataAnnotations;

namespace Caritas.Web.ModelsView
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(10, ErrorMessage = "El {0} debe tener al menos {2} y un máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string PasswordActual { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(10, ErrorMessage = "El {0} debe tener al menos {2} y un máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "La contraseña y la confirmación son distintas.")]
        public string PasswordConfirm { get; set; }
    }
}
