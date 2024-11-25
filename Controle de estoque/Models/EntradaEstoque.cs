using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Controle_de_estoque.Models
{
    public class EntradaEstoque
    {
        [Key]
        public int EntradaEstoqueId { get; set; }

        [Required]
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Required]
        public DateTime DataEntrada { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoUnitario { get; set; }

        [Required]
        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }

        [StringLength(50)]
        public string Lote { get; set; }
    }
}
