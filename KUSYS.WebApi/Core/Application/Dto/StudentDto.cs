using KUSYS.WebApi.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace KUSYS.WebApi.Core.Application.Dto
{
    public class StudentDto
    {
        public int StudentId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public Role Role { get; set; } = Role.student;

    }
}
