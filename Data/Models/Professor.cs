using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Models
{
    [Table("professor")]
    public class Professor : Pessoa
    {
        [Key]
        public new int id { get; set; }
    }
}
