using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Atividade_SAEP_3.Models
{
    
    public class Consulta
    {
        [Key]
        public int Id { get; set; }
        public int pacienteId { get; set; }
        public int medicoId { get; set; }
        public DateTime DataHora { get; set; }
        public string statusAtendimento { get; set; }

        [ForeignKey("pacienteId")]
        public Paciente? paciente { get; set; }
        
        [ForeignKey("medicoId")]
        public Medico? medico { get; set; }
    }
}