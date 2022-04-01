using Microsoft.EntityFrameworkCore;
using CatalogoAPI.Models;

namespace CatalogoAPI.Context
{
    public class CatalogoDbContext : DbContext  
    {

        public CatalogoDbContext(DbContextOptions<CatalogoDbContext> opt) : base(opt) {}
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        
    }
}