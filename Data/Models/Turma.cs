using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Data.Models
{
    [Table("turma")]
    public class Turma
    {
        [Key] public int id { get; set; }
        public string nome { get; set; }
        public int idCurso { get; set; }
    }
}
