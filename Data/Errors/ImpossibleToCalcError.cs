using Microsoft.AspNetCore.Mvc;

namespace API.Data.Errors
{
    public class ImpossibleToCalcError
    {
        public string title { get; private set; }
        public string description { get; private set; }
        public int status { get; private set; }
        public List<string> variaveis { get; private set; }
        public ImpossibleToCalcError()
        {
            this.title = "Impossível calcular.";
            this.description = "Para efetuar este calculo, é necessário que todas as variáveis desta formula não sejam nulos. Neste caso, uma ou mais variáveis são nulas.";
            this.status = GetStatusCode();
            this.variaveis = new List<string>();
        }

        public int GetStatusCode()
        {
            return StatusCodes.Status406NotAcceptable;
        }

        public void AddVariavel(string variavel)
        {
            variaveis.Add(variavel);
        }
    }
}
