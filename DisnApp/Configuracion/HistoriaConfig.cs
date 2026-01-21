using DisnApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisnApp.Configuracion
{
    public class HistoriaConfig : IEntityTypeConfiguration<Historia>
    {
        public void Configure(EntityTypeBuilder<Historia> b)
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.UrlImagen).HasMaxLength(500).IsRequired();

            b.HasIndex(x => new { x.UsuarioId, x.FechaExpiracion });

            b.HasOne(x => x.Usuario)
                .WithMany()
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
