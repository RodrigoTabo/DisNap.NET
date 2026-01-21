using DisnApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisnApp.Configuracion
{
    public class ComentarioConfig : IEntityTypeConfiguration<Comentario>
    {
        public void Configure(EntityTypeBuilder<Comentario> b)
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Contenido).HasMaxLength(1000).IsRequired();

            b.HasIndex(x => new { x.PublicacionId, x.FechaComentario });

            b.HasOne(x => x.Usuario)
                .WithMany(u => u.Comentarios)
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Publicacion)
                .WithMany(p => p.Comentarios)
                .HasForeignKey(x => x.PublicacionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
