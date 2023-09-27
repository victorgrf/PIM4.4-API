using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace API.Data.ViewModels
{
    public class DisciplinaMinistrada_Input
    {
        [Required]
        [Range(10000, int.MaxValue, ErrorMessage = "O Valor de {0} deveria estar entre {1} e {2}.")]
        public int idDisciplina { get; set; }

        [Required]
        [Range(10000, int.MaxValue, ErrorMessage = "O Valor de {0} deveria estar entre {1} e {2}.")]
        public int idTurma { get; set; }

        [Required]
        [Range(10000, int.MaxValue, ErrorMessage = "O Valor de {0} deveria estar entre {1} e {2}.")]
        public int idProfessor { get; set; }

        [Required]
        public bool encerrada { get; set; }

        [Required]
        public bool coordenador { get; set; }
    }

    public class DisciplinaMinistrada
    {
        public int id { get; set; }
        public ViewModels.Disciplina? disciplina { get; set; }
        public ViewModels.Turma? turma { get; set; }
        public ViewModels.Professor? professor { get; set; }
        public bool encerrada { get; set; }
        public bool coordenador { get; set; }
    }
}
