using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CatalogoAPI.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }
        [Required]
        [MaxLength(80)]
        public string Nome { get; set; }
        [Required]
        [MaxLength(300)]        
        public string ImagemUrl { get; set; }
        public ICollection<Produto> Produtos { get; set; }

        // Model validation in the model scope
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) 
        {
            if (!string.IsNullOrEmpty(this.Nome)) 
            {
                var primeiraLetra = this.Nome[0].ToString();

                if (primeiraLetra != primeiraLetra.ToUpper()) 
                {
                    yield return new ValidationResult("A primeira letra do Nome deve ser em caixa alta (Letra Maiúscula)",
                    new[] 
                    { nameof(this.Nome) } 
                    );
                }
            }
        }
    }
}