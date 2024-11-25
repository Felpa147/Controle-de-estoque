using Controle_de_estoque.Data;
using Controle_de_estoque.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Operador,Administrador")]
        [HttpPost]
        public async Task<ActionResult<EntradaEstoque>> PostEntradaEstoque(EntradaEstoque entradaEstoque)
        {
            if (entradaEstoque.Quantidade <= 0)
            {
                return BadRequest("A quantidade deve ser maior que zero.");
            }

            if (entradaEstoque.DataEntrada.Date > DateTime.Now.Date)
            {
                return BadRequest("A data de entrada não pode ser futura.");
            }

            var produto = await _context.Produtos.FindAsync(entradaEstoque.ProdutoId);
            if (produto == null)
            {
                return BadRequest("Produto não encontrado.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.EntradasEstoque.Add(entradaEstoque);

            produto.QuantidadeEmEstoque += entradaEstoque.Quantidade;

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostEntradaEstoque), new { id = entradaEstoque.EntradaEstoqueId }, entradaEstoque);
        }
    }
}