using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Controle_de_estoque.Models
{
    public class SaidaEstoque
    {
        [Key]
        public int SaidaEstoqueId { get; set; }

        [Required]
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Required]
        public DateTime DataSaida { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoVenda { get; set; }

        [StringLength(255)]
        public string Observacoes { get; set; }
    }
}
