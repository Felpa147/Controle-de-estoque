using System.ComponentModel.DataAnnotations;

namespace Controle_de_estoque.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required]
        [StringLength(50)]
        public string NomeUsuario { get; set; }

        [Required]
        [StringLength(100)]
        public string Senha { get; set; }

        [StringLength(100)]
        public string NomeCompleto { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Perfil { get; set; } // Ex: "Administrador"
    }
}
