using System.Collections.Generic;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using System.Threading.Tasks;

namespace CatalogoAPI.Repositories.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<IEnumerable<Categoria>> GetCategoriasProdutos();

        Task<PagedList<Categoria>> GetCategorias(CategoriasParameters categoriasParameters);

    }
}