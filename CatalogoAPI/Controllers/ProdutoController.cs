using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CatalogoAPI.Models;
using CatalogoAPI.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MimicAPI.V1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController : ControllerBase
    {
        // Constructor and Dependencies
        #region DI Injected
        private readonly CatalogoDbContext _context;
        public ProdutosController(CatalogoDbContext context) 
        {
            _context = context;
        }
        #endregion

        // GET Methods - Produtos Controller
        #region GET Methods
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAllAsync()
        {
            try 
            {
                return await _context.Produtos.AsNoTracking().ToListAsync();
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                            $"Erro ao tentar buscar Produtos. Erro {e.Message}");
            } 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetByIdAsync(int id)
        {
            try 
            {
                var produtoFiltrado = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);
                if (produtoFiltrado == null) return NotFound();
            
                return Ok(produtoFiltrado);
            }
            catch (Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                            $"Erro ao tentar buscar por ID o Produto. Erro {e.Message}");
            }
        }
        #endregion

        // POST Methods - Produtos Controller
        #region POST Methods
        [HttpPost]
        public ActionResult Post([FromBody]Produto produto)
        {
            try 
            {
                var produtoCriado = _context.Produtos.Add(produto);
                _context.SaveChanges();

                return Ok(produto);
            } 
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                            $"Erro ao tentar criar Produto. Erro {e.Message}"); 
            }
           
            // --> Can be done returning a statuscode created while pointing for the [getbyid] method <--
            //return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }
        #endregion

        // UPDATE Methods - Produtos Controller
        #region PUT Methods
        [HttpPut("{id}")]
        public ActionResult Update(int id, Produto produto)
        { 
            try 
            {
                if(id != produto.ProdutoId) return BadRequest();

                var produtoAtualizado = _context.Produtos.Update(produto);
                _context.SaveChanges();

                return Ok(produtoAtualizado);
            } 
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                            $"Erro ao tentar atualizar Produto já existente. Erro {e.Message}");
            }
        }
        #endregion

        // DELETE Methods - Produto Controller
        #region DELETE Methods
        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            try 
            {            
                var produtoFiltrado = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
                 /* The Find(); method works too, searching first on memory,
                  but only if ID is a primary key */

                 // var produtoFiltrado = _context.Produtos.Find(id);
                if (produtoFiltrado == null) return NotFound();

                _context.Produtos.Remove(produtoFiltrado);
                _context.SaveChanges();

                return Ok(new {message = "Deletado"});
            }
            catch(Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                            $"Erro ao tentar deletar Produto. Erro {e.Message}");
            }
        }
        #endregion
    
    }
}