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
    public class Declaracao : WritersPDF.Writer
    {
        public Declaracao(IWebHostEnvironment webHostEnvironment, DBContext dbContext) : base(webHostEnvironment, dbContext)
        {

        }

        // Função para gerar o boletim em pdf
        public void Gerar(int idAluno)
        {
            // Buscando dados no banco de dados
            var aluno = base.dbContext.Alunos.FirstOrDefault(n => n.id == idAluno);
            var cursoMatriculados = base.dbContext.CursoMatriculados.Where(n => n.idAluno == idAluno).ToList();
            var cursos = new List<Models.Curso>();
            if (cursoMatriculados.Count > 0)
                foreach (var cm in cursoMatriculados)
                {
                    var curso = base.dbContext.Cursos.FirstOrDefault(n => n.id == cm.idCurso);
                    if (curso != null) cursos.Add(curso);
                }

            // Definindo o caminho do arquivo
            var pasta = Path.Combine(base.webHostEnvironment.ContentRootPath, "DataBase/Files/Declaracoes");
            base.nome = "declaração escolar " + aluno.nome + " (ID-" + idAluno + ").pdf";
            base.caminho = Path.Combine(pasta, base.nome);

            // Criando ou abrindo o arquivo
            FileStream arquivo;
            if (File.Exists(base.caminho))
                File.Delete(base.caminho);
            arquivo = new FileStream(base.caminho, FileMode.Create, FileAccess.Write);

            // Preparando para escrever no arquivo
            var writer = PdfWriter.GetInstance(base.pdf, arquivo);
            base.MudarMargins(80, 80, 15, 15);
            base.pdf.Open();

            // Adicionando o PageEvent para o Writer
            //writer.PageEvent = new Eventos(this);

            // Escrevendo o título
            var t1 = new Paragraph("[Nome da Universidade]\n\n", base.fonteH2);
            t1.Alignment = Element.ALIGN_CENTER;
            base.pdf.Add(t1);

            // Escrevendo o título
            var t2 = new Paragraph("Declaração\n\n\n\n", base.fonteH2);
            t2.Alignment = Element.ALIGN_CENTER;
            base.pdf.Add(t2);

            // Adicionando informacões do aluno
            var texto = new Paragraph("", base.fonteP1);
            texto.Add(new Phrase("A Secretaria da Universidade [Nome da Universidade] atesta que "));
            texto.Font = base.fonteP1italic;
            texto.Add(new Phrase(aluno.nome));
            texto.Font = base.fonteP1;
            texto.Add(new Phrase(", carteira de identidade (RG) "));
            texto.Font = base.fonteP1italic;
            texto.Add(new Phrase(aluno.rg.ToString()));
            texto.Font = base.fonteP1;
            texto.Add(new Phrase(", com o Registro Acadêmico (RA) "));
            texto.Font = base.fonteP1italic;
            texto.Add(new Phrase(idAluno.ToString()));
            texto.Font = base.fonteP1;
            texto.Add(new Phrase(", matriculado(a) no cursos cursos citados abaixo, encontra-se regularmente inscrito(a) no período letivo desta instituição de ensino superior. Esta declaração é válida para os fins que se fizerem necessários."));

            texto.Alignment = Element.ALIGN_JUSTIFIED;
            texto.SpacingAfter = 20;
            texto.SpacingBefore = 20;
            base.pdf.Add(texto);

            // Adicionando a área de cursos
            var cursosP = new Paragraph("\nCurso(s) Matriculado(s): ", base.fonteP1);
            cursosP.Font = base.fonteP1italic;

            // Adicionando cada curso do aluno
            for (var i = 0; i < cursos.Count; i++)
            {
                if (i > 0) cursosP.Add(new Phrase(", "));
                cursosP.Add(new Phrase(cursos[i].nome));
            }

            cursosP.Alignment = Element.ALIGN_LEFT;
            base.pdf.Add(cursosP);

            // Adicionando data de emissão
            var dataEmissao = new Paragraph("\n\nData de Emissão: ", base.fonteP1);
            dataEmissao.Add(new Phrase(DateTime.Now.ToShortDateString() + " às " + DateTime.Now.ToShortTimeString() + "\n\n\n"));
            dataEmissao.Alignment = Element.ALIGN_CENTER;
            base.pdf.Add(dataEmissao);

            // Desenha espaço para assinatura
            var linha = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(1.0F, 60.0F, BaseColor.Black, Element.ALIGN_CENTER, 1)));
            base.pdf.Add(linha);

            // Adicionando texto para assinatura
            var assinaturaTexto = new Paragraph("Assinatura da secretaria ou de um substituto legal.", base.fonteP1);
            assinaturaTexto.Alignment = Element.ALIGN_CENTER;
            base.pdf.Add(assinaturaTexto);

            // Fechando o arquivo
            pdf.Close();
            arquivo.Close();
        }
    }
}
