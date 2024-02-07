using System.Data.Entity;

namespace SistemaInventario.Models
{
    public class SistemaInventarioDbContext : ApplicationDbContext
    {
        public DbSet<Transacciones> Transacciones { get; set; }
        public DbSet<Almacenes> Almacenes { get; set; }
        public DbSet<ExistenciasXAlmacenes> ExistenciaXAlmacenes { get; set; }
        public DbSet<Articulos> Articulos { get; set; }
        public DbSet<TiposInventarios> TipoInventarios { get; set; }

    }
}