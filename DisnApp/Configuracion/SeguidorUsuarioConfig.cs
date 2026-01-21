using DisnApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisnApp.Configuracion
{
    public class SeguidorUsuarioConfig : IEntityTypeConfiguration<SeguidorUsuario>
    {
        public void Configure(EntityTypeBuilder<SeguidorUsuario> b)
        {
            b.HasKey(x => x.Id);

            b.HasIndex(x => new { x.SeguidorId, x.SeguidoId }).IsUnique();

            b.HasOne(x => x.Seguidor)
                .WithMany(u => u.Siguiendo)
                .HasForeignKey(x => x.SeguidorId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Seguido)
                .WithMany(u => u.Seguidores)
                .HasForeignKey(x => x.SeguidoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
