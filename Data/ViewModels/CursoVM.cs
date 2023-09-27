using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace API.Data.ViewModels
{
    public class Curso_Input
    {
        [Required] 
        [MaxLength(255)] 
        public string nome { get; set; }

        [Required] 
        [Range(1, int.MaxValue, ErrorMessage = "O Valor de {0} deveria estar entre {1} e {2}.")] 
        public int cargaHoraria { get; set; }

        [Required] 
        [Range(1, int.MaxValue, ErrorMessage = "O Valor de {0} deveria estar entre {1} e {2}.")] 
        public int aulasTotais { get; set; }
    }

    public class Curso
    {
        public int id { get; set; }
        public string nome { get; set; }
        public int cargaHoraria { get; set; }
        public int aulasTotais { get; set; }
        public List<Disciplina?>? disciplinas{ get; set; }
    }
}
