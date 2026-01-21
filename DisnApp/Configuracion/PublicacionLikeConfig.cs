using DisnApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisnApp.Configuracion
{
    public class PublicacionLikeConfig : IEntityTypeConfiguration<PublicacionLike>
    {
        public void Configure(EntityTypeBuilder<PublicacionLike> b)
        {
            b.HasKey(x => x.Id);

            b.HasIndex(x => new { x.PublicacionId, x.UsuarioId }).IsUnique();

            b.HasOne(x => x.Usuario)
                .WithMany(u => u.Likes) // o .WithMany()
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Publicacion)
                .WithMany(p => p.Likes) // o .WithMany()
                .HasForeignKey(x => x.PublicacionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
