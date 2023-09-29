using iTextSharp.text;
using iTextSharp.text.pdf;
using API.Data.ViewModels;
using API.Data.Models;
using Microsoft.AspNetCore.Hosting;
using API.DataBase;

namespace API.Data.WritersPDF
{
    public class Writer
    {
        protected float pixelPorMilimetro;
        protected IWebHostEnvironment webHostEnvironment;
        protected BaseFont fontePadrao;
        protected Font fonteH1;
        protected Font fonteH2;
        protected Font fonteP1;
        protected Font fonteP1bold;
        protected Font fonteP1italic;
        protected Font fonteP2;
        protected Font fonteP2bold;
        protected Font fonteFooter;
        protected Document pdf;
        protected DBContext dbContext;
        protected string? caminho;
        protected string? nome;

        public Writer(IWebHostEnvironment webHostEnvironment, DBContext dbContext)
        {
            this.pixelPorMilimetro = 72 / 25.2f;
            this.webHostEnvironment = webHostEnvironment;
            this.fontePadrao = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            this.fonteH1 = new Font(this.fontePadrao, 25, Font.BOLD, BaseColor.Black);
            this.fonteH2 = new Font(this.fontePadrao, 18, Font.BOLD, BaseColor.Black);
            this.fonteP1 = new Font(this.fontePadrao, 14, Font.NORMAL, BaseColor.Black);
            this.fonteP1bold = new Font(this.fontePadrao, 14, Font.BOLD, BaseColor.Black);
            this.fonteP1italic = new Font(this.fontePadrao, 14, Font.ITALIC, BaseColor.Black);
            this.fonteP2 = new Font(this.fontePadrao, 10, Font.NORMAL, BaseColor.Black);
            this.fonteP2bold = new Font(this.fontePadrao, 10, Font.BOLD, BaseColor.Black);
            this.fonteFooter = new Font(this.fontePadrao, 12, Font.NORMAL, BaseColor.Black);
            this.pdf = new Document(PageSize.A4,
                15 * this.pixelPorMilimetro,
                15 * this.pixelPorMilimetro,
                15 * this.pixelPorMilimetro,
                20 * this.pixelPorMilimetro);
            this.dbContext = dbContext;
        }

        public Font GetFontFooter()
        {
            return this.fonteFooter;
        }

        public string? GetCaminho()
        {
            return this.caminho;
        }

        public string? GetNome()
        {
            return this.nome;
        }

        public void MudarMargins(float marginLeft, float marginRight, float marginTop, float marginBottom)
        {
            this.pdf.SetMargins(marginLeft, marginRight, marginTop, marginBottom);
        }

        protected string TraduzirBool(bool x)
        {
            if (x)
            {
                return "sim";
            }
            return "não";
        }
        protected string TraduzirInt(int? x)
        {
            if (x == null)
            {
                return "--";
            }
            return x.ToString();
        }

        protected string TraduzirFloat(float? x)
        {
            if (x == null)
            {
                return "--";
            }
            return x.ToString();
        }

        protected string TraduzirZero(float x)
        {
            if (x == 0f)
            {
                return "--";
            }
            return x.ToString();
        }

        protected float TraduzirNull(float? x)
        {
            if (x == null)
            {
                return 0f;
            }
            return (float)x;
        }

        protected void CriarCelula(PdfPTable tabela, string nome, int align)
        {
            var cor = new BaseColor(0.95f, 0.95f, 0.95f);
            if (tabela.Rows.Count % 2 == 1)
            {
                cor = BaseColor.White;
            }

            var cell = new PdfPCell(new Phrase(nome, this.fonteP2));
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Border = 0;
            cell.BorderWidthBottom = 1;
            cell.FixedHeight = 25;
            cell.PaddingBottom = 5;
            cell.BackgroundColor = cor;
            tabela.AddCell(cell);
        }
    }
    public class Disciplina
    {
        public Models.Disciplina? disciplina { get; private set; }
        public Models.DisciplinaCursada? disciplinaCursada { get; private set; }

        public Disciplina(Models.Disciplina? disciplina, Models.DisciplinaCursada? disciplinaCursada)
        {
            this.disciplina = disciplina;
            this.disciplinaCursada = disciplinaCursada;
        }
    }

    public class Cursos
    {
        public Models.CursoMatriculado? cursoMatriculado { get; private set; }
        public Models.Curso? curso { get; private set; }
        public List<Disciplina>? disciplinas { get; private set; }

        public Cursos(Models.CursoMatriculado? cursoMatriculado, Models.Curso? curso, List<Disciplina>? disciplinas)
        {
            this.cursoMatriculado = cursoMatriculado;
            this.curso = curso;
            this.disciplinas = disciplinas;
        }
    }

    public class Eventos : PdfPageEventHelper
    {
        public Writer writer { get; private set; }

        public Eventos(Writer writer)
        {
            this.writer = writer;
        }

        public override void OnOpenDocument(PdfWriter pdfWriter, Document documento)
        {
            base.OnOpenDocument(pdfWriter, documento);
        }

        public override void OnEndPage(PdfWriter pdfWriter, Document documento)
        {
            base.OnEndPage(pdfWriter, documento);

            var momento = "Documento gerado em: " + DateTime.Now.ToShortDateString() + " às " + DateTime.Now.ToShortTimeString() + ".";
            pdfWriter.DirectContent.BeginText();
            pdfWriter.DirectContent.SetFontAndSize(this.writer.GetFontFooter().BaseFont, this.writer.GetFontFooter().Size);
            pdfWriter.DirectContent.SetTextMatrix(documento.LeftMargin, documento.BottomMargin * 0.75f);
            pdfWriter.DirectContent.ShowText(momento);
            pdfWriter.DirectContent.EndText();

            var pagAtual = pdfWriter.PageNumber;
            var pag = "Página: " + pagAtual;

            var largura = this.writer.GetFontFooter().BaseFont.GetWidthPoint(pag, this.writer.GetFontFooter().Size);
            var pagTamanho = documento.PageSize;
            pdfWriter.DirectContent.BeginText();
            pdfWriter.DirectContent.SetFontAndSize(this.writer.GetFontFooter().BaseFont, this.writer.GetFontFooter().Size);
            pdfWriter.DirectContent.SetTextMatrix(pagTamanho.Width - documento.RightMargin - largura, documento.BottomMargin * 0.75f);
            pdfWriter.DirectContent.ShowText(pag);
            pdfWriter.DirectContent.EndText();
        }
    }
}
