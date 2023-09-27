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

            foreach (var r in response)
            {
                List<Models.Curso_Disciplina> cursos_disciplinas = this.context.Curso_Disciplinas
                .Where(e => e.idCurso == r.id).ToList();

                if (cursos_disciplinas.Count > 0)
                {
                    r.disciplinas = new List<ViewModels.Disciplina?>();
                    foreach (var cd in cursos_disciplinas)
                    {
                        r.disciplinas.Add(this.context.Disciplinas
                        .Where(n => n.id == cd.idDisciplina)
                        .Select(disciplina => new ViewModels.Disciplina()
                        {
                            id = disciplina.id,
                            nome = disciplina.nome
                        }).FirstOrDefault());
                    }
                }
            }
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

            List<Models.Curso_Disciplina> cursos_disciplinas = this.context.Curso_Disciplinas
            .Where(e => e.idCurso == response.id).ToList();

            if (cursos_disciplinas.Count > 0)
            {
                response.disciplinas = new List<ViewModels.Disciplina?>();
                foreach (var cd in cursos_disciplinas)
                {
                    response.disciplinas.Add(this.context.Disciplinas
                    .Where(n => n.id == cd.idDisciplina)
                    .Select(disciplina => new ViewModels.Disciplina()
                    {
                        id = disciplina.id,
                        nome = disciplina.nome
                    }).FirstOrDefault());
                }
            }


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

        public void ServiceAddDisciplina(ViewModels.Curso_Disciplina_Input curso_disciplina)
        {
            var obj = new Models.Curso_Disciplina(curso_disciplina.idCurso, curso_disciplina.idDisciplina);
            this.context.Add(obj);
            this.context.SaveChanges();
        }

        public void ServiceRemoveDisciplina(Models.Curso_Disciplina curso_disciplina)
        {
            this.context.Remove(curso_disciplina);
            this.context.SaveChanges();
        }
    }
}
