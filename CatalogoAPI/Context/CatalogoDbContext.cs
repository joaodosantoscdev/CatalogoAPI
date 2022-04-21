using Microsoft.EntityFrameworkCore;
using CatalogoAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CatalogoAPI.Context
{
    public class CatalogoDbContext : IdentityDbContext  
    {

        public CatalogoDbContext(DbContextOptions<CatalogoDbContext> opt) : base(opt) {}
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        
    }
}