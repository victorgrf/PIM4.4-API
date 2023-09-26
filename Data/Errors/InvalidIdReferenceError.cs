using Microsoft.AspNetCore.Mvc;

namespace API.Data.Errors
{
    public class InvalidIdReferenceError
    {
        public string title { get; private set; }
        public int status { get; private set; }
        public List<Object> ids { get; private set; }
        public InvalidIdReferenceError()
        {
            this.title = "O(s) id de referencia fornecido não corresponde a nenhum cadastro.";
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
