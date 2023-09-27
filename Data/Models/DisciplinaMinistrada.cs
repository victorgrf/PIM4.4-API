using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using API.Data.ViewModels;
using System;

namespace API.Data.Models
{
    [Table("disciplinaministrada")]
    public class DisciplinaMinistrada
    {
        [Key] public int id { get; set; }
        public int idDisciplina { get ; set; } 
        public int idTurma { get ; set; }      
        public int idProfessor { get ; set; } 
        public bool encerrada { get; set; }
        public bool coordenador { get ; set; }
    }
}
