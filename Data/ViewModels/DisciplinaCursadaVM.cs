using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace API.Data.ViewModels
{
    public class DisciplinaCursada_Input
    {
        [Required]
        [Range(10000, int.MaxValue, ErrorMessage = "O Valor de {0} deveria estar entre {1} e {2}.")]
        public int idDisciplina { get; set; }

        [Required]
        [Range(10000, int.MaxValue, ErrorMessage = "O Valor de {0} deveria estar entre {1} e {2}.")]
        public int idCursoMatriculado { get; set; }

        public float? prova1 { get; set; }
        public float? prova2 { get; set; }
        public float? trabalho { get; set; }
        public int? faltas { get; set; } = 0;
    }

    public class DisciplinaCursada
    {
        public int id { get; set; }
        public Disciplina? disciplina { get; set; }
        public CursoMatriculado? cursoMatriculado { get; set; }
        public float? prova1 { get; set; }
        public float? prova2 { get; set; }
        public float? trabalho { get; set; }
        public int? faltas { get; set; }
        public float? media { get; set; }
        public int? frequencia { get; set; }
        public string situacao { get; set; } = "Cursando";
    }
}
