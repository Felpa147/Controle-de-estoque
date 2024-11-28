document.addEventListener("DOMContentLoaded", () => {
    carregarUsuario();
    configurarBotoes();
});

function carregarUsuario() {
    const usuarioAutenticado = sessionStorage.getItem("usuarioAutenticado");
    if (!usuarioAutenticado) {
        alert("Você precisa estar logado para acessar essa página.");
        window.location.href = "login.html";
    } else {
        document.getElementById("userName").textContent = usuarioAutenticado;
    }
}

function configurarBotoes() {
    const btnLogout = document.getElementById("btnLogout");
    btnLogout.addEventListener("click", () => {
        sessionStorage.removeItem("usuarioAutenticado");
        window.location.href = "login.html";
    });
}

function navigateTo(page) {
    window.location.href = page;
}
