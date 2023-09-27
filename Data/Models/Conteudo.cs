using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Data.Models
{
    [Table("conteudo")]
    public class Conteudo
    {
        [Key] public int id { get; set; }
        public string documentoURL { get; set; }
        public int idDisciplinaMinistrada { get; set; }
    }
}
