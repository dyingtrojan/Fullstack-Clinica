using System.ComponentModel.DataAnnotations;

namespace Atividade_SAEP_3.Models
{   
    public class Medico
    {
        [Key]
        public int Id { get; set; }
        public string nome { get; set; }
        public string crm { get; set; }
        public string especialidade { get; set; }
    }
}