using System.ComponentModel.DataAnnotations;

namespace Controle_de_estoque.Models
{
    public class Categoria
    {
        [Key]
        public int CategoriaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [StringLength(255)]
        public string Descricao { get; set; }

        // Relacionamentos
        public ICollection<Produto> Produtos { get; set; }
    }
}
