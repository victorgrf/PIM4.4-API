using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace API.Data.ViewModels
{
    public class Curso_Disciplina_Input
    {
        [Required]
        [Range(10000, int.MaxValue, ErrorMessage = "O Valor de {0} deveria estar entre {1} e {2}.")]
        public int idCurso { get; set; }

        [Required]
        [Range(10000, int.MaxValue, ErrorMessage = "O Valor de {0} deveria estar entre {1} e {2}.")]
        public int idDisciplina { get; set; }
    }
}
