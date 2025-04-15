
using EclipseWorksChallange.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EclipseWorksChallange.Infraestructure.Persistence
{
    public class EclipseWorksSQLServerContext : DbContext
    {
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Projeto> Projeto { get; set; }
        public virtual DbSet<Tarefa> Tarefa { get; set; }
        public virtual DbSet<HistoricoTarefa> HistoricoTarefa { get; set; }
        public virtual DbSet<NiveisUsuario> NiveisUsuario { get; set; }

        public EclipseWorksSQLServerContext(DbContextOptions<EclipseWorksSQLServerContext> options) : base(options)
        {

        }

        public EclipseWorksSQLServerContext()
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Projeto>()
                .HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tarefa>()
                .HasOne(t => t.Projeto)
                .WithMany(p => p.Tarefas)
                .HasForeignKey(t => t.ProjetoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tarefa>()
                .HasOne(t => t.UsuarioResponsavel)
                .WithMany()
                .HasForeignKey(t => t.UsuarioResponsavelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HistoricoTarefa>()
                .HasOne(h => h.Tarefa)
                .WithMany(t => t.Historicos)
                .HasForeignKey(h => h.TarefaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HistoricoTarefa>()
                 .HasOne(h => h.Usuario)
                 .WithMany()
                 .HasForeignKey(h => h.UsuarioId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasOne(h => h.Nivel)
                .WithMany()
                .HasForeignKey(h => h.NivelID);
        }
    }
}
