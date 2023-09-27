using Microsoft.AspNetCore.Mvc;

namespace API.Data.Errors
{
    public class InvalidIdReferenceError
    {
        public string title { get; private set; }
        public string description { get; private set; }
        public int status { get; private set; }
        public List<Object> ids { get; private set; }
        public InvalidIdReferenceError()
        {
            this.title = "Id(s) de referencia incorreto(s).";
            this.description = "Um ou mais ids fornecidos deveriam referenciar a cadastros presentes no banco de dados. Estes não tiveram resultados ao buscar por seus respectivos cadastros.";
            this.status = GetStatusCode();
            this.ids = new List<Object>();
        }

        public int GetStatusCode()
        {
            return StatusCodes.Status404NotFound;
        }

        public void AddId(string tipo, int id)
        {
            ids.Add(new
            {
                tipo = tipo,
                id = id
            });
        }
    }
}
