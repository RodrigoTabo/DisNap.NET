using DisnApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class HistoriaVistaConfig : IEntityTypeConfiguration<HistoriaVista>
{
    public void Configure(EntityTypeBuilder<HistoriaVista> b)
    {
        b.HasKey(x => x.Id);

        b.HasIndex(x => new { x.UsuarioId, x.HistoriaId }).IsUnique();

        b.HasOne(x => x.Usuario)
            .WithMany(u => u.HistoriaVista)
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Historia)
          .WithMany(h => h.HistoriaVistas)
          .HasForeignKey(x => x.HistoriaId)
          .OnDelete(DeleteBehavior.Cascade);
    }
}