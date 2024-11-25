using Controle_de_estoque.Data;
using Controle_de_estoque.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Controle_de_estoque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            return await _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Fornecedor)
                .ToListAsync();
        }

        // GET: api/Produtos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Fornecedor)
                .FirstOrDefaultAsync(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound("Produto não encontrado.");
            }

            return produto;
        }

        // POST: api/Produtos
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto)
        {
            if (await _context.Produtos.AnyAsync(p => p.CodigoIdentificacao == produto.CodigoIdentificacao))
            {
                return BadRequest("Já existe um produto com este código de identificação.");
            }

            if (produto.DataValidade <= System.DateTime.Now)
            {
                return BadRequest("A data de validade deve ser futura.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduto), new { id = produto.ProdutoId }, produto);
        }

        // PUT: api/Produtos/5
        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest("O ID fornecido não corresponde ao ID do produto.");
            }

            if (await _context.Produtos.AnyAsync(p => p.CodigoIdentificacao == produto.CodigoIdentificacao && p.ProdutoId != id))
            {
                return BadRequest("Já existe outro produto com este código de barras.");
            }

            if (produto.DataValidade <= System.DateTime.Now)
            {
                return BadRequest("A data de validade deve ser futura.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Evitar atualização direta da quantidade em estoque, apenas os controllers de saida e entrada podem fazer isso.
            var quantidadeEmEstoqueAtual = await _context.Produtos
                .Where(p => p.ProdutoId == id)
                .Select(p => p.QuantidadeEmEstoque)
                .FirstOrDefaultAsync();

            produto.QuantidadeEmEstoque = quantidadeEmEstoqueAtual;

            _context.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                {
                    return NotFound("Produto não encontrado.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Produtos/5
        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound("Produto não encontrado.");
            }

            bool hasEntradas = await _context.EntradasEstoque.AnyAsync(e => e.ProdutoId == id);
            bool hasSaidas = await _context.SaidasEstoque.AnyAsync(s => s.ProdutoId == id);

            if (hasEntradas || hasSaidas)
            {
                return BadRequest("Não é possível excluir um produto que possui movimentações de estoque associadas.");
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.ProdutoId == id);
        }
    }
}
