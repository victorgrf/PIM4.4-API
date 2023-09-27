using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using API.Data.ViewModels;
using System;

namespace API.Data.Models
{
    [Table("disciplinacursada")]
    public class DisciplinaCursada
    {
        [Key]
        public int id { get; set; }
        public int idDisciplina { get; set; }
        public int idCursoMatriculado { get; set; }
        public float? prova1 { get; set; }
        public float? prova2 { get; set; }
        public float? trabalho { get; set; }
        public float? media { get; set; }
        public int? faltas { get; set; }
        public int? frequencia { get; set; }
        public string situacao { get; set; }
    }
}
