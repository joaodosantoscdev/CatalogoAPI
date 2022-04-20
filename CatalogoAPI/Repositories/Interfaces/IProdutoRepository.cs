using System.Collections.Generic;
using System.Threading.Tasks;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;

namespace CatalogoAPI.Repositories.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutosPorPreco();

        Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParameters);
    }
}