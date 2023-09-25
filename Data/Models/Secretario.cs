using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Models
{
    [Table("secretario")]
    public class Secretario : Pessoa
    {
        [Key]
        public new int id { get; set; }
    }
}
