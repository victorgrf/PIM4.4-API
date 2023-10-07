using API.DataBase;
using API.Data.ViewModels;
using API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Services
{
    public class CursoMatriculadoService
    {
        private readonly DBContext context;
        public CursoMatriculadoService(DBContext context)
        {
            this.context = context;
        }

        public List<ViewModels.CursoMatriculado> ServiceGetAll(int? idAluno)
        {
            var response = this.context.CursoMatriculados
            .Where(idAluno != null ? n => n.idAluno == idAluno : n => n.id != 0)
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
                }).FirstOrDefault(),
                disciplinas = this.context.DisciplinaCursadas
                .Where(n => n.idCursoMatriculado == cursoMatriculado.id)
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
                }).ToList()
            }).ToList();
            return response;
        }

        public ViewModels.CursoMatriculado ServiceGet(int id)
        {
            var response = this.context.CursoMatriculados
            .Where(n => n.id == id)
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
                }).FirstOrDefault(),
                disciplinas = this.context.DisciplinaCursadas
                .Where(n => n.idCursoMatriculado == cursoMatriculado.id)
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
                }).ToList()
            }).FirstOrDefault();

            return response;
        }

        public void ServicePost(CursoMatriculado_Input cursoMatriculado)
        {
            CursoMatriculado_Input CursoMatriculado = cursoMatriculado;
            var obj = new Models.CursoMatriculado()
            {
                idAluno = cursoMatriculado.idAluno,
                idTurma = cursoMatriculado.idTurma,
                idCurso = cursoMatriculado.idCurso,
                semestreAtual = cursoMatriculado.semestreAtual,
                trancado = cursoMatriculado.trancado,
                finalizado = cursoMatriculado.finalizado
            };

            this.context.Add(obj);
            this.context.SaveChanges();
        }

        public void ServicePut(int id, CursoMatriculado_Input cursoMatriculado)
        {
            var obj = this.context.CursoMatriculados.FirstOrDefault(n => n.id == id);

            obj.idAluno = cursoMatriculado.idAluno;
            obj.idTurma = cursoMatriculado.idTurma;
            obj.idCurso = cursoMatriculado.idCurso;
            obj.semestreAtual = cursoMatriculado.semestreAtual;
            obj.trancado = cursoMatriculado.trancado;
            obj.finalizado = cursoMatriculado.finalizado;

            this.context.SaveChanges();
        }

        public void ServiceDelete(Models.CursoMatriculado cursoMatriculado)
        {
            this.context.Remove(cursoMatriculado);
            this.context.SaveChanges();
        }
    }
}
