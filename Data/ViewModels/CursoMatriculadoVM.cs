using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace API.Data.ViewModels
{
    public class CursoMatriculado_Input
    {
        [Required]
        [Range(10000, int.MaxValue, ErrorMessage = "O Valor de {0} deveria estar entre {1} e {2}.")]
        public int idAluno { get; set; }

        [Required]
        [Range(10000, int.MaxValue, ErrorMessage = "O Valor de {0} deveria estar entre {1} e {2}.")]
        public int idCurso { get; set; }

        [Required]
        [Range(10000, int.MaxValue, ErrorMessage = "O Valor de {0} deveria estar entre {1} e {2}.")]
        public int idTurma { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "O Valor de {0} deveria estar entre {1} e {2}.")]
        public int semestreAtual { get; set; }

        [Required]
        public bool trancado { get; set; }

        [Required]
        public bool finalizado { get; set; }
    }

    public class CursoMatriculado
    {
        public int id { get; set; }
        public Aluno? aluno { get; set; }
        public Curso? curso { get; set; }
        public Turma? turma { get; set; }
        public int semestreAtual { get; set; }
        public bool trancado { get; set; }
        public bool finalizado { get; set; }
    }
}
