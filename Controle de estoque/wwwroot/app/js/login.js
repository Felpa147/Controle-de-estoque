async function login() {
    const nomeUsuario = document.getElementById('loginNomeUsuario').value.trim();
    const senha = document.getElementById('loginSenha').value.trim();

    // Validações básicas no front-end
    if (!nomeUsuario || !senha) {
        alert('Por favor, preencha todos os campos.');
        return;
    }

    const loginData = {
        NomeUsuario: nomeUsuario,
        Senha: senha
    };

    try {
        const response = await fetch('/api/Usuarios/login', { // Caminho relativo para o endpoint
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(loginData)
        });

        if (response.ok) {
            const data = await response.json();
            // Armazenar o nome do usuário na sessionStorage
            sessionStorage.setItem('usuarioAutenticado', nomeUsuario);
            // Redirecionar para a página principal ou menu
            window.location.href = 'menu.html';
        } else if (response.status === 401) {
            const errorData = await response.json();
            alert(errorData.message || 'Nome de usuário ou senha inválidos.');
        } else {
            const errorData = await response.text();
            alert(`Erro ao fazer login: ${errorData}`);
        }
    } catch (error) {
        console.error('Erro ao fazer login:', error);
        alert('Ocorreu um erro ao tentar fazer login. Por favor, tente novamente mais tarde.');
    }
}

// Adicionar event listener ao botão de login após o DOM carregar
document.addEventListener('DOMContentLoaded', () => {
    const btnLogin = document.getElementById('btnLogin');
    if (btnLogin) {
        btnLogin.addEventListener('click', login);
    }
});
