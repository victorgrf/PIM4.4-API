using API.DataBase;
using API.Data.ViewModels;
using API.Data.Models;
using API.Data.Identity;

namespace API.Data.Services
{
    public class AlunoService
    {
        private readonly DBContext context;
        public AlunoService(DBContext context)
        {
            this.context = context;
        }

        public List<ViewModels.Aluno> ServiceGetAll(string? nome)
        {
            var response = this.context.Alunos
                .Where(nome != null ? n => n.nome.Contains(nome) : n => n.id != 0)
                .Select(aluno => new ViewModels.Aluno()
                {
                    id = aluno.id,
                    nome = aluno.nome,
                    cpf = aluno.cpf,
                    rg = aluno.rg,
                    telefone = aluno.telefone,
                    email = aluno.email,
                    cargo = aluno.cargo
                }).ToList();
            return response;
        }

        public ViewModels.Aluno ServiceGet(int id)
        {
            var response = this.context.Alunos
                .Where(n => n.id == id)
                .Select(aluno => new ViewModels.Aluno()
                {
                    id = aluno.id,
                    nome = aluno.nome,
                    cpf = aluno.cpf,
                    rg = aluno.rg,
                    telefone = aluno.telefone,
                    email = aluno.email,
                    cargo = aluno.cargo
                }).FirstOrDefault();

            return response;
        }

        public void ServicePost(Aluno_Input aluno)
        {
            var obj = new Models.Aluno()
            {
                senha = aluno.senha,
                nome = aluno.nome,
                cpf = aluno.cpf,
                rg = aluno.rg,
                telefone = aluno.telefone,
                email = aluno.email,
                cargo = Roles.Aluno
            };

            this.context.Add(obj);
            this.context.SaveChanges();
        }

        public void ServicePut(int id, Aluno_Input aluno)
        {
            var obj = this.context.Alunos.FirstOrDefault(n => n.id == id);

            obj.senha = aluno.senha;
            obj.nome = aluno.nome;
            obj.cpf = aluno.cpf;
            obj.rg = aluno.rg;
            obj.telefone = aluno.telefone;
            obj.email = aluno.email;

            this.context.SaveChanges();
        }

        public void ServiceDelete(Models.Aluno aluno)
        {
            this.context.Remove(aluno);
            this.context.SaveChanges();
        }
    }
}
