using KUSYS.WebApi.Core.Application.Dto;
using KUSYS.WebApi.Core.Application.Interfaces;
using KUSYS.WebApi.Core.Application.Model;
using KUSYS.WebApi.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace KUSYS.WebApi.Controller
{
    [Authorize(Roles ="admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService accountService)
        {
            _studentService = accountService;
        }
        [HttpPost("New")]
        public async Task<IActionResult> New([FromBody] StudentModel model, CancellationToken cancellationToken)
        {

            // burda role kontrolü yapılacak
            if (!ModelState.IsValid)
                return BadRequest("Lütfen gerekli alanları kontrol doldurunuz.");

            var isAccount = await _studentService.GetEmailStudent(model.Email, cancellationToken);

            if (isAccount != null)
                return BadRequest("Email başka kullanıcı tarafından kullanılmaktadır.");

            return Ok(await _studentService.CreateStudent(model, cancellationToken));

        }

        [HttpGet("Delete")]
        public async Task<IActionResult> Delete([FromQuery, Required] int studentId, CancellationToken cancellationToken)
        {
            // burda role kontrolü yapılacak
            if (!ModelState.IsValid)
                return BadRequest("Kullanıcı id boş bırakılamaz");

            return Ok(await _studentService.DeleteStudent(studentId, cancellationToken));
        }
        
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromQuery] int studentId, [FromBody] StudentModel model, CancellationToken cancellationToken)
        {
            // burda role kontrolü yapılacak
            if (!ModelState.IsValid)
                return BadRequest("Lütfen accountId ile kullanıcı bilgilerini doldurunuz.");

            return Ok(await _studentService.UpdateStudent(studentId, model, cancellationToken));
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get([FromQuery] int studentId, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest("Lütfen accountId ile kullanıcı bilgilerini doldurunuz.");

            return Ok(await _studentService.GetStudent(studentId, cancellationToken));
        }

        [AllowAnonymous]
        [HttpGet("List")]
        public async Task<IActionResult> List(CancellationToken cancellationToken)
        {
            return Ok(await _studentService.List(cancellationToken));
        }

        [AllowAnonymous]
        [HttpPost("AddCourse")]
        public async Task<IActionResult> AddCourse([FromBody] StudentCourseModel model, CancellationToken cancellationToken)
        {
            var isAdded = await _studentService.AddCourse(model.studentId, model.courseId, cancellationToken);  
            if (isAdded)
                return Ok(isAdded);
            else
                return BadRequest("is exits");
        }
    }

}
