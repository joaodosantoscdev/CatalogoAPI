using CatalogoAPI.Context;
using CatalogoAPI.Repositories.Interfaces;
using CatalogoAPI.Repositories;

namespace CatalogoAPI.Repositories
{
  public class UnitOfWork : IUnitOfWork
  {
      private ProdutoRepository _produtoRepository;
      private CategoriaRepository _categoriaRepository;
      public CatalogoDbContext _context;

      public UnitOfWork(CatalogoDbContext context)
      {        
          _context = context;
      }
      
      
    public IProdutoRepository ProdutoRepository 
    {
        get 
        {
            return _produtoRepository = _produtoRepository ?? new ProdutoRepository(_context);
        }
    }

        public ICategoriaRepository CategoriaRepository 
    {
        get 
        {
            return _categoriaRepository = _categoriaRepository ?? new CategoriaRepository(_context);
        }
    }

    public void Commit()
    {
      _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();    
    }
  }
}