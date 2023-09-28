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
        protected Font fonteP2;
        protected Font fonteP2bold;
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
            this.fonteP2 = new Font(this.fontePadrao, 10, Font.NORMAL, BaseColor.Black);
            this.fonteP2bold = new Font(this.fontePadrao, 10, Font.BOLD, BaseColor.Black);
            this.pdf = new Document(PageSize.A4,
                15 * this.pixelPorMilimetro,
                15 * this.pixelPorMilimetro,
                15 * this.pixelPorMilimetro,
                15 * this.pixelPorMilimetro);
            this.dbContext = dbContext;
        }

        public string? GetCaminho()
        {
            return this.caminho;
        }

        public string? GetNome()
        {
            return this.nome;
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
}
