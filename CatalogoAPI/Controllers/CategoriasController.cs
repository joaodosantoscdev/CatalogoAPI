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
using CatalogoAPI.DTOs;
using AutoMapper;

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
        private readonly IMapper _mapper;
        public CategoriasController(IUnitOfWork uof, ILogger<CategoriasController> logger, IMapper mapper) 
        {
            _uof = uof;
            _logger = logger;
            _mapper = mapper;
        }
        #endregion

        // GET Methods - Categorias Controller
        #region GET Methods
        [HttpGet("produtos")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasProdutosAsync()
        {
            try
            {
                // Example of ILogger interface usage for debugging purposes.
                // _logger.Log(LogLevel.Information, ">>>>>>>>>>>>>>>>>> ENTROU <<<<<<<<<<<<<<<<<<<<");

                var categoria = _uof.CategoriaRepository.GetCategoriasProdutos().ToList();
                var categoriaDTO = _mapper.Map<List<CategoriaDTO>>(categoria);

                return categoriaDTO;
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                            $"Erro ao tentar buscar todas Categorias. Erro {e.Message}"); 
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> GetAllAsync()
        {
            try
            {
                var categoria = _uof.CategoriaRepository.Get().ToList();
                var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categoria);

                return categoriasDTO;
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                            $"Erro ao tentar buscar todas Categorias. Erro {e.Message}"); 
            }
            
        }

        [HttpGet("{id}")]
        public ActionResult<CategoriaDTO> GetByIdAsync(int id)
        {
            try
            {
                var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
                if (categoria == null)  return NotFound();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return categoriaDTO;
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
        public ActionResult Post([FromBody]CategoriaDTO categoriaDTO)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(categoriaDTO);

                _uof.CategoriaRepository.Add(categoria);
                _uof.Commit();

                 var categoriaResultDTO = _mapper.Map<CategoriaDTO>(categoria);

                return Ok(categoriaResultDTO);
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
        public ActionResult Update(int id, CategoriaDTO categoriaDTO)
        {
            try
            {
                if (id != categoriaDTO.CategoriaId) return BadRequest();

                var categoria = _mapper.Map<Categoria>(categoriaDTO);

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
        public ActionResult<CategoriaDTO> Delete(int id)
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