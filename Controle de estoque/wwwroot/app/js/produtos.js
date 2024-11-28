// Chamar verificarAutenticacao e carregarProdutos ao carregar a página
document.addEventListener('DOMContentLoaded', () => {
    verificarAutenticacao();
    carregarCategorias();
    carregarFornecedores();
    carregarProdutos();
});

// Variáveis para armazenar as categorias e fornecedores
let categorias = [];
let fornecedores = [];

// Função para carregar as categorias
async function carregarCategorias() {
    const response = await fetch(`${apiUrl}/Categorias`);
    if (response.ok) {
        categorias = await response.json();
        preencherSelectCategorias();
    } else {
        alert('Erro ao carregar categorias.');
    }
}

// Função para preencher o select de categorias
function preencherSelectCategorias() {
    const selectCategoria = document.getElementById('categoriaId');
    const selectEditarCategoria = document.getElementById('editarCategoriaId');
    selectCategoria.innerHTML = '<option value="">Selecione uma Categoria</option>';
    selectEditarCategoria.innerHTML = '<option value="">Selecione uma Categoria</option>';
    categorias.forEach(categoria => {
        const option = document.createElement('option');
        option.value = categoria.categoriaId;
        option.textContent = categoria.nome;
        selectCategoria.appendChild(option);
        selectEditarCategoria.appendChild(option.cloneNode(true));
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
    const selectEditarFornecedor = document.getElementById('editarFornecedorId');
    selectFornecedor.innerHTML = '<option value="">Selecione um Fornecedor</option>';
    selectEditarFornecedor.innerHTML = '<option value="">Selecione um Fornecedor</option>';
    fornecedores.forEach(fornecedor => {
        const option = document.createElement('option');
        option.value = fornecedor.fornecedorId;
        option.textContent = fornecedor.nome;
        selectFornecedor.appendChild(option);
        selectEditarFornecedor.appendChild(option.cloneNode(true));
    });
}

// Função para carregar os produtos
async function carregarProdutos() {
    const response = await fetch(`${apiUrl}/Produtos`);
    if (response.ok) {
        const produtos = await response.json();
        exibirProdutos(produtos);
    } else {
        alert('Erro ao carregar produtos.');
    }
}

// Função para exibir os produtos na tabela
function exibirProdutos(produtos) {
    const tabela = document.getElementById('tabelaProdutos');
    tabela.innerHTML = '';
    // Cabeçalho da tabela
    const header = tabela.createTHead();
    const headerRow = header.insertRow();
    ['ID', 'Nome', 'Descrição', 'Código', 'Preço Compra', 'Preço Venda', 'Unidade', 'Validade', 'Categoria', 'Fornecedor', 'Ações'].forEach(text => {
        const cell = headerRow.insertCell();
        cell.textContent = text;
    });
    // Corpo da tabela
    const tbody = tabela.createTBody();
    produtos.forEach(produto => {
        const row = tbody.insertRow();
        row.insertCell().textContent = produto.produtoId;
        row.insertCell().textContent = produto.nome;
        row.insertCell().textContent = produto.descricao || '';
        row.insertCell().textContent = produto.codigoIdentificacao;
        row.insertCell().textContent = produto.precoCompra.toFixed(2);
        row.insertCell().textContent = produto.precoVenda.toFixed(2);
        row.insertCell().textContent = produto.unidadeDeMedida;
        row.insertCell().textContent = new Date(produto.dataValidade).toLocaleDateString();
        row.insertCell().textContent = produto.categoria ? produto.categoria.nome : '';
        row.insertCell().textContent = produto.fornecedor ? produto.fornecedor.nome : '';
        const actionsCell = row.insertCell();
        actionsCell.innerHTML = `
            <button onclick="mostrarFormularioEditar(${produto.produtoId})">Editar</button>
            <button onclick="excluirProduto(${produto.produtoId})">Excluir</button>
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
    document.getElementById('nomeProduto').value = '';
    document.getElementById('descricaoProduto').value = '';
    document.getElementById('codigoIdentificacao').value = '';
    document.getElementById('precoCompra').value = '';
    document.getElementById('precoVenda').value = '';
    document.getElementById('unidadeDeMedida').value = '';
    document.getElementById('dataValidade').value = '';
    document.getElementById('categoriaId').value = '';
    document.getElementById('fornecedorId').value = '';
}

async function adicionarProduto() {
    const nome = document.getElementById('nomeProduto').value;
    const descricao = document.getElementById('descricaoProduto').value;
    const codigoIdentificacao = document.getElementById('codigoIdentificacao').value;
    const precoCompra = parseFloat(document.getElementById('precoCompra').value);
    const precoVenda = parseFloat(document.getElementById('precoVenda').value);
    const unidadeDeMedida = document.getElementById('unidadeDeMedida').value;
    const dataValidade = document.getElementById('dataValidade').value;
    const categoriaId = document.getElementById('categoriaId').value;
    const fornecedorId = document.getElementById('fornecedorId').value;

    // Validações básicas
    if (!nome || !codigoIdentificacao || !precoCompra || !precoVenda || !dataValidade) {
        alert('Por favor, preencha todos os campos obrigatórios.');
        return;
    }

    // Verificar se a categoria foi selecionada
    if (!categoriaId) {
        alert('Por favor, selecione uma categoria.');
        return;
    }

    // Verificar se o fornecedor foi selecionado
    if (!fornecedorId) {
        alert('Por favor, selecione um fornecedor.');
        return;
    }

    const produto = {
        Nome: nome,
        Descricao: descricao,
        CodigoIdentificacao: codigoIdentificacao,
        PrecoCompra: precoCompra,
        PrecoVenda: precoVenda,
        UnidadeDeMedida: unidadeDeMedida,
        DataValidade: dataValidade,
        CategoriaId: parseInt(categoriaId),
        FornecedorId: parseInt(fornecedorId)
    };

    const response = await fetch(`${apiUrl}/Produtos`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(produto)
    });

    if (response.ok) {
        alert('Produto adicionado com sucesso.');
        fecharFormularioAdicionar();
        carregarProdutos();
    } else {
        const error = await response.text();
        alert(`Erro ao adicionar produto: ${error}`);
    }
}

// Função para mostrar o formulário de editar
async function mostrarFormularioEditar(id) {
    const response = await fetch(`${apiUrl}/Produtos/${id}`);
    if (response.ok) {
        const produto = await response.json();
        document.getElementById('produtoId').value = produto.produtoId;
        document.getElementById('editarNomeProduto').value = produto.nome;
        document.getElementById('editarDescricaoProduto').value = produto.descricao || '';
        document.getElementById('editarCodigoIdentificacao').value = produto.codigoIdentificacao;
        document.getElementById('editarPrecoCompra').value = produto.precoCompra;
        document.getElementById('editarPrecoVenda').value = produto.precoVenda;
        document.getElementById('editarUnidadeDeMedida').value = produto.unidadeDeMedida;
        document.getElementById('editarDataValidade').value = produto.dataValidade.split('T')[0];
        document.getElementById('editarCategoriaId').value = produto.categoriaId || '';
        document.getElementById('editarFornecedorId').value = produto.fornecedorId || '';

        document.getElementById('formularioEditar').style.display = 'block';
    } else {
        alert('Erro ao carregar produto para edição.');
    }
}

// Função para fechar o formulário de editar
function fecharFormularioEditar() {
    document.getElementById('formularioEditar').style.display = 'none';
    document.getElementById('produtoId').value = '';
    document.getElementById('editarNomeProduto').value = '';
    document.getElementById('editarDescricaoProduto').value = '';
    document.getElementById('editarCodigoIdentificacao').value = '';
    document.getElementById('editarPrecoCompra').value = '';
    document.getElementById('editarPrecoVenda').value = '';
    document.getElementById('editarUnidadeDeMedida').value = '';
    document.getElementById('editarDataValidade').value = '';
    document.getElementById('editarCategoriaId').value = '';
    document.getElementById('editarFornecedorId').value = '';
}

// Função para atualizar produto
async function atualizarProduto() {
    const id = document.getElementById('produtoId').value;
    const nome = document.getElementById('editarNomeProduto').value;
    const descricao = document.getElementById('editarDescricaoProduto').value;
    const codigoIdentificacao = document.getElementById('editarCodigoIdentificacao').value;
    const precoCompra = parseFloat(document.getElementById('editarPrecoCompra').value);
    const precoVenda = parseFloat(document.getElementById('editarPrecoVenda').value);
    const unidadeDeMedida = document.getElementById('editarUnidadeDeMedida').value;
    const dataValidade = document.getElementById('editarDataValidade').value;
    const categoriaId = parseInt(document.getElementById('editarCategoriaId').value);
    const fornecedorId = parseInt(document.getElementById('editarFornecedorId').value);

    // Validações básicas
    if (!nome || !codigoIdentificacao || !precoCompra || !precoVenda || !dataValidade) {
        alert('Por favor, preencha todos os campos obrigatórios.');
        return;
    }
    if (!categoriaId) {
        alert('Por favor, selecione uma categoria.');
        return;
    }

    if (!fornecedorId) {
        alert('Por favor, selecione um fornecedor.');
        return;
    }
    const produto = {
        ProdutoId: id,
        Nome: nome,
        Descricao: descricao,
        CodigoIdentificacao: codigoIdentificacao,
        PrecoCompra: precoCompra,
        PrecoVenda: precoVenda,
        UnidadeDeMedida: unidadeDeMedida,
        DataValidade: dataValidade,
        CategoriaId: categoriaId ,
        FornecedorId: fornecedorId
    };
    console.log('Dados do produto a ser enviado:', produto);

    const response = await fetch(`${apiUrl}/Produtos/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(produto)
    });

    if (response.ok) {
        alert('Produto atualizado com sucesso.');
        fecharFormularioEditar();
        carregarProdutos();
    } else {
        const error = await response.text();
        alert(`Erro ao atualizar produto: ${error}`);
    }
}

// Função para excluir produto
async function excluirProduto(id) {
    if (confirm('Tem certeza que deseja excluir este produto?')) {
        const response = await fetch(`${apiUrl}/Produtos/${id}`, {
            method: 'DELETE'
        });
        if (response.ok) {
            alert('Produto excluído com sucesso.');
            carregarProdutos();
        } else {
            const error = await response.text();
            alert(`Erro ao excluir produto: ${error}`);
        }
    }
}
