using global::ApiDeepSeek.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ApiDeepSeek.Doamin.Efenties_Models
{


    namespace ApiDeepSeek.Doamin.Efenties_Models
    {
        public class UserRole
        {
            [Key]
            public int Id { get; set; }

            [Required]
            public string UserId { get; set; } = string.Empty;   // внешний ключ к User.UserId (GUID)

            [Required]
            public int RoleId { get; set; }                       // внешний ключ к Role.Id

            
           
        }
    }
}
