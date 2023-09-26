using System.ComponentModel.DataAnnotations;

namespace API.Data.ViewModels
{
    public class Secretario_Input
    {
        [Required]
        [MaxLength(20)]
        public string senha { get; set; }

        [Required]
        [MaxLength(255)]
        public string nome { get; set; }

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "O Valor de {0} deveria estar entre {1} e {2}.")]
        public long cpf { get; set; }

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "O Valor de {0} deveria estar entre {1} e {2}.")]
        public long rg { get; set; }

        [Required]
        [MaxLength(255)]
        public string email { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "O Valor de {0} deveria estar entre {1} e {2}.")]
        public long? telefone { get; set; }
    }

    public class Secretario
    {
        public int id { get; set; }
        public string nome { get; set; }
        public long cpf { get; set; }
        public long rg { get; set; }
        public long? telefone { get; set; }
        public string email { get; set; }
        public string cargo { get; set; }
    }
}
