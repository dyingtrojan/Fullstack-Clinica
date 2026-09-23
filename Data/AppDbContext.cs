
using Microsoft.EntityFrameworkCore;
using Atividade_SAEP_3.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
    {
    }

public DbSet<Atividade_SAEP_3.Models.Paciente> Paciente { get; set; } = default!;

public DbSet<Atividade_SAEP_3.Models.Medico> Medico { get; set; } = default!;

public DbSet<Atividade_SAEP_3.Models.Consulta> Consulta { get; set; } = default!;
}
   
