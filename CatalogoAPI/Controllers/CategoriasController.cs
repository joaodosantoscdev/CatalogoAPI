using System.Net;
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
using CatalogoAPI.Pagination;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace CatalogoAPI.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Authorize(AuthenticationSchemes = "Bearer")]
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
        /// <summary>
        ///     Retorna as categorias e seus respectivos produtos
        /// </summary>
        /// <returns></returns>
        
        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutosAsync()
        {
            try
            {
                // Example of ILogger interface usage for debugging purposes.
                // _logger.Log(LogLevel.Information, ">>>>>>>>>>>>>>>>>> ENTROU <<<<<<<<<<<<<<<<<<<<");

                var categoria = await _uof.CategoriaRepository.GetCategoriasProdutos();
                var categoriaDTO = _mapper.Map<List<CategoriaDTO>>(categoria);

                return categoriaDTO;
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                            $"Erro ao tentar buscar todas Categorias. Erro {e.Message}"); 
            }
        }

        /// <summary>
        ///  Retorna toda as categorias contidas no banco
        /// </summary>
        /// <param name="categoriasParameters"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAll([FromQuery]CategoriasParameters categoriasParameters)
        {
            try
            {
                var categorias = await _uof.CategoriaRepository.GetCategorias(categoriasParameters);

                var metadata = new {
                    categorias.TotalCount,
                    categorias.PageSize,
                    categorias.CurrentPage,
                    categorias.TotalPages,
                    categorias.HasPrevious,
                    categorias.HasNext

                };

                var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);

                return categoriasDTO;
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                            $"Erro ao tentar buscar todas Categorias. Erro {e.Message}"); 
            }
            
        }

        /// <summary>
        ///     Retorna uma categoria pelo ID solicitado 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDTO>> GetById(int id)
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
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
        /// <summary>
        ///     Cadastra uma nova categoria no banco
        /// </summary>
        /// <param name="categoriaDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]CategoriaDTO categoriaDTO)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(categoriaDTO);

                _uof.CategoriaRepository.Add(categoria);
                await _uof.Commit();

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
        /// <summary>
        ///     Atualiza uma categoria contida na base de dado de acordo com o ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoriaDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, CategoriaDTO categoriaDTO)
        {
            try
            {
                if (id != categoriaDTO.CategoriaId) return BadRequest();

                var categoria = _mapper.Map<Categoria>(categoriaDTO);

                _uof.CategoriaRepository.Update(categoria);
                await _uof.Commit();

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
        /// <summary>
        ///     Deleta uma categoria de acordo com ID passado na requisição
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            try
            {
                var categoriaFiltrada = await _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
                if (categoriaFiltrada == null) return null;

                _uof.CategoriaRepository.Delete(categoriaFiltrada);
                await _uof.Commit();

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