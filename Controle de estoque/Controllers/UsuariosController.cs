using Controle_de_estoque.Data;
using Controle_de_estoque.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace Controle_de_estoque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UsuariosController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Método para hashear senhas
        private string HashPassword(string senha)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(senha));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // POST: api/Usuarios/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            // Hashear a senha fornecida
            var senhaHash = HashPassword(login.Senha);

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NomeUsuario == login.NomeUsuario && u.Senha == senhaHash);

            if (usuario == null)
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.NomeUsuario),
                new Claim(ClaimTypes.Role, usuario.Perfil)
            };

            var chaveSecreta = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key")));
            var creds = new SigningCredentials(chaveSecreta, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        // GET: api/Usuarios
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuarios/5
        [Authorize(Roles = "Administrador")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            return usuario;
        }

        // POST: api/Usuarios
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            // Validação: Verificar se o nome de usuário já existe
            if (await _context.Usuarios.AnyAsync(u => u.NomeUsuario == usuario.NomeUsuario))
            {
                return BadRequest("Já existe um usuário com este nome de usuário.");
            }

            // Validação de modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Hashear a senha antes de salvar
            usuario.Senha = HashPassword(usuario.Senha);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.UsuarioId }, usuario);
        }

        // PUT: api/Usuarios/5
        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return BadRequest("O ID fornecido não corresponde ao ID do usuário.");
            }

            // Validação: Verificar se o nome de usuário já existe em outro usuário
            if (await _context.Usuarios.AnyAsync(u => u.NomeUsuario == usuario.NomeUsuario && u.UsuarioId != id))
            {
                return BadRequest("Já existe outro usuário com este nome de usuário.");
            }

            // Validação de modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Manter a senha atual se não for fornecida uma nova
            var usuarioAtual = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.UsuarioId == id);
            if (usuarioAtual == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            if (string.IsNullOrEmpty(usuario.Senha))
            {
                usuario.Senha = usuarioAtual.Senha;
            }
            else
            {
                // Hashear a nova senha
                usuario.Senha = HashPassword(usuario.Senha);
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound("Usuário não encontrado.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Usuarios/5
        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            // Impedir que o usuário exclua a si mesmo
            var usuarioLogado = User.Identity.Name;
            if (usuario.NomeUsuario == usuarioLogado)
            {
                return BadRequest("Você não pode excluir a si mesmo.");
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
    }
}
