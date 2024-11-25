﻿using System.ComponentModel.DataAnnotations;

namespace Controle_de_estoque.Models
{
    public class Categoria
    {
        [Key]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome da categoria não pode exceder 50 caracteres.")]
        public string Nome { get; set; }

        [StringLength(255, ErrorMessage = "A descrição não pode exceder 255 caracteres.")]
        public string Descricao { get; set; }

        // Relacionamentos
        public ICollection<Produto> Produtos { get; set; }
    }
}