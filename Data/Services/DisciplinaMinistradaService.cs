using API.DataBase;
using API.Data.ViewModels;
using API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Services
{
    public class DisciplinaMinistradaService
    {
        private readonly DBContext context;
        public DisciplinaMinistradaService(DBContext context)
        {
            this.context = context;
        }

        public List<ViewModels.DisciplinaMinistrada> ServiceGetAll()
        {
            var response = this.context.DisciplinaMinistradas
                .Where(n => n.id != 0)
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
                    professor = this.context.Professores
                        .Where(n => n.id == disciplinaMinistrada.idProfessor)
                        .Select(professor => new ViewModels.Professor()
                        {
                            id = professor.id,
                            nome = professor.nome,
                            cpf = professor.cpf,
                            rg = professor.rg,
                            telefone = professor.telefone,
                            email = professor.email,
                            cargo = professor.cargo
                        }).FirstOrDefault(),
                    encerrada = disciplinaMinistrada.encerrada,
                    coordenador = disciplinaMinistrada.coordenador
                }).ToList();
            return response;
        }

        public ViewModels.DisciplinaMinistrada ServiceGet(int id)
        {
            var response = this.context.DisciplinaMinistradas
                .Where(n => n.id == id)
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
                    professor = this.context.Professores
                        .Where(n => n.id == disciplinaMinistrada.idProfessor)
                        .Select(professor => new ViewModels.Professor()
                        {
                            id = professor.id,
                            nome = professor.nome,
                            cpf = professor.cpf,
                            rg = professor.rg,
                            telefone = professor.telefone,
                            email = professor.email,
                            cargo = professor.cargo
                        }).FirstOrDefault(),
                    encerrada = disciplinaMinistrada.encerrada,
                    coordenador = disciplinaMinistrada.coordenador
                }).FirstOrDefault();

            return response;
        }

        public void ServicePost(DisciplinaMinistrada_Input disciplinaMinistrada)
        {
            DisciplinaMinistrada_Input DisciplinaMinistrada = disciplinaMinistrada;
            var obj = new Models.DisciplinaMinistrada()
            {
                idDisciplina = disciplinaMinistrada.idDisciplina,
                idTurma = disciplinaMinistrada.idTurma,
                idProfessor = disciplinaMinistrada.idProfessor,
                encerrada = disciplinaMinistrada.encerrada,
                coordenador = disciplinaMinistrada.coordenador
            };

            this.context.Add(obj);
            this.context.SaveChanges();
        }

        public void ServicePut(int id, DisciplinaMinistrada_Input disciplinaMinistrada)
        {
            var obj = this.context.DisciplinaMinistradas.FirstOrDefault(n => n.id == id);

            obj.idDisciplina = disciplinaMinistrada.idDisciplina;
            obj.idTurma = disciplinaMinistrada.idTurma;
            obj.idProfessor = disciplinaMinistrada.idProfessor;
            obj.encerrada = disciplinaMinistrada.encerrada;
            obj.coordenador = disciplinaMinistrada.coordenador;

            this.context.SaveChanges();
        }

        public void ServiceDelete(Models.DisciplinaMinistrada disciplinaMinistrada)
        {
            this.context.Remove(disciplinaMinistrada);
            this.context.SaveChanges();
        }
    }
}
