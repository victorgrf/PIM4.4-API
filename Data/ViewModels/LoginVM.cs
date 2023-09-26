using System.ComponentModel.DataAnnotations;

namespace API.Data.ViewModels
{
    public class Login
    {
        [Required] public string senha { get; set; }
        [Required] public int id { get; set; }
    }

    public class Refresh
    {
        [Required] public string token { get; set; }
        [Required] public string refreshToken { get; set; }
        [Required] public int id { get; set; }
    }
}
