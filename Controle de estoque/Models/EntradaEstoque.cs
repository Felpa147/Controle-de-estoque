using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Controle_de_estoque.Models
{
    public class EntradaEstoque
    {
        [Key]
        public int EntradaEstoqueId { get; set; }

        [Required(ErrorMessage = "O campo ProdutoId é obrigatório.")]
        public int ProdutoId { get; set; }

        public Produto? Produto { get; set; }

        [Required(ErrorMessage = "O campo Quantidade é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "O campo DataEntrada é obrigatório.")]
        public DateTime DataEntrada { get; set; }

        [Required(ErrorMessage = "O campo PrecoUnitario é obrigatório.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoUnitario { get; set; }

        public int? FornecedorId { get; set; }

        public Fornecedor? Fornecedor { get; set; }

        [StringLength(50, ErrorMessage = "O lote não pode exceder 50 caracteres.")]
        public string Lote { get; set; }
    }
}
