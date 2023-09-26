using API.DataBase;
using API.Data.ViewModels;
using API.Data.Models;
using API.Data.Identity;

namespace API.Data.Services
{
    public class ProfessorService
    {
        private readonly DBContext context;
        public ProfessorService(DBContext context)
        {
            this.context = context;
        }

        public List<ViewModels.Professor> ServiceGetAll(string? nome)
        {
            var response = this.context.Professores
                .Where(nome != null ? n => n.nome.Contains(nome) : n => n.id != 0)
                .Select(professor => new ViewModels.Professor()
                {
                    id = professor.id,
                    nome = professor.nome,
                    cpf = professor.cpf,
                    rg = professor.rg,
                    telefone = professor.telefone,
                    email = professor.email,
                    cargo = professor.cargo,
                    disciplinasMinistradas = this.context.DisciplinaMinistradas
                        .Where(n => n.idProfessor == professor.id)
                        .Select(disciplinaMinistrada => new ViewModels.DisciplinaMinistrada()
                        {
                            id = disciplinaMinistrada.id,
                            disciplina = this.context.Disciplinas
                                .Where(n => n.id == disciplinaMinistrada.idDisciplina)
                                .Select(disciplina => new ViewModels.Disciplina()
                                {
                                    id = disciplina.id,
                                    nome = disciplina.nome
                                }).FirstOrDefault(),
                            turma = this.context.Turmas
                                .Where(n => n.id == disciplinaMinistrada.idTurma)
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
                                }).FirstOrDefault(),
                            coordenador = disciplinaMinistrada.coordenador
                        }).ToList(),
                }).ToList();
            return response;
        }

        public ViewModels.Professor ServiceGet(int id)
        {
            var response = this.context.Professores
                .Where(n => n.id == id)
                .Select(professor => new ViewModels.Professor()
                {
                    id = professor.id,
                    nome = professor.nome,
                    cpf = professor.cpf,
                    rg = professor.rg,
                    telefone = professor.telefone,
                    email = professor.email,
                    cargo = professor.cargo,
                    disciplinasMinistradas = this.context.DisciplinaMinistradas
                        .Where(n => n.idProfessor == professor.id)
                        .Select(disciplinaMinistrada => new ViewModels.DisciplinaMinistrada()
                        {
                            id = disciplinaMinistrada.id,
                            disciplina = this.context.Disciplinas
                                .Where(n => n.id == disciplinaMinistrada.idDisciplina)
                                .Select(disciplina => new ViewModels.Disciplina()
                                {
                                    id = disciplina.id,
                                    nome = disciplina.nome
                                }).FirstOrDefault(),
                            turma = this.context.Turmas
                                .Where(n => n.id == disciplinaMinistrada.idTurma)
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
                                }).FirstOrDefault(),
                            coordenador = disciplinaMinistrada.coordenador
                        }).ToList(),
                }).FirstOrDefault();

            return response;
        }

        public void ServicePost(Professor_Input professor)
        {
            var obj = new Models.Professor()
            {
                senha = professor.senha,
                nome = professor.nome,
                cpf = professor.cpf,
                rg = professor.rg,
                telefone = professor.telefone,
                email = professor.email,
                cargo = Roles.Professor
            };

            this.context.Add(obj);
            this.context.SaveChanges();
        }

        public void ServicePut(int id, Professor_Input professor)
        {
            var obj = this.context.Professores.FirstOrDefault(n => n.id == id);

            obj.senha = professor.senha;
            obj.nome = professor.nome;
            obj.cpf = professor.cpf;
            obj.rg = professor.rg;
            obj.telefone = professor.telefone;
            obj.email = professor.email;

            this.context.SaveChanges();
        }

        public void ServiceDelete(Models.Professor professor)
        {
            this.context.Remove(professor);
            this.context.SaveChanges();
        }
    }
}
