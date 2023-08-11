using AutoMapper;
using KUSYS.WebApi.Core.Application.Dto;
using KUSYS.WebApi.Core.Application.Model;
using KUSYS.WebApi.Core.Domain;
using KUSYS.WebApi.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Principal;

namespace KUSYS.WebApi.Core.Application.Interfaces
{
    public interface IStudentService
    {
        Task<StudentDto> GetEmailStudent(string email, CancellationToken cancellationToken);
        Task<bool> CreateStudent(StudentModel model, CancellationToken cancellationToken);
        Task<bool> DeleteStudent(int studentId, CancellationToken cancellationToken);
        Task<bool> UpdateStudent(int studentId, StudentModel updateAccount, CancellationToken cancellationToken);
        Task<StudentDto> GetStudent(int studentId, CancellationToken cancellationToken);
        Task<bool> AddCourse(int studentId, string courseId, CancellationToken cancellationToken);
        Task<List<Student>> List(CancellationToken cancellationToken);
        Task<List<AllListModel>> GetStudentsCourses(CancellationToken cancellationToken);
    }

    public class StudentService : IStudentService
    {
        private readonly KuysContext _kuysContext;
        private readonly IMapper _mapper;
        public StudentService(KuysContext kuysContext, IMapper mapper)
        {
            _kuysContext = kuysContext;
            _mapper = mapper;
        }
        public async Task<StudentDto> GetEmailStudent(string email, CancellationToken cancellationToken)
        {
            var account = await _kuysContext.Students.FirstOrDefaultAsync(it => it.Email == email, cancellationToken);
            return _mapper.Map<StudentDto>(account);
        }
        public async Task<bool> CreateStudent(StudentModel model, CancellationToken cancellationToken)
        {
            try
            {
                var newAccount = new StudentDto() { Email = model.Email, Password = model.Password, FirstName = model.FirstName, BirthDate = model.BirthDate, LastName = model.LastName,Role = Role.student };
                var obj = _mapper.Map<Student>(newAccount);
                _kuysContext.Students.Add(obj);
                await _kuysContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public async Task<bool> DeleteStudent(int studentId, CancellationToken cancellationToken)
        {
            var account = await _kuysContext.Students.FirstOrDefaultAsync(it => it.StudentId == studentId, cancellationToken);
            if (account == null)
                return false;
            _kuysContext.Students.Remove(account);
            await _kuysContext.SaveChangesAsync(cancellationToken);
            return true;

        }
        public async Task<bool> UpdateStudent(int accountId, StudentModel updateAccount, CancellationToken cancellationToken)
        {
            var isAccount = await _kuysContext.Students.FirstOrDefaultAsync(it => it.StudentId == accountId, cancellationToken);
            if (isAccount == null)
                return false;

            isAccount.Password = updateAccount.Password;
            isAccount.FirstName = updateAccount.FirstName;
            isAccount.LastName = updateAccount.LastName;
            isAccount.Email = updateAccount.Email;
            isAccount.BirthDate = updateAccount.BirthDate;


            _kuysContext.Students.Update(isAccount);
            await _kuysContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        public async Task<List<Student>> List(CancellationToken cancellationToken)
        {
            try
            {
                return await _kuysContext.Students.Where(it=>it.Role!=Role.admin).ToListAsync(cancellationToken);
            }
            catch (Exception exception)
            {

                throw exception;
            }

        }
        public async Task<bool> AddCourse(int studentId, string courseId, CancellationToken cancellationToken)
        {
            var student = await _kuysContext.Students.FirstOrDefaultAsync(it => it.StudentId == studentId,cancellationToken);
            var isCourse = await _kuysContext.Students.Include(it => it.Courses).FirstOrDefaultAsync(cancellationToken);
            if (isCourse.Courses.Any(it => it.CourseId == courseId))
                return false;

            var courses = await _kuysContext.Courses.FirstOrDefaultAsync(it => it.CourseId == courseId, cancellationToken);
            student.Courses.Add(courses);

            await _kuysContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        public async Task<List<AllListModel>> GetStudentsCourses(CancellationToken cancellationToken)
        {
            var records = new List<AllListModel>();
            var students = _kuysContext.Students.Include(it=>it.Courses);

            foreach (Student student in students)
            {
                var record = new AllListModel() { BirthDate = student.BirthDate,FirstName= student.FirstName,LastName = student.LastName,Courses = student.Courses.Select(it=>it.CourseName).ToList()};
                records.Add(record);
            }
            return records;
        }

        public async Task<StudentDto> GetStudent(int studentId, CancellationToken cancellationToken)
        {
            var student = await _kuysContext.Students.FirstOrDefaultAsync(it => it.StudentId == studentId, cancellationToken);
            return _mapper.Map<StudentDto>(student);
        }

    }
    public class AllListModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public List<string>Courses{ get; set; }
    }
}
