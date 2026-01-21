using DisnApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisnApp.Configuracion
{
    public class PublicacionConfig : IEntityTypeConfiguration<Publicacion>
    {
        public void Configure(EntityTypeBuilder<Publicacion> b)
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Descripcion).HasMaxLength(500).IsRequired();
            b.Property(x => x.UrlImagen).HasMaxLength(500).IsRequired();

            b.HasIndex(x => new { x.UsuarioId, x.FechaSubida });

            b.HasOne(x => x.Usuario)
                .WithMany(u => u.Publicaciones)
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
