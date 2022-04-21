using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;
using CatalogoAPI.Validations;

namespace CatalogoAPI.Models
{
  public class Produto
  {
    public int ProdutoId { get; set; }
    [Required]
    [MaxLength(80)]
    [PrimeiraLetraMaiuscula]
    public string Nome { get; set; }
    [Required]
    [MaxLength(300)]
    public string Descricao { get; set; }
    [Required]
    [DataType(DataType.Currency)]
    [ColumnAttribute(TypeName = "decimal(8, 2)")]
    public decimal Preco { get; set; }
    [Required]
    public float Estoque { get; set; }
    [Required]
    [MaxLength(500)]
    public string ImagemUrl { get; set; }
    public DateTime DataCadastro { get; set; }
    public Categoria Categoria { get; set; }
    public int CategoriaId { get; set; }
    
  }
}