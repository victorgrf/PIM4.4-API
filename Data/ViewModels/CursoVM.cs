using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace API.Data.ViewModels
{
    public class Curso_Input
    {
        [Required] public string nome { get; set; }
        [Required] [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")] public int cargaHoraria { get; set; }
        [Required] [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")] public int aulasTotais { get; set; }
    }

    public class Curso
    {
        public int id { get; set; }
        public string nome { get; set; }
        public int cargaHoraria { get; set; }
        public int aulasTotais { get; set; }
    }
}
