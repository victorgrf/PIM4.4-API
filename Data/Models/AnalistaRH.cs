using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Data.Models
{
    [Table("analistarh")]
    public class AnalistaRH : Pessoa
    {
        [Key]
        public new int id { get; set; }
    }
}
