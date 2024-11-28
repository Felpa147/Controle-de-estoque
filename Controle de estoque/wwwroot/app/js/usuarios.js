// Variáveis globais
let usuarios = [];
let usuarioEditando = null;

// Chamar verificarAutenticacao e carregarUsuarios ao carregar a página
document.addEventListener('DOMContentLoaded', () => {
    verificarAutenticacao();
    carregarUsuarios();
});

// Função para carregar os usuários
async function carregarUsuarios() {
    const response = await fetch(`${apiUrl}/Usuarios`);
    if (response.ok) {
        usuarios = await response.json();
        exibirUsuarios(usuarios);
    } else {
        alert('Erro ao carregar usuários.');
    }
}

// Função para exibir os usuários na tabela
function exibirUsuarios(listaUsuarios) {
    const tabela = document.getElementById('tabelaUsuarios');
    tabela.innerHTML = '';
    // Cabeçalho da tabela
    const header = tabela.createTHead();
    const headerRow = header.insertRow();
    ['ID', 'Nome', 'Email', 'Login', 'Ações'].forEach(text => {
        const cell = headerRow.insertCell();
        cell.textContent = text;
    });
    // Corpo da tabela
    const tbody = tabela.createTBody();
    listaUsuarios.forEach(usuario => {
        const row = tbody.insertRow();
        row.insertCell().textContent = usuario.usuarioId;

        const cellNome = row.insertCell();
        cellNome.textContent = usuario.nome;
        cellNome.classList.add('truncate');
        cellNome.title = usuario.nome;

        const cellEmail = row.insertCell();
        cellEmail.textContent = usuario.email;
        cellEmail.classList.add('truncate');
        cellEmail.title = usuario.email;

        row.insertCell().textContent = usuario.login;
        const actionsCell = row.insertCell();
        actionsCell.innerHTML = `
            <button onclick="mostrarFormularioEditar(${usuario.usuarioId})">Editar</button>
            <button onclick="excluirUsuario(${usuario.usuarioId})">Excluir</button>
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
    document.getElementById('nomeUsuario').value = '';
    document.getElementById('emailUsuario').value = '';
    document.getElementById('loginUsuario').value = '';
    document.getElementById('senhaUsuario').value = '';
}

// Função para adicionar usuário
async function adicionarUsuario() {
    const nome = document.getElementById('nomeUsuario').value;
    const email = document.getElementById('emailUsuario').value;
    const login = document.getElementById('loginUsuario').value;
    const senha = document.getElementById('senhaUsuario').value;

    // Validações básicas
    if (!nome || !email || !login || !senha) {
        alert('Por favor, preencha todos os campos.');
        return;
    }

    const usuario = {
        Nome: nome,
        Email: email,
        Login: login,
        Senha: senha
    };

    const response = await fetch(`${apiUrl}/Usuarios`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(usuario)
    });

    if (response.ok) {
        alert('Usuário adicionado com sucesso.');
        fecharFormularioAdicionar();
        carregarUsuarios();
    } else {
        const error = await response.text();
        alert(`Erro ao adicionar usuário: ${error}`);
    }
}

// Função para mostrar o formulário de editar
function mostrarFormularioEditar(usuarioId) {
    usuarioEditando = usuarioId;
    document.getElementById('formularioEditar').style.display = 'block';

    // Obter os dados do usuário
    const usuario = usuarios.find(u => u.usuarioId === usuarioId);
    if (usuario) {
        document.getElementById('editarNomeUsuario').value = usuario.nome;
        document.getElementById('editarEmailUsuario').value = usuario.email;
        document.getElementById('editarLoginUsuario').value = usuario.login;
        // Não preencha o campo de senha por questões de segurança
        document.getElementById('editarSenhaUsuario').value = '';
    }
}

// Função para fechar o formulário de editar
function fecharFormularioEditar() {
    document.getElementById('formularioEditar').style.display = 'none';
    usuarioEditando = null;
    document.getElementById('editarNomeUsuario').value = '';
    document.getElementById('editarEmailUsuario').value = '';
    document.getElementById('editarLoginUsuario').value = '';
    document.getElementById('editarSenhaUsuario').value = '';
}

// Função para editar usuário
async function editarUsuario() {
    const nome = document.getElementById('editarNomeUsuario').value;
    const email = document.getElementById('editarEmailUsuario').value;
    const login = document.getElementById('editarLoginUsuario').value;
    const senha = document.getElementById('editarSenhaUsuario').value;

    if (!nome || !email || !login) {
        alert('Por favor, preencha todos os campos obrigatórios.');
        return;
    }

    const usuario = {
        UsuarioId: usuarioEditando,
        Nome: nome,
        Email: email,
        Login: login
    };

    // Incluir a senha somente se foi preenchida
    if (senha) {
        usuario.Senha = senha;
    }

    const response = await fetch(`${apiUrl}/Usuarios/${usuarioEditando}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(usuario)
    });

    if (response.ok) {
        alert('Usuário atualizado com sucesso.');
        fecharFormularioEditar();
        carregarUsuarios();
    } else {
        const error = await response.text();
        alert(`Erro ao atualizar usuário: ${error}`);
    }
}

// Função para excluir usuário
async function excluirUsuario(usuarioId) {
    if (confirm('Tem certeza que deseja excluir este usuário?')) {
        const response = await fetch(`${apiUrl}/Usuarios/${usuarioId}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            alert('Usuário excluído com sucesso.');
            carregarUsuarios();
        } else {
            const error = await response.text();
            alert(`Erro ao excluir usuário: ${error}`);
        }
    }
}
