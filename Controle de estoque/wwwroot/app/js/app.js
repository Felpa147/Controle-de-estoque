// URL da API
window.apiUrl = '/api';

function verificarAutenticacao() {
    const usuarioAutenticado = sessionStorage.getItem('usuarioAutenticado');
    if (!usuarioAutenticado) {
        window.location.href = 'login.html';
    }
}
// Função para fazer logout
function logout() {
    sessionStorage.removeItem('usuarioAutenticado');
    window.location.href = 'index.html';
}

// Adicionar event listener ao botão de logout
const btnLogout = document.getElementById('btnLogout');
if (btnLogout) {
    btnLogout.addEventListener('click', logout);
}

// Função para verificar se o usuário está autenticado
function estaAutenticado() {
    return sessionStorage.getItem('usuarioAutenticado') !== null;
}

// Função para verificar a autenticação
function verificarAutenticacao() {
    if (!estaAutenticado()) {
        window.location.href = 'index.html';
    } else {
        const nomeUsuario = sessionStorage.getItem('usuarioAutenticado');
        const nomeUsuarioElemento = document.getElementById('nomeUsuario');
        if (nomeUsuarioElemento) {
            nomeUsuarioElemento.textContent = nomeUsuario;
        }
    }
}

document.addEventListener('DOMContentLoaded', () => {
    const btnLogout = document.getElementById('btnLogout');
    if (btnLogout) {
        btnLogout.addEventListener('click', logout);
    }
});
