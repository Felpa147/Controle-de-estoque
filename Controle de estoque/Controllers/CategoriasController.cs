using Controle_de_estoque.Data;
using Controle_de_estoque.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controle_de_estoque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Categorias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            return await _context.Categorias.ToListAsync();
        }

        // GET: api/Categorias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound("Categoria não encontrada.");
            }

            return categoria;
        }

        // POST: api/Categorias
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria)
        {
            // Validação: Verificar se o nome da categoria já existe
            if (await _context.Categorias.AnyAsync(c => c.Nome == categoria.Nome))
            {
                return BadRequest("Já existe uma categoria com este nome.");
            }

            // Validação de modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.CategoriaId }, categoria);
        }

        // PUT: api/Categorias/5
        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest("O ID fornecido não corresponde ao ID da categoria.");
            }

            // Validação: Verificar se o nome da categoria já existe (exceto para a própria categoria)
            if (await _context.Categorias.AnyAsync(c => c.Nome == categoria.Nome && c.CategoriaId != id))
            {
                return BadRequest("Já existe outra categoria com este nome.");
            }

            // Validação de modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
                {
                    return NotFound("Categoria não encontrada.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Categorias/5
        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound("Categoria não encontrada.");
            }

            // Regra de negócio: Não permitir excluir categoria com produtos associados
            bool hasProdutos = await _context.Produtos.AnyAsync(p => p.CategoriaId == id);
            if (hasProdutos)
            {
                return BadRequest("Não é possível excluir uma categoria que possui produtos associados.");
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.CategoriaId == id);
        }
    }
}