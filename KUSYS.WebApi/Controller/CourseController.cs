using KUSYS.WebApi.Core.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KUSYS.WebApi.Controller
{
    [Authorize(Roles = "admin,student")]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseServices _courseServices;
        public CourseController(ICourseServices courseServices)
        {
            _courseServices= courseServices;
        }
        [HttpGet("List")]
        public async Task<IActionResult> Courses(CancellationToken cancellationToken)
        {
            return Ok(await _courseServices.GetCourses(cancellationToken));
        }
        [HttpGet("StudentCourses")]
        public async Task<IActionResult> StudentCourses([FromQuery] int studentId,CancellationToken cancellationToken)
        {
            return Ok(await _courseServices.GetStudentCourses(studentId,cancellationToken));
        }

    }
}
