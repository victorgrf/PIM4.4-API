using Microsoft.AspNetCore.Mvc;

namespace API.Data.Errors
{
    public class NotRelatedError
    {
        public string title { get; private set; }
        public int status { get; private set; }
        public List<Object> tables { get; private set; }
        public int cursoFornecido { get; private set; }
        public int cursoNaTurma { get; private set; }

        public NotRelatedError(int cursoFornecido, int cursoNaTurma)
        {
            this.title = "O curso fornecido não coincide com o curso relacionado a turma fornecida.";
            this.status = GetStatusCode();
            this.tables = new List<Object>();
            this.cursoFornecido = cursoFornecido;
            this.cursoNaTurma = cursoNaTurma;
        }

        public int GetStatusCode()
        {
            return StatusCodes.Status400BadRequest;
        }
    }
}
