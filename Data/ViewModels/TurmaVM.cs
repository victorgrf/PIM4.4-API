using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace API.Data.ViewModels
{
    public class Turma_Input
    {
        [Required]
        [MaxLength(255)]
        public string nome { get; set; }

        [Required]
        [Range(10000, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int idCurso { get; set; }
    }

    public class Turma
    {
        public int id { get; set; }
        public string nome { get; set; }
        public ViewModels.Curso? curso { get; set; }
    }
}
