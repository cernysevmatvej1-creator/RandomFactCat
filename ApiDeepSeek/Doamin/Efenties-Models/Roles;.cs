using ApiDeepSeek.Doamin.Efenties_Models.ApiDeepSeek.Doamin.Efenties_Models;

namespace ApiDeepSeek.Doamin.Efenties_Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
