using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Controle_de_estoque.Models
{
    public class SaidaEstoque
    {
        [Key]
        public int SaidaEstoqueId { get; set; }

        [Required(ErrorMessage = "O ProdutoId é obrigatório.")]
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "A data de saída é obrigatória.")]
        public DateTime DataSaida { get; set; }

        [Required(ErrorMessage = "O preço unitário é obrigatório.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço unitário deve ser maior que zero.")]
        public decimal PrecoUnitario { get; set; }

        [StringLength(255, ErrorMessage = "A observação não pode exceder 255 caracteres.")]
        public string Observacao { get; set; }
    }
}
