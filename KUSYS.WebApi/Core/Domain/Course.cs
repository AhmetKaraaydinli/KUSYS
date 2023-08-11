using System.ComponentModel.DataAnnotations;

namespace KUSYS.WebApi.Core.Domain
{
    public class Course
    {
        [Key]
        public string CourseId { get; set; }
        public string CourseName{ get; set; }
        public ICollection<Student> Students { get; set; }
        public Course()
        {
            Students = new List<Student>();
        }
    }
}
