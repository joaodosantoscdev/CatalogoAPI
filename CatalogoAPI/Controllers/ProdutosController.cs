using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CatalogoAPI.Models;
using CatalogoAPI.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using CatalogoAPI.Repositories.Interfaces;
using CatalogoAPI.DTOs;
using AutoMapper;

namespace MimicAPI.V1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController : ControllerBase
    {
        // Constructor and Dependencies
        #region DI Injected
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork uof, IMapper mapper) 
        {
            _uof = uof;
            _mapper = mapper;
        }
        #endregion

        // GET Methods - Produtos Controller
        #region GET Methods
        [HttpGet("produtoPreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutoPrecos()
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
            var produtoDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtoDTO;
        }

        [HttpGet("")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetAll()
        {
            try 
            {
                var produtos = _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
                var produtoDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

                return produtoDTO;
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                            $"Erro ao tentar buscar Produtos. Erro {e.Message}");
            } 
        }

        [HttpGet("{id}")]
        public ActionResult<ProdutoDTO> GetById(int id)
        {
            try 
            {
                var produtoFiltrado = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
                if (produtoFiltrado == null) return NotFound();

                var produtoDTO = _mapper.Map<ProdutoDTO>(produtoFiltrado);
            
                return Ok(produtoDTO);
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
        public ActionResult Post([FromBody]ProdutoDTO produtoDTO)
        {
            try 
            {
                var produto = _mapper.Map<Produto>(produtoDTO);
                _uof.ProdutoRepository.Add(produto);
                _uof.Commit();

                var produtoResultDTO = _mapper.Map<ProdutoDTO>(produto);

                return Ok(produtoResultDTO);
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
        public ActionResult Update(int id, ProdutoDTO produtoDTO)
        { 
            try 
            {
                if(id != produtoDTO.ProdutoId) return BadRequest();

                var produto = _mapper.Map<Produto>(produtoDTO);

                _uof.ProdutoRepository.Update(produto);
                _uof.Commit();

                return Ok();
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
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            try 
            {            
                var produtoFiltrado = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
                 /* The Find(); method works too, searching first on memory,
                  but only if ID is a primary key */

                 // var produtoFiltrado = _context.Produtos.Find(id);
                if (produtoFiltrado == null) return NotFound();

                _uof.ProdutoRepository.Delete(produtoFiltrado);
                _uof.Commit();

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