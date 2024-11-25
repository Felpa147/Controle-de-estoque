using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Controle_de_estoque.Models
{
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [StringLength(255)]
        public string Descricao { get; set; }

        [StringLength(50)]
        public string CodigoDeBarras { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoCompra { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoVenda { get; set; }

        public int QuantidadeEmEstoque { get; set; }

        [StringLength(20)]
        public string UnidadeDeMedida { get; set; }

        public DateTime DataValidade { get; set; }

        // Relacionamentos
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }
    }
}
