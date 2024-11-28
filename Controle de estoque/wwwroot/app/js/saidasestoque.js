// Chamar verificarAutenticacao e carregarSaidas ao carregar a página
document.addEventListener('DOMContentLoaded', () => {
    verificarAutenticacao();
    carregarProdutos();
    carregarSaidas();
});

// Variável para armazenar os produtos
let produtos = [];

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

// Função para carregar as saídas de estoque
async function carregarSaidas() {
    const response = await fetch(`${apiUrl}/SaidasEstoque`);
    if (response.ok) {
        const saidas = await response.json();
        exibirSaidas(saidas);
    } else {
        alert('Erro ao carregar saídas de estoque.');
    }
}

// Função para exibir as saídas na tabela
function exibirSaidas(saidas) {
    const tabela = document.getElementById('tabelaSaidas');
    tabela.innerHTML = '';
    // Cabeçalho da tabela
    const header = tabela.createTHead();
    const headerRow = header.insertRow();
    ['ID', 'Produto', 'Quantidade', 'Data', 'Preço Unitário', 'Observação', 'Ações'].forEach(text => {
        const cell = headerRow.insertCell();
        cell.textContent = text;
    });
    // Corpo da tabela
    const tbody = tabela.createTBody();
    saidas.forEach(saida => {
        const row = tbody.insertRow();
        row.insertCell().textContent = saida.saidaEstoqueId;
        row.insertCell().textContent = saida.produto ? saida.produto.nome : '';
        row.insertCell().textContent = saida.quantidade;
        row.insertCell().textContent = new Date(saida.dataSaida).toLocaleDateString();
        row.insertCell().textContent = saida.precoUnitario.toFixed(2);
        row.insertCell().textContent = saida.observacao || '';
        const actionsCell = row.insertCell();
        actionsCell.innerHTML = `
            <button onclick="excluirSaida(${saida.saidaEstoqueId})">Excluir</button>
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
    document.getElementById('dataSaida').value = '';
    document.getElementById('precoUnitario').value = '';
    document.getElementById('observacao').value = '';
}

// Função para adicionar saída de estoque
async function adicionarSaida() {
    const produtoId = parseInt(document.getElementById('produtoId').value);
    const quantidade = parseInt(document.getElementById('quantidade').value);
    const dataSaida = document.getElementById('dataSaida').value;
    const precoUnitario = parseFloat(document.getElementById('precoUnitario').value);
    const observacao = document.getElementById('observacao').value;

    // Validações básicas
    if (!produtoId || isNaN(quantidade) || !dataSaida || isNaN(precoUnitario)) {
        alert('Por favor, preencha todos os campos obrigatórios.');
        return;
    }

    const saida = {
        ProdutoId: produtoId,
        Quantidade: quantidade,
        DataSaida: dataSaida,
        PrecoUnitario: precoUnitario,
        Observacao: observacao
    };

    const response = await fetch(`${apiUrl}/SaidasEstoque`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(saida)
    });

    if (response.ok) {
        alert('Saída de estoque registrada com sucesso.');
        fecharFormularioAdicionar();
        carregarSaidas();
    } else {
        const error = await response.text();
        alert(`Erro ao registrar saída de estoque: ${error}`);
    }
}

// Função para excluir saída de estoque
async function excluirSaida(id) {
    if (confirm('Tem certeza que deseja excluir esta saída de estoque?')) {
        const response = await fetch(`${apiUrl}/SaidasEstoque/${id}`, {
            method: 'DELETE'
        });
        if (response.ok) {
            alert('Saída de estoque excluída com sucesso.');
            carregarSaidas();
        } else {
            const error = await response.text();
            alert(`Erro ao excluir saída de estoque: ${error}`);
        }
    }
}
