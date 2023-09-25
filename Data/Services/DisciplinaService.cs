using API.DataBase;
using API.Data.ViewModels;
using API.Data.Models;

namespace API.Data.Services
{
    public class DisciplinaService
    {
        private readonly DBContext context;
        public DisciplinaService(DBContext context)
        {
            this.context = context;
        }

        public List<ViewModels.Disciplina> ServiceGetAll(string? nome)
        {
            var response = this.context.Disciplinas
                .Where(nome != null ? n => n.nome.Contains(nome) : n => n.id != 0)
                .Select(disciplina => new ViewModels.Disciplina()
                {
                    id = disciplina.id,
                    nome = disciplina.nome
                }).ToList();
            return response;
        }

        public ViewModels.Disciplina ServiceGet(int id)
        {
            var response = this.context.Disciplinas
                .Where(n => n.id == id)
                .Select(disciplina => new ViewModels.Disciplina()
                {
                    id = disciplina.id,
                    nome = disciplina.nome
                }).FirstOrDefault();

            return response;
        }

        public void ServicePost(Disciplina_Input disciplina)
        {
            var obj = new Models.Disciplina()
            {
                nome = disciplina.nome
            };

            this.context.Add(obj);
            this.context.SaveChanges();
        }

        public void ServicePut(int id, Disciplina_Input disciplina)
        {
            var obj = this.context.Disciplinas.FirstOrDefault(n => n.id == id);

            obj.nome = disciplina.nome;

            this.context.SaveChanges();
        }

        public void ServiceDelete(Models.Disciplina disciplina)
        {
            this.context.Remove(disciplina);
            this.context.SaveChanges();
        }
    }
}
