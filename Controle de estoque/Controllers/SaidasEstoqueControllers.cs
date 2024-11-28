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
    public class SaidasEstoqueController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SaidasEstoqueController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SaidasEstoque
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaidaEstoque>>> GetSaidasEstoque()
        {
            return await _context.SaidasEstoque
                .Include(s => s.Produto)
                .ToListAsync();
        }

        // POST: api/SaidasEstoque
        [HttpPost]
        public async Task<ActionResult<SaidaEstoque>> PostSaidaEstoque(SaidaEstoque saidaEstoque)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar se há quantidade suficiente em estoque
            var produto = await _context.Produtos.FindAsync(saidaEstoque.ProdutoId);
            if (produto == null)
            {
                return BadRequest("Produto não encontrado.");
            }

            if (produto.QuantidadeEmEstoque < saidaEstoque.Quantidade)
            {
                return BadRequest("Quantidade em estoque insuficiente.");
            }

            _context.SaidasEstoque.Add(saidaEstoque);

            // Atualizar a quantidade em estoque do produto
            produto.QuantidadeEmEstoque -= saidaEstoque.Quantidade;

            await _context.SaveChangesAsync();

            await _context.Entry(saidaEstoque).Reference(s => s.Produto).LoadAsync();

            return CreatedAtAction(nameof(GetSaidasEstoque), new { id = saidaEstoque.SaidaEstoqueId }, saidaEstoque);
        }

        // DELETE: api/SaidasEstoque/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSaidaEstoque(int id)
        {
            var saidaEstoque = await _context.SaidasEstoque.FindAsync(id);
            if (saidaEstoque == null)
            {
                return NotFound("Saída de estoque não encontrada.");
            }

            // Atualizar a quantidade em estoque do produto
            var produto = await _context.Produtos.FindAsync(saidaEstoque.ProdutoId);
            if (produto != null)
            {
                produto.QuantidadeEmEstoque += saidaEstoque.Quantidade;
            }

            _context.SaidasEstoque.Remove(saidaEstoque);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
