using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using API.Data.ViewModels;
using API.Data.Models;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using API.DataBase;
using Microsoft.AspNetCore.Mvc;

namespace API.Data.WritersPDF
{
    public class Boletim : WritersPDF.Writer
    {
        public Boletim(IWebHostEnvironment webHostEnvironment, DBContext dbContext) : base(webHostEnvironment, dbContext)
        {
            
        }

        // Função para gerar o boletim em pdf
        public void Gerar(int idAluno)
        {
            // Buscando dados no banco de dados
            var aluno = base.dbContext.Alunos.FirstOrDefault(n => n.id == idAluno);
            var cursoMatriculados = base.dbContext.CursoMatriculados.Where(n => n.idAluno == idAluno).ToList();
            var cursos = new List<Cursos>();
            if (cursoMatriculados.Count > 0) 
                foreach (var cm in cursoMatriculados)
                {
                    var curso = base.dbContext.Cursos.FirstOrDefault(n => n.id == cm.idCurso);
                    var disciplinaCursadas = base.dbContext.DisciplinaCursadas.Where(n => n.idCursoMatriculado == cm.id).ToList();
                    var discplinas = new List<Disciplina>();

                    foreach (var dc in disciplinaCursadas)
                    {
                        var d = base.dbContext.Disciplinas.Where(n => n.id == dc.idDisciplina).FirstOrDefault();
                        discplinas.Add(new Disciplina(d, dc));
                    }
                    cursos.Add(new Cursos(cm, curso, discplinas));
                }

            // Definindo o caminho do arquivo
            var pasta = Path.Combine(base.webHostEnvironment.ContentRootPath, "DataBase\\Files\\Boletims\\");
            base.nome = "boletim " + aluno.nome + " (ID-" + idAluno + ").pdf";
            base.caminho = Path.Combine(pasta, base.nome);

            // Criando ou abrindo o arquivo
            FileStream arquivo;
            if (File.Exists(base.caminho))
                arquivo = new FileStream(base.caminho, FileMode.Open, FileAccess.Write);
            else
                arquivo = new FileStream(base.caminho, FileMode.Create, FileAccess.Write);

            // Preparando para escrever no arquivo
            var writer = PdfWriter.GetInstance(pdf, arquivo);
            base.pdf.Open();

            // Adicionando o PageEvent para o Writer
            writer.PageEvent = new Eventos(this);

            // Escrevendo o título
            var titulo = new Paragraph("Boletim - [Nome da Universidade]\n", base.fonteH1);
            titulo.Alignment = Element.ALIGN_LEFT;
            base.pdf.Add(titulo);

            // Adicionando informacões do aluno
            var alunoP1 = new Paragraph("", base.fonteP1bold);
            alunoP1.Add(new Phrase("Nome do aluno: "));
            alunoP1.Font = base.fonteP1;
            alunoP1.Add(new Phrase(aluno.nome));
            alunoP1.Font = base.fonteP1bold;
            alunoP1.Add(new Phrase(" ID/RA do aluno: "));
            alunoP1.Font = base.fonteP1;
            alunoP1.Add(new Phrase(idAluno.ToString()));

            var alunoP2 = new Paragraph("", base.fonteP1bold);
            alunoP2.Add(new Phrase("CPF: "));
            alunoP2.Font = base.fonteP1;
            alunoP2.Add(new Phrase(aluno.cpf.ToString()));
            alunoP2.Font = base.fonteP1bold;
            alunoP2.Add(new Phrase(" RG: "));
            alunoP2.Font = base.fonteP1;
            alunoP2.Add(new Phrase(aluno.rg.ToString()));

            alunoP1.Alignment = Element.ALIGN_LEFT;
            alunoP2.Alignment = Element.ALIGN_LEFT;
            base.pdf.Add(alunoP1);
            base.pdf.Add(alunoP2);

            // Adicionando a área de cursos
            var cursosP = new Paragraph("\nCursos do aluno (apenas os não finalizados)", base.fonteH2);
            cursosP.Alignment = Element.ALIGN_LEFT;
            base.pdf.Add(cursosP);

            // Desenha uma Linha horizontal
            var line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.Black, Element.ALIGN_LEFT, 1)));
            base.pdf.Add(line);

            // Adicionando cada curso do aluno
            foreach (var curso in cursos)
            {
                if (!curso.cursoMatriculado.finalizado)
                {
                    var cursoT1 = new Paragraph("", base.fonteP1);
                    cursoT1.Add(new Phrase(curso.curso.nome));
                    cursoT1.Font = base.fonteP1bold;
                    cursoT1.Add(new Phrase(" | ID/RA do curso: "));
                    cursoT1.Font = base.fonteP1;
                    cursoT1.Add(new Phrase(curso.cursoMatriculado.idCurso.ToString()));

                    var cursoT2 = new Paragraph("", base.fonteP1bold);
                    cursoT2.Add(new Phrase("ID/RA da turma: "));
                    cursoT2.Font = base.fonteP1;
                    cursoT2.Add(new Phrase(curso.cursoMatriculado.idTurma.ToString()));
                    cursoT2.Font = base.fonteP1bold;
                    cursoT2.Add(new Phrase(" Semestre Atual: "));
                    cursoT2.Font = base.fonteP1;
                    cursoT2.Add(new Phrase(curso.cursoMatriculado.semestreAtual.ToString()));
                    cursoT2.Font = base.fonteP1bold;
                    cursoT2.Add(new Phrase(" Trancado: "));
                    cursoT2.Font = base.fonteP1;
                    cursoT2.Add(new Phrase(base.TraduzirBool(curso.cursoMatriculado.trancado)));
                    cursoT2.Font = base.fonteP1bold;
                    cursoT2.Add(new Phrase(" Finalizado: "));
                    cursoT2.Font = base.fonteP1;
                    cursoT2.Add(new Phrase(base.TraduzirBool(curso.cursoMatriculado.finalizado)));
                    cursoT2.Font = base.fonteP1bold;

                    cursoT1.Alignment = Element.ALIGN_LEFT;
                    cursoT2.Alignment = Element.ALIGN_LEFT;
                    base.pdf.Add(cursoT1);
                    base.pdf.Add(cursoT2);

                    // Adicionando linha em branco (espaçamento)
                    base.pdf.Add(new Paragraph(" ", base.fonteP1));

                    // Adicionando a tabela de disciplinas
                    var tabelaDisciplinas = new PdfPTable(7); // 10
                    tabelaDisciplinas.SetTotalWidth(new float[] {2.35f, 0.5f, 0.75f, 0.6f, 0.6f, 0.6f, 0.6f });
                    tabelaDisciplinas.DefaultCell.BorderWidth = 1;
                    tabelaDisciplinas.WidthPercentage = 100;
                    
                    // Adicionando a primeira célula
                    base.CriarCelula(tabelaDisciplinas, "Nome", Element.ALIGN_LEFT);
                    base.CriarCelula(tabelaDisciplinas, "Faltas", Element.ALIGN_MIDDLE);
                    base.CriarCelula(tabelaDisciplinas, "Frequencia", Element.ALIGN_MIDDLE);
                    base.CriarCelula(tabelaDisciplinas, "Prova 1", Element.ALIGN_MIDDLE);
                    base.CriarCelula(tabelaDisciplinas, "Prova 2", Element.ALIGN_MIDDLE);
                    base.CriarCelula(tabelaDisciplinas, "Trabalho", Element.ALIGN_MIDDLE);
                    base.CriarCelula(tabelaDisciplinas, "Situação", Element.ALIGN_RIGHT);

                    // Adicionando cada disciplina na tabela
                    if (curso.disciplinas.Count > 0)
                    {
                        foreach (var disciplina in curso.disciplinas)
                        {
                            base.CriarCelula(tabelaDisciplinas, disciplina.disciplina.nome, Element.ALIGN_LEFT);
                            base.CriarCelula(tabelaDisciplinas, base.TraduzirInt(disciplina.disciplinaCursada.faltas), Element.ALIGN_MIDDLE);
                            base.CriarCelula(tabelaDisciplinas, base.TraduzirFloat(disciplina.disciplinaCursada.frequencia), Element.ALIGN_MIDDLE);
                            base.CriarCelula(tabelaDisciplinas, base.TraduzirFloat(disciplina.disciplinaCursada.prova1), Element.ALIGN_MIDDLE);
                            base.CriarCelula(tabelaDisciplinas, base.TraduzirFloat(disciplina.disciplinaCursada.prova2), Element.ALIGN_MIDDLE);
                            base.CriarCelula(tabelaDisciplinas, base.TraduzirFloat(disciplina.disciplinaCursada.trabalho), Element.ALIGN_MIDDLE);
                            base.CriarCelula(tabelaDisciplinas, disciplina.disciplinaCursada.situacao, Element.ALIGN_RIGHT);
                        }
                    }

                    base.pdf.Add(tabelaDisciplinas);

                    // Adicionando linha em branco (espaçamento)
                    base.pdf.Add(new Paragraph(" ", base.fonteP1));
                }
            }


            // Fechando o arquivo
            pdf.Close();
            arquivo.Close();
        }
    }
}
