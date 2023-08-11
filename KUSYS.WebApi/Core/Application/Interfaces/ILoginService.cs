using AutoMapper;
using KUSYS.WebApi.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace KUSYS.WebApi.Core.Application.Interfaces
{
    public interface ILoginService
    {
        Task<bool> CheckPassword(string password,string email,CancellationToken cancellationToken);
    }
    public class LoginService : ILoginService
    {
        private readonly KuysContext _kuysContext;
        public LoginService(KuysContext kuysContext)
        {

            _kuysContext = kuysContext;
        }
        public async Task<bool> CheckPassword(string password, string email,CancellationToken cancellationToken)
        {
            var student = await _kuysContext.Students.FirstOrDefaultAsync(it=>it.Password==password && it.Email == email, cancellationToken);
            if (student == null)
                return false;
            return true;
        }
    }
}
