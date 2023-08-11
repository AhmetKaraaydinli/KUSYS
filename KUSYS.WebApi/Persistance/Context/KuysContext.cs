using KUSYS.WebApi.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace KUSYS.WebApi.Persistance.Context
{
    public class KuysContext : DbContext
    {
        public KuysContext(DbContextOptions<KuysContext> options) : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
