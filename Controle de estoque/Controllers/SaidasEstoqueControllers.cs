using Controle_de_estoque.Data;
using Controle_de_estoque.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

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
        [Authorize(Roles = "Operador,Administrador")]
        [HttpPost]
        public async Task<ActionResult<SaidaEstoque>> PostSaidaEstoque(SaidaEstoque saidaEstoque)
        {
            if (saidaEstoque.Quantidade <= 0)
            {
                return BadRequest("A quantidade deve ser maior que zero.");
            }

            if (saidaEstoque.DataSaida.Date > DateTime.Now.Date)
            {
                return BadRequest("A data de saída não pode ser futura.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

            produto.QuantidadeEmEstoque -= saidaEstoque.Quantidade;

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostSaidaEstoque), new { id = saidaEstoque.SaidaEstoqueId }, saidaEstoque);
        }
    }
}
