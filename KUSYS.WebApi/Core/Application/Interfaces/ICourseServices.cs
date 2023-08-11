using AutoMapper;
using KUSYS.WebApi.Core.Application.Dto;
using KUSYS.WebApi.Core.Application.Model;
using KUSYS.WebApi.Core.Domain;
using KUSYS.WebApi.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace KUSYS.WebApi.Core.Application.Interfaces
{
    public interface ICourseServices
    {
        Task<List<CoursesResponse>> GetCourses(CancellationToken cancellationToken);
        Task<List<CoursesResponse>> GetStudentCourses(int studentId, CancellationToken cancellationToken);
    }
    public class CourseServices : ICourseServices
    {
        private readonly KuysContext _kuysContext;
        private readonly IMapper _mapper;
        public CourseServices(KuysContext kuysContext, IMapper mapper)
        {
            _kuysContext = kuysContext;
            _mapper = mapper;
        }
        public async Task<List<CoursesResponse>> GetCourses(CancellationToken cancellationToken)
        {
            var courseList = await _kuysContext.Courses.ToListAsync(cancellationToken);
            return courseList.Select(it => new CoursesResponse { CourseId = it.CourseId, CourseName = it.CourseName }).ToList();
        }

        public async Task<List<CoursesResponse>> GetStudentCourses(int studentId, CancellationToken cancellationToken)
        {
            var student = await _kuysContext.Students.Include(it => it.Courses).FirstOrDefaultAsync(it=>it.StudentId==studentId,cancellationToken);
            return student.Courses.Select(it => new CoursesResponse { CourseId = it.CourseId, CourseName = it.CourseName }).ToList();
        }

    }
}
