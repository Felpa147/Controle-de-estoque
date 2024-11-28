// Função para carregar as categorias ao carregar a página
async function carregarCategorias() {
    try {
        const response = await fetch('/api/Categorias');
        if (response.ok) {
            const categorias = await response.json();
            exibirCategorias(categorias);
        } else {
            alert('Erro ao carregar categorias: ' + response.statusText);
        }
    } catch (error) {
        alert('Erro ao conectar com o servidor: ' + error.message);
    }
}

// Função para exibir as categorias na tabela
function exibirCategorias(categorias) {
    const tabela = document.getElementById('tabelaCategorias');
    tabela.innerHTML = '';

    // Criar cabeçalho da tabela
    const header = tabela.createTHead();
    const headerRow = header.insertRow();
    ['ID', 'Nome', 'Descrição', 'Ações'].forEach(text => {
        const cell = headerRow.insertCell();
        cell.textContent = text;
    });

    // Criar corpo da tabela
    const tbody = tabela.createTBody();
    categorias.forEach(categoria => {
        const row = tbody.insertRow();
        row.insertCell().textContent = categoria.categoriaId;
        row.insertCell().textContent = categoria.nome;
        row.insertCell().textContent = categoria.descricao || 'Sem descrição'; // Exibir "Sem descrição" se o campo estiver vazio
        const actionsCell = row.insertCell();
        actionsCell.innerHTML = `
            <button onclick="mostrarFormularioEditar(${categoria.categoriaId}, '${categoria.nome}', '${categoria.descricao || ''}')">Editar</button>
            <button onclick="excluirCategoria(${categoria.categoriaId})">Excluir</button>
        `;
    });
}

// Função para adicionar categoria
async function adicionarCategoria() {
    const nome = document.getElementById('nomeCategoria').value.trim();
    const descricao = document.getElementById('descricaoCategoria').value.trim();

    // Validação de campos
    if (!nome || !descricao) {
        alert('Todos os campos são obrigatórios.');
        return;
    }

    try {
        const response = await fetch('/api/Categorias', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ nome, descricao })
        });
        if (response.ok) {
            alert('Categoria adicionada com sucesso.');
            fecharFormularioAdicionar();
            carregarCategorias();
        } else {
            const error = await response.json();
            alert('Erro ao adicionar categoria: ' + JSON.stringify(error.errors));
        }
    } catch (error) {
        alert('Erro ao conectar com o servidor: ' + error.message);
    }
}

// Função para mostrar formulário de edição
function mostrarFormularioEditar(id, nome, descricao) {
    document.getElementById('formularioEditar').style.display = 'block';
    document.getElementById('categoriaId').value = id;
    document.getElementById('editarNomeCategoria').value = nome;
    document.getElementById('editarDescricaoCategoria').value = descricao;
}

// Fechar formulário de edição
function fecharFormularioEditar() {
    document.getElementById('formularioEditar').style.display = 'none';
    document.getElementById('categoriaId').value = '';
    document.getElementById('editarNomeCategoria').value = '';
    document.getElementById('editarDescricaoCategoria').value = '';
}

// Função para atualizar categoria
async function atualizarCategoria() {
    const id = document.getElementById('categoriaId').value;
    const nome = document.getElementById('editarNomeCategoria').value.trim();
    const descricao = document.getElementById('editarDescricaoCategoria').value.trim();

    // Validação de campos
    if (!nome || !descricao) {
        alert('Todos os campos são obrigatórios.');
        return;
    }

    try {
        const response = await fetch(`/api/Categorias/${id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ categoriaId: id, nome, descricao })
        });
        if (response.ok) {
            alert('Categoria atualizada com sucesso.');
            fecharFormularioEditar();
            carregarCategorias();
        } else {
            const error = await response.json();
            alert('Erro ao atualizar categoria: ' + JSON.stringify(error.errors));
        }
    } catch (error) {
        alert('Erro ao conectar com o servidor: ' + error.message);
    }
}

// Função para excluir categoria
async function excluirCategoria(id) {
    if (confirm('Tem certeza que deseja excluir esta categoria?')) {
        try {
            const response = await fetch(`/api/Categorias/${id}`, { method: 'DELETE' });
            if (response.ok) {
                alert('Categoria excluída com sucesso.');
                carregarCategorias();
            } else {
                const error = await response.text();
                alert('Erro ao excluir categoria: ' + error);
            }
        } catch (error) {
            alert('Erro ao conectar com o servidor: ' + error.message);
        }
    }
}

// Função para mostrar o formulário de adição
function mostrarFormularioAdicionar() {
    document.getElementById('formularioAdicionar').style.display = 'block';
}

// Fechar formulário de adição
function fecharFormularioAdicionar() {
    document.getElementById('formularioAdicionar').style.display = 'none';
    document.getElementById('nomeCategoria').value = '';
    document.getElementById('descricaoCategoria').value = '';
}

// Inicializar carregamento
document.addEventListener('DOMContentLoaded', carregarCategorias);
