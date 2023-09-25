using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Data.Models
{
    [Table("pessoa")]
    public class Pessoa
    {
        [Key]
        public int id { get; set; }
        public string senha { get; set; }
        public string nome { get; set; }
        public Int64 cpf { get; set; }
        public Int64 rg { get; set; }
        public Int64? telefone { get; set;}
        public string? email { get; set; }
    }
}
