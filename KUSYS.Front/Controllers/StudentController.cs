using KUSYS.Front.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace KUSYS.Front.Controllers
{
    public class StudentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IToastNotification _toast;
        public StudentController(IHttpClientFactory httpClientFactory,IToastNotification toastNotification)
        {
            _httpClientFactory = httpClientFactory;
            _toast = toastNotification;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> List(CancellationToken cancellationToken)
        {
            var token = User.Claims.FirstOrDefault(it => it.Type == "accesToken").Value;
            var expireDate = User.Claims.FirstOrDefault(it => it.Type == "exp").Value;
            var date = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expireDate)).DateTime; ;

            if (date < DateTime.UtcNow)
                return RedirectToAction("Login", "Authentication");
            if (token != null)
            {
                var client = this._httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync("http://localhost:5109/api/Student/List");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync(cancellationToken);
                    var courses = JsonSerializer.Deserialize<List<StudentReponse>>(data, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    return View(courses);
                }

            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int studentId, CancellationToken cancellationToken)
        {
            var token = User.Claims.FirstOrDefault(it => it.Type == "accesToken").Value;
            var expireDate = User.Claims.FirstOrDefault(it => it.Type == "exp").Value;
            var date = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expireDate)).DateTime; ;

            if (date < DateTime.UtcNow)
                return RedirectToAction("Authentication", "Login");
            if (token != null)
            {
                var client = this._httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(string.Format("http://localhost:5109/api/Student/Delete?studentId={0}", studentId));
                if (response.IsSuccessStatusCode)
                {
                    await response.Content.ReadAsStringAsync(cancellationToken);
                    _toast.AddSuccessToastMessage("Student succces delete ");

                }
                else
                {
                    _toast.AddErrorToastMessage("Student succces error ");
                }
                
            }
            return RedirectToAction("List", "Student");
        }

        public IActionResult New()
        {
            return View(new StudentModel());
        }
        [HttpPost]
        public async Task<IActionResult> New(StudentModel student, CancellationToken cancellationToken)
        {
            var token = User.Claims.FirstOrDefault(it => it.Type == "accesToken").Value;
            var expireDate = User.Claims.FirstOrDefault(it => it.Type == "exp").Value;
            var date = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expireDate)).DateTime; ;

            if (date < DateTime.UtcNow)
                return RedirectToAction("Authentication", "Login");
            if (token != null)
            {
                var client = this._httpClientFactory.CreateClient();
                var content = new StringContent(JsonSerializer.Serialize(student), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.PostAsync("http://localhost:5109/api/Student/New", content);
                if (response.IsSuccessStatusCode)
                {
                    await response.Content.ReadAsStringAsync(cancellationToken);
                    _toast.AddSuccessToastMessage("New Student Succes");
                }
                    
                else
                    _toast.AddErrorToastMessage("we encountered an error");

            }
            return RedirectToAction("List", "Student");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int studentId, CancellationToken cancellationToken)
        {
            var token = User.Claims.FirstOrDefault(it => it.Type == "accesToken").Value;
            var expireDate = User.Claims.FirstOrDefault(it => it.Type == "exp").Value;
            var date = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expireDate)).DateTime; ;

            if (date < DateTime.UtcNow)
                return RedirectToAction("Authentication", "Login");
            if (token != null)
            {
                var client = this._httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(string.Format("http://localhost:5109/api/Student/Get?studentId={0}", studentId));
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync(cancellationToken);
                    var student = JsonSerializer.Deserialize<StudentReponse>(data, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    return View(student);
                }
            }
            return RedirectToAction("List", "Student");
        }

        [HttpPost]
        public async Task<IActionResult> Update(int studentId, StudentModel model, CancellationToken cancellationToken)
        {
            var token = User.Claims.FirstOrDefault(it => it.Type == "accesToken").Value;
            var expireDate = User.Claims.FirstOrDefault(it => it.Type == "exp").Value;
            var date = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expireDate)).DateTime; ;

            if (date < DateTime.UtcNow)
                return RedirectToAction("Authentication", "Login");
            if (token != null)
            {
                var client = this._httpClientFactory.CreateClient();
                var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.PostAsync(string.Format("http://localhost:5109/api/Student/Update?studentId={0}", studentId), content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List", "Student");
                }
                else
                {
                    _toast.AddErrorToastMessage("Not Found Student");
                }

            }
            return RedirectToAction("List", "Student");
        }

    }
}
