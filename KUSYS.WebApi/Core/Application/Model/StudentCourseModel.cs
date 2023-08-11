using Microsoft.AspNetCore.Authorization;

namespace KUSYS.WebApi.Core.Application.Model
{
    public class StudentCourseModel
    {
        public int studentId { get; set; }
        public string courseId { get; set; }
    }
}
