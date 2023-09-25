namespace API.Data.ViewModels
{
    public class Professor_Input
    {
        public string senha { get; set; }
        public string nome { get; set; }
        public Int64 cpf { get; set; }
        public Int64 rg { get; set; }
        public Int64? telefone { get; set; }
        public string? email { get; set; }
    }

    public class Professor
    {
        public int id { get; set; }
        public string senha { get; set; }
        public string nome { get; set; }
        public Int64 cpf { get; set; }
        public Int64 rg { get; set; }
        public Int64? telefone { get; set; }
        public string? email { get; set; }
    }
}
