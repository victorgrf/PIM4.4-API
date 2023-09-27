using Microsoft.AspNetCore.Mvc;

namespace API.Data.Errors
{
    public class InvalidFormatError
    {
        public string title { get; private set; }
        public string description { get; private set; }
        public int status { get; private set; }
        public List<Object> ids { get; private set; }
        public InvalidFormatError()
        {
            this.title = "Arquivo em formato não permitido.";
            this.description = "A API não aceita qualquer formato de arquivo, certifique-se de enviar apenas arquivos nos formatos aceitos";
            this.status = GetStatusCode();
            this.ids = new List<Object>();
        }

        public int GetStatusCode()
        {
            return StatusCodes.Status406NotAcceptable;
        }

        public void AddFile(string nome, string recebido, List<string> aceito)
        {
            ids.Add(new
            {
                nomeArquivo = nome,
                fomatoRecebido = recebido,
                formatosAceitos = aceito
            });
        }
    }
}
