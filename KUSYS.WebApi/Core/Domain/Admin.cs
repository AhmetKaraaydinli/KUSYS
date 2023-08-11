namespace KUSYS.WebApi.Core.Domain
{
    public class Admin
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; } = Role.admin;
    }
}
