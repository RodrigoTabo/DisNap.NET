using DisnApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DisnApp.Data
{
    public class RedDbContext : IdentityDbContext<Usuario>
    {
        public RedDbContext(DbContextOptions<RedDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RedDbContext).Assembly);
        }


        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Conversacion> Conversaciones { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<ConversacionUsuario> ConversacionUsuarios { get; set; }
        public DbSet<Historia> Historias { get; set; }
        public DbSet<HistoriaVista> HistoriaVistas { get; set; }
        public DbSet<Mensaje> Mensajes { get; set; }
        public DbSet<Publicacion> Publicaciones { get; set; }
        public DbSet<PublicacionLike> PublicacionLikes { get; set; }
        public DbSet<SeguidorUsuario> SeguidorUsuarios { get; set; }


    }

}



