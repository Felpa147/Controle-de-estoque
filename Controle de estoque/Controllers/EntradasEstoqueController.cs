using Controle_de_estoque.Data;
using Controle_de_estoque.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Controle_de_estoque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntradasEstoqueController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EntradasEstoqueController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/EntradasEstoque
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntradaEstoque>>> GetEntradasEstoque()
        {
            return await _context.EntradasEstoque
                .Include(e => e.Produto)
                .Include(e => e.Fornecedor)
                .ToListAsync();
        }

        // POST: api/EntradasEstoque
        [HttpPost]
        public async Task<ActionResult<EntradaEstoque>> PostEntradaEstoque(EntradaEstoque entradaEstoque)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.EntradasEstoque.Add(entradaEstoque);

            // Atualizar a quantidade em estoque do produto
            var produto = await _context.Produtos.FindAsync(entradaEstoque.ProdutoId);
            if (produto == null)
            {
                return BadRequest("Produto não encontrado.");
            }

            produto.QuantidadeEmEstoque += entradaEstoque.Quantidade;

            await _context.SaveChangesAsync();
            await _context.Entry(entradaEstoque).Reference(e => e.Produto).LoadAsync();
            await _context.Entry(entradaEstoque).Reference(e => e.Fornecedor).LoadAsync();
            return CreatedAtAction(nameof(GetEntradasEstoque), new { id = entradaEstoque.EntradaEstoqueId }, entradaEstoque);
        }

        // DELETE: api/EntradasEstoque/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntradaEstoque(int id)
        {
            var entradaEstoque = await _context.EntradasEstoque.FindAsync(id);
            if (entradaEstoque == null)
            {
                return NotFound("Entrada de estoque não encontrada.");
            }

            // Atualizar a quantidade em estoque do produto
            var produto = await _context.Produtos.FindAsync(entradaEstoque.ProdutoId);
            if (produto != null)
            {
                produto.QuantidadeEmEstoque -= entradaEstoque.Quantidade;
            }

            _context.EntradasEstoque.Remove(entradaEstoque);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
