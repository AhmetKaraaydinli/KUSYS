using KUSYS.Front.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace KUSYS.Front.Controllers
{
    public class CourseController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IToastNotification _toast;
        public CourseController(IHttpClientFactory httpClientFactory,IToastNotification toastNotification)
        {
            _httpClientFactory = httpClientFactory;
            _toast = toastNotification;
        }
        public async Task<IActionResult> Index()
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
                var response = await client.GetAsync("http://localhost:5109/api/Course/List");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var courses = JsonSerializer.Deserialize<List<CoursesResponse>>(data, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    var selectModel = new StudentSelectCourse();
                    selectModel.CourseSelectList = new List<SelectListItem>();
                    foreach (var course in courses)
                    {
                        selectModel.CourseSelectList.Add(new SelectListItem() { Text = course.CourseName, Value = course.CourseId });
                    }
                    _toast.AddErrorToastMessage("Student course succes");
                    return View(selectModel);
                    
                }
                else
                {
                    _toast.AddErrorToastMessage("Student course exits");
                    return RedirectToAction("List", "Course");
                }

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(StudentSelectCourse model, CancellationToken cancellationToken)
        {

            var token = User.Claims.FirstOrDefault(it => it.Type == "accesToken").Value;
            var expireDate = User.Claims.FirstOrDefault(it => it.Type == "exp").Value;
            var date = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expireDate)).DateTime;

            var studentId = Convert.ToInt32(User.Claims.FirstOrDefault(it => it.Type == "AccountId").Value);
            
            var requestModel = new StudentCourseModel() { courseId = model.SelectedCourse, studentId = studentId };

            if (date < DateTime.UtcNow)
                return RedirectToAction("Login", "Authentication");
            if (token != null)
            {
                var client = this._httpClientFactory.CreateClient();
                var content = new StringContent(JsonSerializer.Serialize(requestModel), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.PostAsync("http://localhost:5109/api/Student/AddCourse", content);
                if (response.IsSuccessStatusCode)
                {
                    _toast.AddSuccessToastMessage("Student course succes");
                    return RedirectToAction("List", "Course");
                }
                else
                {
                    _toast.AddErrorToastMessage("Student course exits");
                }

            }
            return RedirectToAction("Index", "Course");
        }

        [Authorize(Roles = "student")]
        public async Task<IActionResult> List(CancellationToken cancellationToken)
        {
            var token = User.Claims.FirstOrDefault(it => it.Type == "accesToken").Value;
            var expireDate = User.Claims.FirstOrDefault(it => it.Type == "exp").Value;
            var date = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expireDate)).DateTime; ;

            var studentId = Convert.ToInt32(User.Claims.FirstOrDefault(it => it.Type == "AccountId").Value);

            if (date < DateTime.UtcNow)
                return RedirectToAction("Login", "Authentication");
            if (token != null)
            {
                var client = this._httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(string.Format("http://localhost:5109/api/Course/StudentCourses?studentId={0}", studentId));
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync(cancellationToken);
                    var courses = JsonSerializer.Deserialize<List<CoursesResponse>>(data, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    return View(courses);
                }

            }
            return View();

        }

    }
}
