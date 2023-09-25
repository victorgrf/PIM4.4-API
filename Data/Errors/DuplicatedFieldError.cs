using Microsoft.AspNetCore.Mvc;

namespace API.Data.Errors
{
    public class DuplicatedFieldError
    {
        public string title {  get; private set; }
        public int status { get; private set; }
        public List<string> fields { get; private set; }
        public DuplicatedFieldError()
        {
            this.title = "Um ou mais dados enviados conflitem com os de outro cadastro.";
            this.status = 409;
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
