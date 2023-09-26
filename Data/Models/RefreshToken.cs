using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Models
{
    [Table("refreshtoken")]
    public class RefreshToken
    {
        [Key]
        public int id { get; set; }
        public string refreshToken { get; set; }
        public DateTime criadoEm { get; set; }
        public DateTime expiraEm { get; set; }
    }
}
