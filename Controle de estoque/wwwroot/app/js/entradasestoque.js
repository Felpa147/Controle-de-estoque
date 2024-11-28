// Chamar verificarAutenticacao e carregarEntradas ao carregar a página
document.addEventListener('DOMContentLoaded', () => {
    verificarAutenticacao();
    carregarProdutos();
    carregarFornecedores();
    carregarEntradas();
});

// Variáveis para armazenar os produtos e fornecedores
let produtos = [];
let fornecedores = [];

// Função para carregar os produtos
async function carregarProdutos() {
    const response = await fetch(`${apiUrl}/Produtos`);
    if (response.ok) {
        produtos = await response.json();
        preencherSelectProdutos();
    } else {
        alert('Erro ao carregar produtos.');
    }
}

// Função para preencher o select de produtos
function preencherSelectProdutos() {
    const selectProduto = document.getElementById('produtoId');
    selectProduto.innerHTML = '<option value="">Selecione um Produto</option>';
    produtos.forEach(produto => {
        const option = document.createElement('option');
        option.value = produto.produtoId;
        option.textContent = produto.nome;
        selectProduto.appendChild(option);
    });
}

// Função para carregar os fornecedores
async function carregarFornecedores() {
    const response = await fetch(`${apiUrl}/Fornecedores`);
    if (response.ok) {
        fornecedores = await response.json();
        preencherSelectFornecedores();
    } else {
        alert('Erro ao carregar fornecedores.');
    }
}

// Função para preencher o select de fornecedores
function preencherSelectFornecedores() {
    const selectFornecedor = document.getElementById('fornecedorId');
    selectFornecedor.innerHTML = '<option value="">Selecione um Fornecedor</option>';
    fornecedores.forEach(fornecedor => {
        const option = document.createElement('option');
        option.value = fornecedor.fornecedorId;
        option.textContent = fornecedor.nome;
        selectFornecedor.appendChild(option);
    });
}

// Função para carregar as entradas de estoque
async function carregarEntradas() {
    const response = await fetch(`${apiUrl}/EntradasEstoque`);
    if (response.ok) {
        const entradas = await response.json();
        console.log('Entradas recebidas:', entradas);
        exibirEntradas(entradas);
    } else {
        alert('Erro ao carregar entradas de estoque.');
    }
}


// Função para exibir as entradas na tabela
function exibirEntradas(entradas) {
    const tabela = document.getElementById('tabelaEntradas');
    tabela.innerHTML = '';
    // Cabeçalho da tabela
    const header = tabela.createTHead();
    const headerRow = header.insertRow();
    ['ID', 'Produto', 'Quantidade', 'Data', 'Preço Unitário', 'Fornecedor', 'Lote', 'Ações'].forEach(text => {
        const cell = headerRow.insertCell();
        cell.textContent = text;
    });
    // Corpo da tabela
    const tbody = tabela.createTBody();
    entradas.forEach(entrada => {
        const row = tbody.insertRow();
        row.insertCell().textContent = entrada.entradaEstoqueId;
        row.insertCell().textContent = entrada.produto ? entrada.produto.nome : '';
        row.insertCell().textContent = entrada.quantidade;
        row.insertCell().textContent = new Date(entrada.dataEntrada).toLocaleDateString();
        row.insertCell().textContent = entrada.precoUnitario.toFixed(2);
        row.insertCell().textContent = entrada.fornecedor ? entrada.fornecedor.nome : '';
        row.insertCell().textContent = entrada.lote || '';
        const actionsCell = row.insertCell();
        actionsCell.innerHTML = `
            <button onclick="excluirEntrada(${entrada.entradaEstoqueId})">Excluir</button>
        `;
    });
}

// Função para mostrar o formulário de adicionar
function mostrarFormularioAdicionar() {
    document.getElementById('formularioAdicionar').style.display = 'block';
}

// Função para fechar o formulário de adicionar
function fecharFormularioAdicionar() {
    document.getElementById('formularioAdicionar').style.display = 'none';
    document.getElementById('produtoId').value = '';
    document.getElementById('quantidade').value = '';
    document.getElementById('dataEntrada').value = '';
    document.getElementById('precoUnitario').value = '';
    document.getElementById('fornecedorId').value = '';
    document.getElementById('lote').value = '';
}

// Função para adicionar entrada de estoque
async function adicionarEntrada() {
    const produtoId = parseInt(document.getElementById('produtoId').value);
    const quantidade = parseInt(document.getElementById('quantidade').value);
    const dataEntrada = document.getElementById('dataEntrada').value;
    const precoUnitario = parseFloat(document.getElementById('precoUnitario').value);
    const fornecedorId = document.getElementById('fornecedorId').value ? parseInt(document.getElementById('fornecedorId').value) : null;
    const lote = document.getElementById('lote').value;

    // Validações básicas
    if (!produtoId || isNaN(quantidade) || !dataEntrada || isNaN(precoUnitario)) {
        alert('Por favor, preencha todos os campos obrigatórios.');
        return;
    }

    const entrada = {
        ProdutoId: produtoId,
        Quantidade: quantidade,
        DataEntrada: dataEntrada,
        PrecoUnitario: precoUnitario,
        FornecedorId: fornecedorId,
        Lote: lote
    };

    const response = await fetch(`${apiUrl}/EntradasEstoque`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(entrada)
    });

    if (response.ok) {
        alert('Entrada de estoque registrada com sucesso.');
        fecharFormularioAdicionar();
        carregarEntradas();
    } else {
        const error = await response.text();
        alert(`Erro ao registrar entrada de estoque: ${error}`);
    }
}

// Função para excluir entrada de estoque
async function excluirEntrada(id) {
    if (confirm('Tem certeza que deseja excluir esta entrada de estoque?')) {
        const response = await fetch(`${apiUrl}/EntradasEstoque/${id}`, {
            method: 'DELETE'
        });
        if (response.ok) {
            alert('Entrada de estoque excluída com sucesso.');
            carregarEntradas();
        } else {
            const error = await response.text();
            alert(`Erro ao excluir entrada de estoque: ${error}`);
        }
    }
}
