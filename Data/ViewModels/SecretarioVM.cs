using System.ComponentModel.DataAnnotations;

namespace API.Data.ViewModels
{
    public class Secretario_Input
    {
        [Required] [MaxLength(20)] public string senha { get; set; }
        [Required] [MaxLength(255)] public string nome { get; set; }
        [Required] [Range(1, Int64.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")] public Int64 cpf { get; set; }
        [Required] [Range(1, Int64.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")] public Int64 rg { get; set; }
        [Range(1, Int64.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")] public Int64? telefone { get; set; }
        [MaxLength(255)] public string? email { get; set; }
    }

    public class Secretario
    {
        public int id { get; set; }
        public string senha { get; set; }
        public string nome { get; set; }
        public Int64 cpf { get; set; }
        public Int64 rg { get; set; }
        public Int64? telefone { get; set; }
        public string? email { get; set; }
    }
}
