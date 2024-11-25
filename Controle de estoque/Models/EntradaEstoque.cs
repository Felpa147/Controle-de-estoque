using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Controle_de_estoque.Models
{
    public class EntradaEstoque
    {
        [Key]
        public int EntradaEstoqueId { get; set; }

        [Required(ErrorMessage = "O ProdutoId é obrigatório.")]
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "A data de entrada é obrigatória.")]
        public DateTime DataEntrada { get; set; }

        [Required(ErrorMessage = "O preço unitário é obrigatório.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoUnitario { get; set; }

        [Required(ErrorMessage = "O FornecedorId é obrigatório.")]
        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }

        [StringLength(50, ErrorMessage = "O lote não pode exceder 50 caracteres.")]
        public string Lote { get; set; }
    }
}
