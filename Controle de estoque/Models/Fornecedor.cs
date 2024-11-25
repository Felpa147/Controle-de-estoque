using System.ComponentModel.DataAnnotations;

namespace Controle_de_estoque.Models
{
    public class Fornecedor
    {
        [Key]
        public int FornecedorId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [StringLength(14)]
        public string CNPJ { get; set; }

        [StringLength(20)]
        public string Telefone { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(200)]
        public string Endereco { get; set; }

        [StringLength(100)]
        public string Contato { get; set; }

        // Relacionamentos
        public ICollection<Produto> Produtos { get; set; }
        public ICollection<EntradaEstoque> EntradasEstoque { get; set; }
    }
}
