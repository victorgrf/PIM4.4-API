using System.Security.Cryptography;
using System.Text;

namespace API.Data.Identity
{
    public class Criptografia
    {
        // Gerando o hash da senha
        public static string CriptografarSenha(string senha)
        {
            var hashes = SHA1.Create().ComputeHash(new ASCIIEncoding().GetBytes(senha));

            var stringBuilder = new StringBuilder();
            foreach (var item in hashes)
            {
                stringBuilder.Append(item.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}
