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
using CatalogoAPI.Repositories.Interfaces;

namespace MimicAPI.V1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasController : ControllerBase
    {
        // Constructor and Dependencies
        #region DI Injected
        private readonly IUnitOfWork _uof;
        private readonly ILogger _logger;
        public CategoriasController(IUnitOfWork uof, ILogger<CategoriasController> logger) 
        {
            _uof = uof;
            _logger = logger;
        }
        #endregion

        // GET Methods - Categorias Controller
        #region GET Methods
        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutosAsync()
        {
            try
            {
                // Example of ILogger interface usage for debugging purposes.
                // _logger.Log(LogLevel.Information, ">>>>>>>>>>>>>>>>>> ENTROU <<<<<<<<<<<<<<<<<<<<");

                return _uof.CategoriaRepository.GetCategoriasProdutos().ToList();    
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                            $"Erro ao tentar buscar todas Categorias. Erro {e.Message}"); 
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> GetAllAsync()
        {
            try
            {
                return _uof.CategoriaRepository.Get().ToList();    
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                            $"Erro ao tentar buscar todas Categorias. Erro {e.Message}"); 
            }
            
        }

        [HttpGet("{id}")]
        public ActionResult<Categoria> GetByIdAsync(int id)
        {
            try
            {
                return _uof.CategoriaRepository.GetById(c => c.CategoriaId == id); 
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
                _uof.CategoriaRepository.Add(categoria);
                _uof.Commit();

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

                _uof.CategoriaRepository.Update(categoria);
                _uof.Commit();

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
                var categoriaFiltrada = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
                if (categoriaFiltrada == null) return null;

                _uof.CategoriaRepository.Delete(categoriaFiltrada);
                _uof.Commit();

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