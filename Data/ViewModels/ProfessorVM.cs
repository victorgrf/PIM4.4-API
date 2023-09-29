using System.ComponentModel.DataAnnotations;

namespace API.Data.ViewModels
{
    public class Professor_Input
    {
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

    public class Professor
    {
        public int id { get; set; }
        public string nome { get; set; }
        public long cpf { get; set; }
        public long rg { get; set; }
        public long? telefone { get; set; }
        public string email { get; set; }
        public string cargo { get; set; }
        public List<DisciplinaMinistrada>? disciplinasMinistradas { get; set; }
    }
}
