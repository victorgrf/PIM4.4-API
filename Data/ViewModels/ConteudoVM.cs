using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace API.Data.ViewModels
{
    public class Conteudo_Input_Post
    {
        [Required]
        public IFormFile documento { get; set; }

        [Required]
        [Range(10000, int.MaxValue, ErrorMessage = "O Valor de {0} deveria estar entre {1} e {2}.")]
        public int idDisciplinaMinistrada { get; set; }
    }

    public class Conteudo_Input_Put
    {
        public IFormFile? documento { get; set; }

        [Required]
        [Range(10000, int.MaxValue, ErrorMessage = "O Valor de {0} deveria estar entre {1} e {2}.")]
        public int idDisciplinaMinistrada { get; set; }
    }

    public class Conteudo
    {
        public int id { get; set; }
        public string documentoURL { get; set; }
        public ViewModels.DisciplinaMinistrada? disciplinaMinistrada { get; set; }
    }
}
