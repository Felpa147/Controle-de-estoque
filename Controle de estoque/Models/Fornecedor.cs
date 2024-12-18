﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Controle_de_estoque.Models
{
    public class Fornecedor
    {
        [Key]
        public int FornecedorId { get; set; }

        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; }

        [StringLength(14, ErrorMessage = "O CNPJ deve ter 14 caracteres.")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "O CNPJ deve conter apenas números.")]
        [Column(TypeName = "VARCHAR(14)")] // Especificando VARCHAR com comprimento
        public string CNPJ { get; set; }

        [StringLength(20, ErrorMessage = "O telefone não pode exceder 20 caracteres.")]
        public string Telefone { get; set; }

        [StringLength(100, ErrorMessage = "O email não pode exceder 100 caracteres.")]
        [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
        public string Email { get; set; }

        [StringLength(200, ErrorMessage = "O endereço não pode exceder 200 caracteres.")]
        public string Endereco { get; set; }

        [StringLength(100, ErrorMessage = "O contato não pode exceder 100 caracteres.")]
        public string Contato { get; set; }

        // Relacionamentos
        [JsonIgnore]
        public ICollection<Produto>? Produtos { get; set; }

        [JsonIgnore]
        public ICollection<EntradaEstoque>? EntradasEstoque { get; set; }
    }
}
