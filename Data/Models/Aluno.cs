using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Models
{
    [Table("aluno")]
    public class Aluno : Pessoa
    {
        [Key]
        public new int id { get; set; }
    }
}
