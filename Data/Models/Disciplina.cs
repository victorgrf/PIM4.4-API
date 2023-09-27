using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Data.Models
{
    [Table("disciplina")]
    public class Disciplina
    {
        [Key] public int id { get; set; }
        public string nome { get; set; }

        // Relacionamento
        public List<Curso_Disciplina> curso_disciplinas { get; set; }
    }
}
