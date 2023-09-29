using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Data.Models
{
    [Table("pessoa")]
    public class Pessoa
    {
        [Key] public int id { get; set; }
        public string senha { get; set; }
        public bool senhaAlterada { get; set; }
        public string nome { get; set; }
        public long cpf { get; set; }
        public long rg { get; set; }
        public long? telefone { get; set;}
        public string email { get; set; }
        public string cargo { get; set; }
    }
}
