using API.DataBase;
using API.Data.ViewModels;
using API.Data.Models;

namespace API.Data.Services
{
    public class CursoService
    {
        private readonly DBContext context;
        public CursoService(DBContext context)
        {
            this.context = context;
        }

        public List<ViewModels.Curso> ServiceGetAll(string? nome)
        {
            var response = this.context.Cursos
                .Where(nome != null ? n => n.nome.Contains(nome) : n => n.id != 0)
                .Select(curso => new ViewModels.Curso()
                {
                    id = curso.id,
                    nome = curso.nome,
                    cargaHoraria = curso.cargaHoraria,
                    aulasTotais = curso.aulasTotais
                }).ToList();
            return response;
        }

        public ViewModels.Curso ServiceGet(int id)
        {
            var response = this.context.Cursos
                .Where(n => n.id == id)
                .Select(curso => new ViewModels.Curso()
                {
                    id = curso.id,
                    nome = curso.nome,
                    cargaHoraria = curso.cargaHoraria,
                    aulasTotais = curso.aulasTotais
                }).FirstOrDefault();

            return response;
        }

        public void ServicePost(Curso_Input curso)
        {
            Curso_Input Curso = curso;
            var obj = new Models.Curso()
            {
                nome = curso.nome,
                cargaHoraria = curso.cargaHoraria,
                aulasTotais = curso.aulasTotais
            };

            this.context.Add(obj);
            this.context.SaveChanges();
        }

        public void ServicePut(int id, Curso_Input curso)
        {
            var obj = this.context.Cursos.FirstOrDefault(n => n.id == id);

            obj.nome = curso.nome;
            obj.cargaHoraria = curso.cargaHoraria;
            obj.aulasTotais = curso.aulasTotais;

            this.context.SaveChanges();
        }

        public void ServiceDelete(Models.Curso curso)
        {
            this.context.Remove(curso);
            this.context.SaveChanges();
        }
    }
}
