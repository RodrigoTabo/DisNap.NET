using DisnApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisnApp.Configuracion
{
    public class MensajeConfig : IEntityTypeConfiguration<Mensaje>
    {
        public void Configure(EntityTypeBuilder<Mensaje> b)
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Texto).HasMaxLength(2000).IsRequired();
            // Tamaño razonable, y evita nulls.

            b.HasIndex(x => new { x.ConversacionId, x.FechaEnvio });
            // Para listar mensajes: WHERE ConversacionId = X ORDER BY FechaEnvio

            b.HasOne(x => x.Conversacion)
                .WithMany(c => c.Mensajes)
                .HasForeignKey(x => x.ConversacionId)
                .OnDelete(DeleteBehavior.Cascade);
           

            b.HasOne(x => x.Emisor)
                .WithMany(u => u.MensajesEnviados)
                .HasForeignKey(x => x.EmisorId)
                .OnDelete(DeleteBehavior.Restrict);
            // Evita cascadas dobles con Usuario
        }
    }
}
