using Microsoft.AspNetCore.Mvc;

namespace API.Data.Errors
{
    public class RelatedTableError
    {
        public string title { get; private set; }
        public string description { get; private set; }
        public int status { get; private set; }
        public List<Object> tables { get; private set; }
        public RelatedTableError()
        {
            this.title = "Cadastros relacionados impedindo o DELETE.";
            this.description = "Este cadastro não pode ser deletado pois está ligado a outro(s) no banco de dados. Delete os cadastros em questão para permitir esta ação.";
            this.status = GetStatusCode();
            this.tables = new List<Object>();
        }

        public int GetStatusCode()
        {
            return StatusCodes.Status418ImATeapot;
        }

        public void AddTable(string tipo, List<int> ids)
        {
            tables.Add(new
            {
                tipo = tipo,
                id = ids
            });
        }
    }
}
