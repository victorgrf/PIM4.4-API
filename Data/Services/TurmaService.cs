using API.DataBase;
using API.Data.ViewModels;
using API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Services
{
    public class TurmaService
    {
        private readonly DBContext context;
        public TurmaService(DBContext context)
        {
            this.context = context;
        }

        public List<ViewModels.Turma> ServiceGetAll(string? nome)
        {
            var response = this.context.Turmas
                .Where(nome != null ? n => n.nome.Contains(nome) : n => n.id != 0)
                .Select(turma => new ViewModels.Turma()
                {
                    id = turma.id,
                    nome = turma.nome,
                    curso = this.context.Cursos
                        .Where(n => n.id == turma.idCurso)
                        .Select(curso => new ViewModels.Curso()
                        {
                            id = curso.id,
                            nome = curso.nome,
                            cargaHoraria = curso.cargaHoraria,
                            aulasTotais = curso.aulasTotais
                        }).FirstOrDefault()
                }).ToList();
            return response;
        }

        public ViewModels.Turma ServiceGet(int id)
        {
            var response = this.context.Turmas
                .Where(n => n.id == id)
                .Select(turma => new ViewModels.Turma()
                {
                    id = turma.id,
                    nome = turma.nome,
                    curso = this.context.Cursos
                        .Where(n => n.id == turma.idCurso)
                        .Select(curso => new ViewModels.Curso()
                        {
                            id = curso.id,
                            nome = curso.nome,
                            cargaHoraria = curso.cargaHoraria,
                            aulasTotais = curso.aulasTotais
                        }).FirstOrDefault()
                }).FirstOrDefault();

            return response;
        }

        public void ServicePost(Turma_Input turma)
        {
            Turma_Input Turma = turma;
            var obj = new Models.Turma()
            {
                nome = turma.nome,
                idCurso = turma.idCurso
            };

            this.context.Add(obj);
            this.context.SaveChanges();
        }

        public void ServicePut(int id, Turma_Input turma)
        {
            var obj = this.context.Turmas.FirstOrDefault(n => n.id == id);

            obj.nome = turma.nome;
            obj.idCurso = turma.idCurso;

            this.context.SaveChanges();
        }

        public void ServiceDelete(Models.Turma turma)
        {
            this.context.Remove(turma);
            this.context.SaveChanges();
        }
    }
}
