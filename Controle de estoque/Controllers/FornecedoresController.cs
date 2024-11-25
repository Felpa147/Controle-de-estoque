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
    public class FornecedoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FornecedoresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Fornecedores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fornecedor>>> GetFornecedores()
        {
            return await _context.Fornecedores.ToListAsync();
        }

        // GET: api/Fornecedores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fornecedor>> GetFornecedor(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);

            if (fornecedor == null)
            {
                return NotFound("Fornecedor não encontrado.");
            }

            return fornecedor;
        }

        // POST: api/Fornecedores
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<ActionResult<Fornecedor>> PostFornecedor(Fornecedor fornecedor)
        {
            // Validação: Verificar se o CNPJ já existe
            if (await _context.Fornecedores.AnyAsync(f => f.CNPJ == fornecedor.CNPJ))
            {
                return BadRequest("Já existe um fornecedor com este CNPJ.");
            }

            // Validação do modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Fornecedores.Add(fornecedor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFornecedor), new { id = fornecedor.FornecedorId }, fornecedor);
        }

        // PUT: api/Fornecedores/5
        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFornecedor(int id, Fornecedor fornecedor)
        {
            if (id != fornecedor.FornecedorId)
            {
                return BadRequest("O ID fornecido não corresponde ao ID do fornecedor.");
            }

            // Validação: Verificar se o CNPJ já existe em outro fornecedor
            if (await _context.Fornecedores.AnyAsync(f => f.CNPJ == fornecedor.CNPJ && f.FornecedorId != id))
            {
                return BadRequest("Já existe outro fornecedor com este CNPJ.");
            }

            // Validação do modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(fornecedor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FornecedorExists(id))
                {
                    return NotFound("Fornecedor não encontrado.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Fornecedores/5
        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFornecedor(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor == null)
            {
                return NotFound("Fornecedor não encontrado.");
            }

            // Regra de negócio: Não permitir excluir fornecedor com produtos associados
            bool hasProdutos = await _context.Produtos.AnyAsync(p => p.FornecedorId == id);
            if (hasProdutos)
            {
                return BadRequest("Não é possível excluir um fornecedor que possui produtos associados.");
            }

            // Regra de negócio: Não permitir excluir fornecedor com entradas de estoque associadas
            bool hasEntradasEstoque = await _context.EntradasEstoque.AnyAsync(e => e.FornecedorId == id);
            if (hasEntradasEstoque)
            {
                return BadRequest("Não é possível excluir um fornecedor que possui entradas de estoque associadas.");
            }

            _context.Fornecedores.Remove(fornecedor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FornecedorExists(int id)
        {
            return _context.Fornecedores.Any(e => e.FornecedorId == id);
        }
    }
}
