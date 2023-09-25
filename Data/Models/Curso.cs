using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Data.Models
{
    [Table("curso")]
    public class Curso
    {
        [Key] public int id { get; set; }
        public string nome { get; set; }
        public int cargaHoraria { get; set; }
        public int aulasTotais { get; set; }
    }
}
