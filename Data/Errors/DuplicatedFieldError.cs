using Microsoft.AspNetCore.Mvc;

namespace API.Data.Errors
{
    public class DuplicatedFieldError
    {
        public string title {  get; private set; }
        public string description { get; private set; }
        public int status { get; private set; }
        public List<string> fields { get; private set; }
        public DuplicatedFieldError()
        {
            this.title = "Dados em conflito";
            this.description = "Um ou mais dados enviados para este cadastro que não podem se repitir conflitem com os de outro cadastro que já está no banco de dados.";
            this.status = GetStatusCode();
            this.fields = new List<string>();
        }

        public int GetStatusCode()
        {
            return StatusCodes.Status409Conflict;
        }

        public void AddField(string field)
        {
            fields.Add(field);
        }
    }
}
