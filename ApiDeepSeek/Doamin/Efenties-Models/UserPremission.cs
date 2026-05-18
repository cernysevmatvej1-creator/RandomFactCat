using ApiDeepSeek.models;
using System.ComponentModel.DataAnnotations;

namespace ApiDeepSeek.Doamin.Efenties_Models
{
    public class UserPermission
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PermissionId { get; set; }

     
        public Permission Permission { get; set; } = null!;
    }
}
