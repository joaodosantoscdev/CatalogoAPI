using System.Collections.Generic;
using System.Linq;
using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Repositories.Interfaces;
using CatalogoAPI.Pagination;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CatalogoAPI.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {

        public ProdutoRepository(CatalogoDbContext context) : base(context)
        {
            
        }

         public async Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParameters) 
        {

            /* return Get()
                .OrderBy(p => p.Nome)
                .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
                .Take(produtosParameters.PageSize)
                .ToList(); */

                return await PagedList<Produto>.ToPagedList(Get().OrderBy(p => p.ProdutoId), 
                                                      produtosParameters.PageNumber, 
                                                      produtosParameters.PageSize);
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorPreco() 
        {
            return await Get().OrderBy(p => p.Preco).ToListAsync();
        }

    }
}