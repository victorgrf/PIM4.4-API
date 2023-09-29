using System.ComponentModel.DataAnnotations;

namespace API.Data.ViewModels
{
    public class Login
    {
        [Required]
        [MaxLength(20)] 
        public string senha { get; set; }
        [Required] 
        public int id { get; set; }
    }

    public class MudarSenha
    {
        [Required]
        public int id { get; set; }

        [Required]
        [MaxLength(20)]
        public string senhaAntiga { get; set; }

        [Required]
        [MaxLength(20)]
        public string senhaNova { get; set; }
    }

    public class Refresh
    {
        [Required] 
        public string token { get; set; }
        [Required] 
        public string refreshToken { get; set; }
        [Required] 
        public int id { get; set; }
    }
}
