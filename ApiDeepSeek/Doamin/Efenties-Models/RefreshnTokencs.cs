using System.ComponentModel.DataAnnotations;

namespace ApiDeepSeek.Doamin.Efenties_Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; }      // сама случайная строка
        public string UserId { get; set; }     // чей токен
        public DateTime Expires { get; set; } // когда истекает
        public bool IsRevoked { get; set; }   // отозван или нет
    }
}
