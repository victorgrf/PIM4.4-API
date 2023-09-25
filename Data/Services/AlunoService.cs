using API.DataBase;
using API.Data.ViewModels;
using API.Data.Models;

namespace API.Data.Services
{
    public class SecretarioService
    {
        private readonly DBContext context;
        public SecretarioService(DBContext context)
        {
            this.context = context;
        }

        public List<ViewModels.Secretario> ServiceGetAll(string? nome)
        {
            var response = this.context.Secretarios
                .Where(nome != null ? n => n.nome.Contains(nome) : n => n.id != 0)
                .Select(secretario => new ViewModels.Secretario()
                {
                    id = secretario.id,
                    senha = secretario.senha,
                    nome = secretario.nome,
                    cpf = secretario.cpf,
                    rg = secretario.rg,
                    telefone = secretario.telefone,
                    email = secretario.email
                }).ToList();
            return response;
        }

        public ViewModels.Secretario ServiceGet(int id)
        {
            var response = this.context.Secretarios
                .Where(n => n.id == id)
                .Select(secretario => new ViewModels.Secretario()
                {
                    id = secretario.id,
                    senha = secretario.senha,
                    nome = secretario.nome,
                    cpf = secretario.cpf,
                    rg = secretario.rg,
                    telefone = secretario.telefone,
                    email = secretario.email
                }).FirstOrDefault();

            return response;
        }

        public void ServicePost(Secretario_Input secretario)
        {
            var obj = new Models.Secretario()
            {
                senha = secretario.senha,
                nome = secretario.nome,
                cpf = secretario.cpf,
                rg = secretario.rg,
                telefone = secretario.telefone,
                email = secretario.email
            };

            this.context.Add(obj);
            this.context.SaveChanges();
        }

        public void ServicePut(int id, Secretario_Input secretario)
        {
            var obj = this.context.Secretarios.FirstOrDefault(n => n.id == id);

            obj.senha = secretario.senha;
            obj.nome = secretario.nome;
            obj.cpf = secretario.cpf;
            obj.rg = secretario.rg;
            obj.telefone = secretario.telefone;
            obj.email = secretario.email;

            this.context.SaveChanges();
        }

        public void ServiceDelete(Models.Secretario secretario)
        {
            this.context.Remove(secretario);
            this.context.SaveChanges();
        }
    }
}
