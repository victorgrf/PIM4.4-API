using System.ComponentModel.DataAnnotations;

namespace API.Data.ViewModels
{
    public class Disciplina_Input
    {
        [Required] [MaxLength(255)] public string nome { get; set; }
    }

    public class Disciplina
    {
        public int id { get; set; }
        public string nome { get; set; }
    }
}
