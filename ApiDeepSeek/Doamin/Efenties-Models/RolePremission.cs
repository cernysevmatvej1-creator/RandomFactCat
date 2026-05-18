using System.ComponentModel.DataAnnotations;

namespace ApiDeepSeek.Doamin.Efenties_Models
{
    public class RolePermission
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        public Role Role { get; set; } = null!;
        public Permission Permission { get; set; } = null!;
    }
}
