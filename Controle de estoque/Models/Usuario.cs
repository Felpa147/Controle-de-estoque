using System.ComponentModel.DataAnnotations;

namespace Controle_de_estoque.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome de usuário não pode exceder 50 caracteres.")]
        public string NomeUsuario { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, ErrorMessage = "A senha não pode exceder 100 caracteres.")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome completo não pode exceder 100 caracteres.")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [StringLength(100, ErrorMessage = "O email não pode exceder 100 caracteres.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O perfil é obrigatório.")]
        [StringLength(20, ErrorMessage = "O perfil não pode exceder 20 caracteres.")]
        public string Perfil { get; set; } // "Administrador", "Operador"
    }
}
