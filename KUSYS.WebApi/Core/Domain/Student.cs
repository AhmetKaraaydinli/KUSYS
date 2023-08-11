using System.ComponentModel.DataAnnotations;

namespace KUSYS.WebApi.Core.Domain
{
    public class Student
    {
        public Student()
        {
            Courses = new List<Course>();
        }
        [Key]
        public int StudentId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public Role Role { get; set; } = Role.student;
        public ICollection<Course> Courses { get; set; }
    }

    public enum Role
    {
        admin = 0,
        student = 1
    }
}
