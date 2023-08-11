using KUSYS.Front.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace KUSYS.Front.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IToastNotification  _toast;
        public AuthenticationController(IHttpClientFactory httpClientFactory, IToastNotification toastNotification)
        {
            _httpClientFactory = httpClientFactory;
            _toast = toastNotification;
        }
        public IActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public async Task< IActionResult> Login(LoginModel model,CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var client = this._httpClientFactory.CreateClient();
                var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"); 
                var response = await client.PostAsync("http://localhost:5109/api/Authentication/Login", content);
                ViewBag.Message = "";



                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync(cancellationToken);
                    var reponseToken = JsonSerializer.Deserialize<TokenResponse>(data,new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    if(reponseToken != null)
                    {
                        JwtSecurityTokenHandler handler = new ();
                        var token = handler.ReadJwtToken(reponseToken.Token);

                        var claims = token.Claims.ToList();
                        if (reponseToken.Token != null)
                            claims.Add(new Claim("accesToken", reponseToken.Token));
                        var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                        var props = new AuthenticationProperties
                        {
                            ExpiresUtc = reponseToken.ExpireDate,
                            IsPersistent = true,
                        };

                        await HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), props);
                        _toast.AddSuccessToastMessage("Login Succes");
                        return RedirectToAction("Index","Home");
                    }
                }
                else
                {
                    _toast.AddErrorToastMessage("Kullanıcı Adı veya şifre hatalı ");
                    return RedirectToAction("Login", "Authentication");
                }

                return View();
            }
            return View(model);
        }
    }
}
