using API.Data.Models;

namespace API.Data.Identity
{
    public static class Roles
    {
        public const string AnalistaRH = "AnalistaRH";
        public const string Secretario = "Secretario";
        public const string Professor = "Professor";
        public const string Aluno = "Aluno";

        public static string Collapse(params string[] roles)
        {
            return String.Join(", ", roles);
        } 
    }

}
