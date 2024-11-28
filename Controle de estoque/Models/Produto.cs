using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Controle_de_estoque.Models
{
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }

        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; }

        [StringLength(255, ErrorMessage = "A descrição não pode exceder 255 caracteres.")]
        public string Descricao { get; set; }

        [StringLength(50, ErrorMessage = "O código de barras não pode exceder 50 caracteres.")]
        public string CodigoIdentificacao { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço de compra deve ser maior que zero.")]
        public decimal PrecoCompra { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço de venda deve ser maior que zero.")]
        public decimal PrecoVenda { get; set; }

        public int QuantidadeEmEstoque { get; set; }

        [StringLength(20, ErrorMessage = "A unidade de medida não pode exceder 20 caracteres.")]
        public string UnidadeDeMedida { get; set; }

        public DateTime DataValidade { get; set; }

        // Relacionamentos
        public int? CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        public int? FornecedorId { get; set; }
        public Fornecedor? Fornecedor { get; set; }
    }
}
