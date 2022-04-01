using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CatalogoAPI.Context;
using CatalogoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MimicAPI.V1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasController : ControllerBase
    {
        // Constructor and Dependencies
        #region DI Injected
        private readonly CatalogoDbContext _context;
        private readonly ILogger _logger;
        public CategoriasController(CatalogoDbContext context, ILogger<CategoriasController> logger) 
        {
            _context = context;
            _logger = logger;
        }
        #endregion

        // GET Methods - Categorias Controller
        #region GET Methods
        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutosAsync()
        {
            try
            {
                // Example of ILogger interface usage for debugging purposes.
                // _logger.Log(LogLevel.Information, ">>>>>>>>>>>>>>>>>> ENTROU <<<<<<<<<<<<<<<<<<<<");

                return await _context.Categorias.Include(c => c.Produtos).ToListAsync();    
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                            $"Erro ao tentar buscar todas Categorias. Erro {e.Message}"); 
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAllAsync()
        {
            try
            {
                return await _context.Categorias.ToListAsync();    
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                            $"Erro ao tentar buscar todas Categorias. Erro {e.Message}"); 
            }
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Categorias.FirstOrDefaultAsync(c => c.CategoriaId == id); 
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                            $"Erro ao tentar buscar Categoria. Erro {e.Message}"); 
            }
        }
        #endregion

        // POST Methods - Categorias Controller
        #region POST Methods
        [HttpPost]
        public ActionResult Post([FromBody]Categoria categoria)
        {
            try
            {
                var produtoCriado = _context.Categorias.Add(categoria);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                            $"Erro ao tentar adicionar Categoria. Erro {e.Message}"); 
            }
        }
        #endregion

        // UPDATE Methods - Categorias Controller
        #region PUT Methods 
        [HttpPut("{id}")]
        public ActionResult Update(int id, Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId) return BadRequest();

                var categoriaAtualizada = _context.Categorias.Update(categoria);
                _context.SaveChanges();

                return Ok(new { message = "Atualizado" });
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                            $"Erro ao tentar atualizar Categoria. Erro {e.Message}"); 
            }
        }
        #endregion

        // DELETE Methods - Categorias Controller
        #region DELETE Methods
        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            try
            {
                var categoriaFiltrada = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);
                if (categoriaFiltrada == null) return null;

                _context.Remove(categoriaFiltrada);
                _context.SaveChanges();

                return Ok(new { message = "Deletada"});   
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                            $"Erro ao tentar deletar Categoria. Erro {e.Message}");            
            }
        }
        #endregion
    
    }
}