using Microsoft.AspNetCore.Mvc.Rendering;

namespace KUSYS.Front.Models
{
    public class StudentSelectCourse
    {
        public string SelectedCourse { get; set; }
        public List<SelectListItem> CourseSelectList { get; set; }
    }

}
