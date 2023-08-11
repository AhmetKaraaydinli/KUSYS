using System.ComponentModel.DataAnnotations;

namespace KUSYS.Front.Models
{
    public class StudentModel
    {
        [Required(ErrorMessage = "Email alanı girişi zorunludur.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "FirstName alanı girişi zorunludur.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName alanı girişi zorunludur.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Password alanı girişi zorunludur.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "BirthDate alanı girişi zorunludur.")]
        public DateTime BirthDate { get; set; }
    }
}
