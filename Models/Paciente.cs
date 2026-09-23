using System.ComponentModel.DataAnnotations;
namespace Atividade_SAEP_3.Models
{
    public class Paciente
    {
        [Key]
        public int Id { get; set; }

        public string Nome { get; set; }

        public string CPF { get; set; }

        public int telefone { get; set; }
        public DateOnly dataNascimento { get; set; }
    }
}