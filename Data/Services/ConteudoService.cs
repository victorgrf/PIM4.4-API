using API.DataBase;
using API.Data.ViewModels;
using API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Hosting;

namespace API.Data.Services
{
    public class ConteudoService
    {
        private readonly DBContext context;
        public ConteudoService(DBContext context)
        {
            this.context = context;
        }

        public List<ViewModels.Conteudo> ServiceGetAll(int? idDisciplinaMinistrada)
        {
            var response = this.context.Conteudos
            .Where(idDisciplinaMinistrada != null ? n => n.idDisciplinaMinistrada == idDisciplinaMinistrada : n => n.id != 0)
            .Select(conteudo => new ViewModels.Conteudo()
            {
                id = conteudo.id,
                documentoURL = conteudo.documentoURL,
                disciplinaMinistrada = this.context.DisciplinaMinistradas
                .Where(n => n.id == conteudo.idDisciplinaMinistrada)
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
                }).FirstOrDefault()
            }).ToList();
            return response;
        }

        public ViewModels.Conteudo ServiceGet(int id)
        {
            var response = this.context.Conteudos
            .Where(n => n.id == id)
            .Select(conteudo => new ViewModels.Conteudo()
            {
                id = conteudo.id,
                documentoURL = conteudo.documentoURL,
                disciplinaMinistrada = this.context.DisciplinaMinistradas
                .Where(n => n.id == conteudo.idDisciplinaMinistrada)
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
                }).FirstOrDefault()
            }).FirstOrDefault();

            return response;
        }

        public void ServicePost(Conteudo_Input_Post conteudo, string documentoURL)
        {
            Conteudo_Input_Post Conteudo = conteudo;
            var obj = new Models.Conteudo()
            {
                documentoURL = documentoURL,
                idDisciplinaMinistrada = conteudo.idDisciplinaMinistrada,
            };

            this.context.Add(obj);
            this.context.SaveChanges();
        }

        public void ServicePut(int id, Conteudo_Input_Put conteudo, string documentoURL, IWebHostEnvironment webHostEnvironment)
        {
            var obj = this.context.Conteudos.FirstOrDefault(n => n.id == id);

            obj.idDisciplinaMinistrada = conteudo.idDisciplinaMinistrada;

            if (documentoURL != null)
            {
                var pasta = Path.Combine(webHostEnvironment.ContentRootPath, "DataBase\\Files\\Conteudos\\");
                var caminho = Path.Combine(pasta, obj.documentoURL.Remove(0, 19));
                if (File.Exists(caminho)) File.Delete(caminho);
                obj.documentoURL = documentoURL;
            }

            this.context.SaveChanges();
        }

        public void ServiceDelete(Models.Conteudo conteudo)
        {
            this.context.Remove(conteudo);
            this.context.SaveChanges();
        }
    }
}
