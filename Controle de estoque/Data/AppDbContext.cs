using Controle_de_estoque.Models;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar relacionamento entre Produto e Categoria
            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar relacionamento entre Produto e Fornecedor
            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Fornecedor)
                .WithMany(f => f.Produtos)
                .HasForeignKey(p => p.FornecedorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar relacionamento entre EntradaEstoque e Produto
            modelBuilder.Entity<EntradaEstoque>()
                .HasOne(e => e.Produto)
                .WithMany()
                .HasForeignKey(e => e.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar relacionamento entre EntradaEstoque e Fornecedor
          modelBuilder.Entity<EntradaEstoque>()
    .HasOne(e => e.Fornecedor)
    .WithMany(f => f.EntradasEstoque) // Relacionamento inverso definido
    .HasForeignKey(e => e.FornecedorId)
    .OnDelete(DeleteBehavior.SetNull); // Define NULL ao excluir o fornecedor


            // Configurar relacionamento entre SaidaEstoque e Produto
            modelBuilder.Entity<SaidaEstoque>()
                .HasOne(s => s.Produto)
                .WithMany()
                .HasForeignKey(s => s.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar unicidade para campos específicos
            modelBuilder.Entity<Fornecedor>()
        .HasIndex(f => f.CNPJ)
        .HasDatabaseName("IX_Fornecedores_CNPJ")
        .IsUnique()
        .HasAnnotation("MySQL:IndexLength", 14); // Especificando o comprimento do índice

            modelBuilder.Entity<Produto>()
                .HasIndex(p => p.CodigoIdentificacao)
                .IsUnique();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
}

    }
}
