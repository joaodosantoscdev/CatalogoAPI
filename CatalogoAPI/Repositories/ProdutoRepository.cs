using System.Collections.Generic;
using System.Linq;
using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Repositories.Interfaces;

namespace CatalogoAPI.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {

        public ProdutoRepository(CatalogoDbContext context) : base(context)
        {
            
        }
        public IEnumerable<Produto> GetProdutosPorPreco() 
        {
            return Get().OrderBy(p => p.Preco).ToList();
        }
    }
}