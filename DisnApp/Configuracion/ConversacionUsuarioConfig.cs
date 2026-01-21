using DisnApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisnApp.Configuracion
{
    public class ConversacionUsuarioConfig : IEntityTypeConfiguration<ConversacionUsuario>
    {
        public void Configure(EntityTypeBuilder<ConversacionUsuario> b)
        {
            b.HasKey(x => x.Id);

            b.HasIndex(x => new { x.ConversacionId, x.UsuarioId }).IsUnique();

            b.HasOne(x => x.Usuario)
                .WithMany(u => u.Conversaciones) 
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Conversacion)
                .WithMany(c => c.Participantes) 
                .HasForeignKey(x => x.ConversacionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
