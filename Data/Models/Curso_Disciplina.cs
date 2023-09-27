using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Data.Models
{
    [Table("curso_disciplina")]
    public class Curso_Disciplina
    {
        [Key] public int id { get; set; }

        // Relacionamento
        public int idCurso { get; set; }
        public Curso curso { get; set; }

        public int idDisciplina { get; set; }
        public Disciplina disciplina { get; set; }

        public Curso_Disciplina(int idCurso, int idDisciplina)
        {
            this.idCurso = idCurso;
            this.idDisciplina = idDisciplina;
        }
    }
}
