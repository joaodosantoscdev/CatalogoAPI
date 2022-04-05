using System.Collections.Generic;
using CatalogoAPI.Models;

namespace CatalogoAPI.Repositories.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosPorPreco();
    }
}