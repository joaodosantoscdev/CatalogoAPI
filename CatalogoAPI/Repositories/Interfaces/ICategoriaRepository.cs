using System.Collections.Generic;
using CatalogoAPI.Models;

namespace CatalogoAPI.Repositories.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        IEnumerable<Categoria> GetCategoriasProdutos();
    }
}