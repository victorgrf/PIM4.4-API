using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using API.Data.ViewModels;
using API.Data.Models;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using API.DataBase;
using Microsoft.AspNetCore.Mvc;
using API.Data.Services;

namespace API.Data.WritersPDF
{
    public class RelatorioDeMatricula : WritersPDF.Writer
    {
        public RelatorioDeMatricula(IWebHostEnvironment webHostEnvironment, DBContext dbContext) : base(webHostEnvironment, dbContext)
        {

        }

        // Função para gerar o Relatório de Matrícula em pdf
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
            var pasta = Path.Combine(base.webHostEnvironment.ContentRootPath, "DataBase/Files/Relatorios");
            base.nome = "relatório de matrícula " + aluno.nome + " (ID-" + idAluno + ").pdf";
            base.caminho = Path.Combine(pasta, base.nome);

            // Criando ou abrindo o arquivo
            FileStream arquivo;
            if (File.Exists(base.caminho))
                File.Delete(base.caminho);
            //arquivo = new FileStream(base.caminho, FileMode.Open, FileAccess.Write);
            //else
            arquivo = new FileStream(base.caminho, FileMode.Create, FileAccess.Write);

            // Preparando para escrever no arquivo
            var writer = PdfWriter.GetInstance(pdf, arquivo);
            base.pdf.Open();

            // Adicionando o PageEvent para o Writer
            writer.PageEvent = new Eventos(this);

            // Escrevendo o título
            var titulo = new Paragraph("Relatório de Matrícula - [Nome da Universidade]", base.fonteH1);
            titulo.Alignment = Element.ALIGN_LEFT;
            base.pdf.Add(titulo);

            // Aréa de informações do aluno
            var alunoInfos = new Paragraph("\n\nInformações pessoais do Aluno", base.fonteH2);
            alunoInfos.Alignment = Element.ALIGN_LEFT;
            base.pdf.Add(alunoInfos);

            var infos = new Paragraph("\nNome: ", base.fonteP1bold);
            infos.Font = base.fonteP1;
            infos.Add(new Phrase(aluno.nome));

            infos.Font = base.fonteP1bold;
            infos.Add(new Phrase("\nCadastro de Pessoa Física (CPF): "));
            infos.Font = base.fonteP1;
            infos.Add(new Phrase(aluno.cpf.ToString()));

            infos.Font = base.fonteP1bold;
            infos.Add(new Phrase("\nRegistro Geral (RG): "));
            infos.Font = base.fonteP1;
            infos.Add(new Phrase(aluno.rg.ToString()));

            infos.Font = base.fonteP1bold;
            infos.Add(new Phrase("\nRegistro Acadêmico (RA): "));
            infos.Font = base.fonteP1;
            infos.Add(new Phrase(idAluno.ToString()));

            infos.Alignment = Element.ALIGN_LEFT;
            base.pdf.Add(infos);

            // Desenha uma Linha horizontal
            var line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.Black, Element.ALIGN_LEFT, 1)));
            base.pdf.Add(line);

            // Defininfo variáveis para utilização futura
            int trancados = 0;
            int finalizados = 0;
            int? menorFrequencia = null;
            int? maiorFrequencia = null;
            float mediaTotal = 0f; var countMediaTotal = 0;
            int faltasTotais = 0;
            float frequenciaMediaTotal = 0; var countFrequenciaMediaTotal = 0;
            int cursando = 0;
            int aprovado = 0;
            int reprovado = 0;

            // Adicionando cada curso do aluno
            foreach (var curso in cursos)
            {
                // Verificando as informações das disciplinas
                // Definido as variáveis
                var numeroFaltas = 0;
                var notaMedia = 0f;
                var countNotaMedia = 0;
                var frequenciaMedia = 0;
                var countFrequenciaMedia = 0;
                if (curso.disciplinas != null && curso.disciplinas.Count > 0)
                {
                    // Iniciando o loop para verificar cada disciplina
                    foreach (var disciplina in curso.disciplinas)
                    {
                        if (disciplina.disciplinaCursada != null)
                        {
                            // Juntando as medias
                            if (disciplina.disciplinaCursada.media != null)
                            {
                                notaMedia += (float)disciplina.disciplinaCursada.media;
                                mediaTotal += (float)disciplina.disciplinaCursada.media;
                                ++countNotaMedia;
                                ++countMediaTotal;
                            }
                            // Juntando as faltas
                            if (disciplina.disciplinaCursada.faltas != null)
                            {
                                numeroFaltas += (int)disciplina.disciplinaCursada.faltas;
                                faltasTotais += (int)disciplina.disciplinaCursada.faltas;
                            }
                            // Juntando as frequências
                            if (disciplina.disciplinaCursada.frequencia != null)
                            {
                                frequenciaMedia += (int)disciplina.disciplinaCursada.frequencia;
                                frequenciaMediaTotal += (int)disciplina.disciplinaCursada.frequencia;
                                ++countFrequenciaMedia;
                                ++countFrequenciaMediaTotal;

                                // Juntando info para as estatísticas gerais
                                if (menorFrequencia == null || disciplina.disciplinaCursada.frequencia < menorFrequencia)
                                    menorFrequencia = disciplina.disciplinaCursada.frequencia;
                                if (maiorFrequencia == null || disciplina.disciplinaCursada.frequencia > maiorFrequencia)
                                    maiorFrequencia = disciplina.disciplinaCursada.frequencia;
                            }

                            // Verificando se a situação do aluno na disciplina
                            switch (disciplina.disciplinaCursada.situacao)
                            {
                                case Situacao.Cursando:
                                    ++cursando;
                                    break;
                                case Situacao.Aprovado:
                                    ++aprovado;
                                    break;
                                case Situacao.Reprovado:
                                    ++reprovado;
                                    break;
                            }
                        }
                    }
                    // Fazendo os cálculos
                    if (notaMedia != 0f) notaMedia /= countNotaMedia;
                    if (frequenciaMedia != 0) frequenciaMedia /= countFrequenciaMedia;
                }

                // Juntando info para as estatísticas gerais
                if (curso.cursoMatriculado != null)
                {
                    if (curso.cursoMatriculado.trancado) ++trancados;
                    if (curso.cursoMatriculado.finalizado) ++finalizados;
                }

                // Seção de informações de cada curso
                var cursoT = new Paragraph("\nInformações do curso", base.fonteH2);
                cursoT.Alignment = Element.ALIGN_LEFT;
                base.pdf.Add(cursoT);

                // Informações do curso
                var cursoP = new Paragraph("Nome: ", base.fonteP1bold);
                cursoP.Font = base.fonteP1;
                cursoP.Add(new Phrase(curso.curso.nome));
                cursoP.Font = base.fonteP1bold;
                cursoP.Add(new Phrase("\nID do Curso: "));
                cursoP.Font = base.fonteP1;
                cursoP.Add(new Phrase(curso.cursoMatriculado.idCurso.ToString()));
                cursoP.Font = base.fonteP1bold;
                cursoP.Add(new Phrase("\nID da Turma: "));
                cursoP.Font = base.fonteP1;
                cursoP.Add(new Phrase(curso.cursoMatriculado.idTurma.ToString()));

                // Estatísticas do curso em específico
                cursoP.Font = base.fonteP1bold;
                cursoP.Add(new Phrase("\nNota média do aluno no curso: "));
                cursoP.Font = base.fonteP1;
                cursoP.Add(new Phrase(base.TraduzirZero((float)notaMedia)));
                cursoP.Font = base.fonteP1bold;
                cursoP.Add(new Phrase("\nNúmero de faltas do aluno no curso: "));
                cursoP.Font = base.fonteP1;
                cursoP.Add(new Phrase(base.TraduzirZero((float)numeroFaltas)));
                cursoP.Font = base.fonteP1bold;
                cursoP.Add(new Phrase("\nFrequência média do aluno no curso: "));
                cursoP.Font = base.fonteP1;
                cursoP.Add(new Phrase(base.TraduzirZero((float)frequenciaMedia)));

                cursoP.Alignment = Element.ALIGN_LEFT;
                base.pdf.Add(cursoP);
            }

            // Últimos cálculos
            if (mediaTotal != 0f) mediaTotal /= countMediaTotal;
            if (frequenciaMediaTotal != 0) frequenciaMediaTotal /= countFrequenciaMediaTotal;

            // Desenha uma Linha horizontal
            var linha = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.Black, Element.ALIGN_LEFT, 1)));
            base.pdf.Add(linha);

            // Seção de estatísticas gerais
            var geralT = new Paragraph("\nInformações Gerais", base.fonteH2);
            geralT.Alignment = Element.ALIGN_LEFT;
            base.pdf.Add(geralT);

            var geralP = new Paragraph("Número de cursos trancados: ", base.fonteP1bold);
            geralP.Font = base.fonteP1;
            geralP.Add(new Phrase(base.TraduzirZero((float)trancados)));
            geralP.Font = base.fonteP1bold;
            geralP.Add(new Phrase("\nNúmero de cursos finalizados: "));
            geralP.Font = base.fonteP1;
            geralP.Add(new Phrase(base.TraduzirZero((float)finalizados)));
            geralP.Font = base.fonteP1bold;
            geralP.Add(new Phrase("\nDisciplina com a MENOR frequência: "));
            geralP.Font = base.fonteP1;
            geralP.Add(new Phrase(base.TraduzirZero((float)base.TraduzirNull(menorFrequencia))));
            geralP.Font = base.fonteP1bold;
            geralP.Add(new Phrase("\nDisciplina com a MAIOR frequência: "));
            geralP.Font = base.fonteP1;
            geralP.Add(new Phrase(base.TraduzirZero((float)base.TraduzirNull(maiorFrequencia))));
            geralP.Font = base.fonteP1bold;
            geralP.Add(new Phrase("\nMedia geral (nota): "));
            geralP.Font = base.fonteP1;
            geralP.Add(new Phrase(base.TraduzirZero((float)mediaTotal)));
            geralP.Font = base.fonteP1bold;
            geralP.Add(new Phrase("\nNúmero total de faltas: "));
            geralP.Font = base.fonteP1;
            geralP.Add(new Phrase(base.TraduzirZero((float)faltasTotais)));
            geralP.Font = base.fonteP1bold;
            geralP.Add(new Phrase("\nFrequência geral: "));
            geralP.Font = base.fonteP1;
            geralP.Add(new Phrase(base.TraduzirZero((float)frequenciaMediaTotal)));
            geralP.Font = base.fonteP1bold;
            geralP.Add(new Phrase("\nNúmero de disciplinas na situação CURSANDO: "));
            geralP.Font = base.fonteP1;
            geralP.Add(new Phrase(base.TraduzirZero((float)cursando)));
            geralP.Font = base.fonteP1bold;
            geralP.Add(new Phrase("\nNúmero de disciplinas na situação APROVADO: "));
            geralP.Font = base.fonteP1;
            geralP.Add(new Phrase(base.TraduzirZero((float)aprovado)));
            geralP.Font = base.fonteP1bold;
            geralP.Add(new Phrase("\nNúmero de disciplinas na situação REPROVADO: "));
            geralP.Font = base.fonteP1;
            geralP.Add(new Phrase(base.TraduzirZero((float)reprovado)));
            base.pdf.Add(geralP);

            // Fechando o arquivo
            pdf.Close();
            arquivo.Close();
        }
    }
}
