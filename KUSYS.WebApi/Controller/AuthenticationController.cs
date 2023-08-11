using KUSYS.WebApi.Core.Application.Interfaces;
using KUSYS.WebApi.Core.Application.Model;
using KUSYS.WebApi.Core.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KUSYS.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILoginService _loginService;
        public AuthenticationController(IStudentService studentService,ILoginService loginService)
        {
            _studentService= studentService;
            _loginService= loginService;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model ,CancellationToken cancellationToken)
        {
            var isAccount = await _studentService.GetEmailStudent(model.Email, cancellationToken);
            if (isAccount == null)
                return BadRequest("Bu Email ile kullanıcı bulumadı lütfen bir kullanıcı kaydedin");

            if (!await _loginService.CheckPassword(model.Password, model.Email, cancellationToken))
                return BadRequest("Yanlış parola girdiniz. Lütfen tekrar deneyiniz.");

            var token = JwtTokenGenerator.GenerateToken(isAccount.Email, isAccount.StudentId,isAccount.Role);
            return Ok(token);
        }
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromBody] LoginModel model, CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}
