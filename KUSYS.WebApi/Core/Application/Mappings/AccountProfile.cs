using AutoMapper;
using KUSYS.WebApi.Core.Application.Dto;
using KUSYS.WebApi.Core.Domain;

namespace KUSYS.WebApi.Core.Application.Mappings
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            this.CreateMap<Student, StudentDto>().ReverseMap();
        }
    }
}
