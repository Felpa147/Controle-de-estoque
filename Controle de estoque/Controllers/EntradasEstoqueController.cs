using Controle_de_estoque.Data;
using Controle_de_estoque.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // POST: api/EntradasEstoque
        [HttpPost]
        public async Task<ActionResult<EntradaEstoque>> PostEntradaEstoque(EntradaEstoque entradaEstoque)
        {
            // Adiciona a entrada de estoque
            _context.EntradasEstoque.Add(entradaEstoque);

            // Atualiza o QuantidadeEmEstoque do Produto
            var produto = await _context.Produtos.FindAsync(entradaEstoque.ProdutoId);
            if (produto == null)
            {
                return BadRequest("Produto não encontrado.");
            }

            produto.QuantidadeEmEstoque += entradaEstoque.Quantidade;

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostEntradaEstoque), new { id = entradaEstoque.EntradaEstoqueId }, entradaEstoque);
        }
    }
}