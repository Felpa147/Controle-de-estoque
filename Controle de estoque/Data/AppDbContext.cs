using Controle_de_estoque.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Controle_de_estoque.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<EntradaEstoque> EntradasEstoque { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<SaidaEstoque> SaidasEstoque { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Configurações adicionais podem ser adicionadas aqui, se necessário
    }
}
