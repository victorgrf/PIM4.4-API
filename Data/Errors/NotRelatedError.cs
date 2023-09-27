using Microsoft.AspNetCore.Mvc;

namespace API.Data.Errors
{
    public class NotRelatedError
    {
        public string title { get; private set; }
        public string description { get; private set; }
        public int status { get; private set; }
        public string tipoDeRelacao { get; private set; }
        public int idFornecido { get; private set; }
        public int idNaTabelaReferenciada { get; private set; }

        public NotRelatedError(int idFornecido, int idNaTabelaReferenciada, string tipoDeRelacao)
        {
            this.title = "Dois cadastros deveriam estar relarionados.";
            this.description = "Um id fornecido deveria equivaler ao id de um cadastro referenciado. Certifique-se de enviar o id de um cadastro que realmente exista.";
            this.status = GetStatusCode();
            this.idFornecido = idFornecido;
            this.idNaTabelaReferenciada = idNaTabelaReferenciada;
            this.tipoDeRelacao = tipoDeRelacao;
        }

        public int GetStatusCode()
        {
            return StatusCodes.Status400BadRequest;
        }
    }
}
