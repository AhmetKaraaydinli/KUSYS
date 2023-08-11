using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace KUSYS.WebApi.Core.Tools
{
    public class JwtTokenDefaults
    {
        public const string ValidAudience = "localhost";
        public const string ValidIssuer = "localhost";
        public const string Key = "TodoAppKocKusysApplication";
        public const int Expire= 4;
    }
}
