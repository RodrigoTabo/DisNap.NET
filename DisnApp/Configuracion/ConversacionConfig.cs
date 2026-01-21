using DisnApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisnApp.Configuracion
{
    public class ConversacionConfig : IEntityTypeConfiguration<Conversacion>
    {
        public void Configure(EntityTypeBuilder<Conversacion> b)
        {
            b.HasKey(x => x.Id);

            b.HasIndex(x => x.UltimaActividad);
            // ordenar inbox por ultima actividad
        }
    }
}
