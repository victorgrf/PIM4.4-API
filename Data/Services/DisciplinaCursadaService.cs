using API.DataBase;
using API.Data.ViewModels;
using API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Services
{
    public class DisciplinaCursadaService
    {
        private readonly DBContext context;
        public DisciplinaCursadaService(DBContext context)
        {
            this.context = context;
        }

        public List<ViewModels.DisciplinaCursada> ServiceGetAll(int? idCursoMatriculado)
        {
            var response = this.context.DisciplinaCursadas
            .Where(idCursoMatriculado != null ? n => n.idCursoMatriculado == idCursoMatriculado : n => n.id != 0)
            .Select(disciplinaCursada => new ViewModels.DisciplinaCursada()
            {
                id = disciplinaCursada.id,
                prova1 = disciplinaCursada.prova1,
                prova2 = disciplinaCursada.prova2,
                trabalho = disciplinaCursada.trabalho,
                media = disciplinaCursada.media,
                faltas = disciplinaCursada.faltas,
                situacao = disciplinaCursada.situacao,
                frequencia = disciplinaCursada.frequencia,
                disciplina = this.context.Disciplinas
                .Where(n => n.id == disciplinaCursada.idDisciplina)
                .Select(disciplina => new ViewModels.Disciplina()
                {
                    id = disciplina.id,
                    nome = disciplina.nome
                }).FirstOrDefault(),
                cursoMatriculado = this.context.CursoMatriculados
                .Where(n => n.id == disciplinaCursada.idCursoMatriculado)
                .Select(cursoMatriculado => new ViewModels.CursoMatriculado()
                {
                    id = cursoMatriculado.id,
                    semestreAtual = cursoMatriculado.semestreAtual,
                    trancado = cursoMatriculado.trancado,
                    finalizado = cursoMatriculado.finalizado,
                    turma = this.context.Turmas
                    .Where(n => n.id == cursoMatriculado.idTurma)
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
                    curso = this.context.Cursos
                    .Where(n => n.id == cursoMatriculado.idCurso)
                    .Select(curso => new ViewModels.Curso()
                    {
                        id = curso.id,
                        nome = curso.nome,
                        cargaHoraria = curso.cargaHoraria,
                        aulasTotais = curso.aulasTotais
                    }).FirstOrDefault(),
                    aluno = this.context.Alunos
                    .Where(n => n.id == cursoMatriculado.idAluno)
                    .Select(aluno => new ViewModels.Aluno()
                    {
                        id = aluno.id,
                        nome = aluno.nome,
                        cpf = aluno.cpf,
                        rg = aluno.rg,
                        telefone = aluno.telefone,
                        email = aluno.email,
                        cargo = aluno.cargo
                    }).FirstOrDefault()
                }).FirstOrDefault()
            }).ToList();
            return response;
        }

        public ViewModels.DisciplinaCursada ServiceGet(int id)
        {
            var response = this.context.DisciplinaCursadas
            .Where(n => n.id == id)
            .Select(disciplinaCursada => new ViewModels.DisciplinaCursada()
            {
                id = disciplinaCursada.id,
                prova1 = disciplinaCursada.prova1,
                prova2 = disciplinaCursada.prova2,
                trabalho = disciplinaCursada.trabalho,
                media = disciplinaCursada.media,
                faltas = disciplinaCursada.faltas,
                situacao = disciplinaCursada.situacao,
                frequencia = disciplinaCursada.frequencia,
                disciplina = this.context.Disciplinas
                .Where(n => n.id == disciplinaCursada.idDisciplina)
                .Select(disciplina => new ViewModels.Disciplina()
                {
                    id = disciplina.id,
                    nome = disciplina.nome
                }).FirstOrDefault(),
                cursoMatriculado = this.context.CursoMatriculados
                .Where(n => n.id == disciplinaCursada.idCursoMatriculado)
                .Select(cursoMatriculado => new ViewModels.CursoMatriculado()
                {
                    id = cursoMatriculado.id,
                    semestreAtual = cursoMatriculado.semestreAtual,
                    trancado = cursoMatriculado.trancado,
                    finalizado = cursoMatriculado.finalizado,
                    turma = this.context.Turmas
                    .Where(n => n.id == cursoMatriculado.idTurma)
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
                    curso = this.context.Cursos
                    .Where(n => n.id == cursoMatriculado.idCurso)
                    .Select(curso => new ViewModels.Curso()
                    {
                        id = curso.id,
                        nome = curso.nome,
                        cargaHoraria = curso.cargaHoraria,
                        aulasTotais = curso.aulasTotais
                    }).FirstOrDefault(),
                    aluno = this.context.Alunos
                    .Where(n => n.id == cursoMatriculado.idAluno)
                    .Select(aluno => new ViewModels.Aluno()
                    {
                        id = aluno.id,
                        nome = aluno.nome,
                        cpf = aluno.cpf,
                        rg = aluno.rg,
                        telefone = aluno.telefone,
                        email = aluno.email,
                        cargo = aluno.cargo
                    }).FirstOrDefault()
                }).FirstOrDefault()
            }).FirstOrDefault();

            return response;
        }

        public void ServicePost(DisciplinaCursada_Input disciplinaCursada)
        {
            DisciplinaCursada_Input DisciplinaCursada = disciplinaCursada;
            var obj = new Models.DisciplinaCursada()
            {
                prova1 = disciplinaCursada.prova1 != null ? disciplinaCursada.prova1 : null,
                prova2 = disciplinaCursada.prova2 != null ? disciplinaCursada.prova2 : null,
                trabalho = disciplinaCursada.trabalho != null ? disciplinaCursada.trabalho : null,
                faltas = disciplinaCursada.faltas != null ? disciplinaCursada.faltas : null,
                idDisciplina = disciplinaCursada.idDisciplina,
                idCursoMatriculado = disciplinaCursada.idCursoMatriculado,
                situacao = Situacao.Cursando
            };

            this.context.Add(obj);
            this.context.SaveChanges();
        }

        public void ServicePut(int id, DisciplinaCursada_Input disciplinaCursada)
        {
            var obj = this.context.DisciplinaCursadas.FirstOrDefault(n => n.id == id);

            obj.prova1 = disciplinaCursada.prova1 != null ? disciplinaCursada.prova1 : obj.prova1;
            obj.prova2 = disciplinaCursada.prova2 != null ? disciplinaCursada.prova2 : obj.prova2;
            obj.trabalho = disciplinaCursada.trabalho != null ? disciplinaCursada.trabalho : obj.trabalho;
            obj.faltas = disciplinaCursada.faltas != null ? disciplinaCursada.faltas : obj.faltas;
            obj.idDisciplina = disciplinaCursada.idDisciplina;
            obj.idCursoMatriculado = disciplinaCursada.idCursoMatriculado;

            this.context.SaveChanges();
        }

        public void ServiceDelete(Models.DisciplinaCursada disciplinaCursada)
        {
            this.context.Remove(disciplinaCursada);
            this.context.SaveChanges();
        }

        public float ServiceMedia(int id)
        {
            var obj = this.context.DisciplinaCursadas.FirstOrDefault(n => n.id == id);
            var media = ((obj.prova1 * 4) + (obj.prova2 * 4) + (obj.trabalho * 2)) / 10;
            obj.media = media;
            this.context.SaveChanges();
            return (float)obj.media;
        }

        public float ServiceFrequencia(int id)
        {
            var obj = this.context.DisciplinaCursadas.FirstOrDefault(n => n.id == id);
            var cm = this.context.CursoMatriculados.FirstOrDefault(n => n.id == obj.idCursoMatriculado);
            var curso = this.context.Cursos.FirstOrDefault(n => n.id == cm.idCurso);
            var frequencia = 100 - (((float)obj.faltas / (float)curso.aulasTotais) * 100f);
            obj.frequencia = (int)frequencia;
            this.context.SaveChanges();
            return (int)obj.frequencia;
        }

        public string ServiceSituacao(int id)
        {
            var obj = this.context.DisciplinaCursadas.FirstOrDefault(n => n.id == id);
            var situacao = string.Empty;

            if (obj.frequencia == null || obj.media == null)
            {
                situacao = Situacao.Cursando;
                obj.situacao = situacao;
            }

            else if (obj.frequencia >= 75 && obj.media >= 7.5f)
            {
                situacao = Situacao.Aprovado;
                obj.situacao = situacao;
            }

            else
            {
                situacao = Situacao.Reprovado;
                obj.situacao = situacao;
            }

            this.context.SaveChanges();
            return obj.situacao;
        }
    }

    public class Situacao
    {
        public const string Cursando = "Cursando";
        public const string Aprovado = "Aprovado";
        public const string Reprovado = "Reprovado";
    }
}
