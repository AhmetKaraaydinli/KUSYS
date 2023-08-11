using System.ComponentModel.DataAnnotations;

namespace KUSYS.Front.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email alanı girişi zorunludur.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı girişi zorunludur.")]
        public string Password { get; set; }

    }
}
