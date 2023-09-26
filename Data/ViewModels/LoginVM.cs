using System.ComponentModel.DataAnnotations;

namespace API.Data.ViewModels
{
    public class Login
    {
        [Required] public string senha { get; set; }
        [Required] public int id { get; set; }
    }
}
