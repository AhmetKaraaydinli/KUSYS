using AutoMapper;
using KUSYS.WebApi.Core.Application.Interfaces;
using KUSYS.WebApi.Core.Application.Mappings;
using KUSYS.WebApi.Core.Tools;
using KUSYS.WebApi.Persistance.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidAudience = JwtTokenDefaults.ValidAudience,
        ValidIssuer = JwtTokenDefaults.ValidIssuer,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenDefaults.Key)),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true
    };

});

builder.Services.AddScoped<IStudentService,StudentService>();
builder.Services.AddScoped<ICourseServices, CourseServices>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<KuysContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});



builder.Services.AddAutoMapper(options =>
{
    options.AddProfiles(new List<Profile>()
    {
        new StudentProfile()
    });
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger");
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();