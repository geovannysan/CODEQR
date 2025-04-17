using Entity;
using Microsoft.EntityFrameworkCore;

namespace NEWCODES.Infraestructura
{
    public class EevntoContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(basePath, @"..\..\..")); // sube 4 niveles
            string dbPath = Path.Combine(projectRoot, "Db", "CodeEvente.db");

            // optionsBuilder.UseSqlite($"Data Source={dbPath}");
            //  string basePath = AppDomain.CurrentDomain.BaseDirectory;
            //  string dbPath = Path.Combine("Db", "UserDb.db");
            //@"Data Source=D:\NET\NEWCODES\NEWCODES\Db\UserDb.db"
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
        public DbSet<Eventos> Eventos { get; set; }
        public DbSet<Localidades> Localidades { get; set; }
        public DbSet<Codigos> Codigos { get; set; }
        public DbSet<DispositivoLocation> DispositivoLocation { get; set; }
        public DbSet<Dispositivos> Dispositivos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dispositivos>()
                .HasOne(d => d.Eventos)
                .WithMany() // O podés usar `.WithMany(e => e.Dispositivos)` si querés navegación inversa
                 .HasForeignKey(d => d.EventoID)
                 .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DispositivoLocation>()
                .HasOne(d => d.Dispositivos)
                .WithMany()
                .HasForeignKey(d => d.DispoId);
            modelBuilder.Entity<Localidades>()
                .HasOne(d=>d.Eventos)
                .WithMany()
                .HasForeignKey(d=>d.IdEvento)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Codigos>()
                .HasOne(d=>d.Eventos)
                .WithMany()
                .HasForeignKey(d=>d.EventoID)
                .OnDelete(DeleteBehavior.Cascade);

                }


    }
}
