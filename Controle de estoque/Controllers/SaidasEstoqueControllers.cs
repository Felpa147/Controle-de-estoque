using Controle_de_estoque.Data;
using Controle_de_estoque.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // POST: api/SaidasEstoque
        [HttpPost]
        public async Task<ActionResult<SaidaEstoque>> PostSaidaEstoque(SaidaEstoque saidaEstoque)
        {
            // Verifica se o produto existe
            var produto = await _context.Produtos.FindAsync(saidaEstoque.ProdutoId);
            if (produto == null)
            {
                return BadRequest("Produto não encontrado.");
            }

            // Verifica se há quantidade suficiente em estoque
            if (produto.QuantidadeEmEstoque < saidaEstoque.Quantidade)
            {
                return BadRequest("Quantidade insuficiente em estoque.");
            }

            // Adiciona a saída de estoque
            _context.SaidasEstoque.Add(saidaEstoque);

            // Atualiza o QuantidadeEmEstoque do Produto
            produto.QuantidadeEmEstoque -= saidaEstoque.Quantidade;

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostSaidaEstoque), new { id = saidaEstoque.SaidaEstoqueId }, saidaEstoque);
        }
    }
}