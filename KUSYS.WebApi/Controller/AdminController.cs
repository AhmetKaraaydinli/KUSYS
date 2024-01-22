using KUSYS.WebApi.Core.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KUSYS.WebApi.Controller
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public AdminController(IStudentService studentService)
        {
            _studentService= studentService;
        }
        [HttpGet("CourseStudentss")]
        public async Task<IActionResult> CourseStudents(CancellationToken cancellationToken)
        {
            return Ok(await _studentService.GetStudentsCourses(cancellationToken));
        }
    }
}
