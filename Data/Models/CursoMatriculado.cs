using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using API.Data.ViewModels;
using System;

namespace API.Data.Models
{
    [Table("cursomatriculado")]
    public class CursoMatriculado
    {
        [Key]
        public int id { get; set; } 
        public int idAluno { get; set; }
        public int idCurso { get; set; }
        public int idTurma { get; set; }
        public int semestreAtual { get; set; }
        public bool trancado { get; set; }
        public bool finalizado { get; set; }
    }
}
