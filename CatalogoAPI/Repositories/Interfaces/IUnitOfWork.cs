using System.Threading.Tasks;
namespace CatalogoAPI.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
         IProdutoRepository ProdutoRepository { get; }
         ICategoriaRepository CategoriaRepository { get; }

         Task Commit();
    }
}