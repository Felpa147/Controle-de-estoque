// Chamar verificarAutenticacao e carregarFornecedores ao carregar a página
document.addEventListener('DOMContentLoaded', () => {
    verificarAutenticacao();
    carregarFornecedores();
});

// Função para carregar os fornecedores
async function carregarFornecedores() {
    const response = await fetch(`${apiUrl}/Fornecedores`);
    if (response.ok) {
        const fornecedores = await response.json();
        exibirFornecedores(fornecedores);
    } else {
        alert('Erro ao carregar fornecedores.');
    }
}

// Função para exibir os fornecedores na tabela
function exibirFornecedores(fornecedores) {
    const tabela = document.getElementById('tabelaFornecedores');
    tabela.innerHTML = '';
    // Cabeçalho da tabela
    const header = tabela.createTHead();
    const headerRow = header.insertRow();
    ['ID', 'Nome', 'CNPJ', 'Telefone', 'Email', 'Endereço', 'Contato', 'Ações'].forEach(text => {
        const cell = headerRow.insertCell();
        cell.textContent = text;
    });
    // Corpo da tabela
    const tbody = tabela.createTBody();
    fornecedores.forEach(fornecedor => {
        const row = tbody.insertRow();
        row.insertCell().textContent = fornecedor.fornecedorId;
        row.insertCell().textContent = fornecedor.nome;
        row.insertCell().textContent = fornecedor.cnpj;
        row.insertCell().textContent = fornecedor.telefone;
        row.insertCell().textContent = fornecedor.email;
        row.insertCell().textContent = fornecedor.endereco;
        row.insertCell().textContent = fornecedor.contato;
        const actionsCell = row.insertCell();
        actionsCell.innerHTML = `
            <button onclick="mostrarFormularioEditar(${fornecedor.fornecedorId})">Editar</button>
            <button onclick="excluirFornecedor(${fornecedor.fornecedorId})">Excluir</button>
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
    document.getElementById('nomeFornecedor').value = '';
    document.getElementById('cnpjFornecedor').value = '';
    document.getElementById('telefoneFornecedor').value = '';
    document.getElementById('emailFornecedor').value = '';
    document.getElementById('enderecoFornecedor').value = '';
    document.getElementById('contatoFornecedor').value = '';
}

// Função para adicionar fornecedor
async function adicionarFornecedor() {
    const nome = document.getElementById('nomeFornecedor').value;
    const cnpj = document.getElementById('cnpjFornecedor').value;
    const telefone = document.getElementById('telefoneFornecedor').value;
    const email = document.getElementById('emailFornecedor').value;
    const endereco = document.getElementById('enderecoFornecedor').value;
    const contato = document.getElementById('contatoFornecedor').value;

    // Validações básicas
    if (!nome || !cnpj) {
        alert('Por favor, preencha os campos obrigatórios (Nome e CNPJ).');
        return;
    }

    const fornecedor = {
        Nome: nome,
        CNPJ: cnpj,
        Telefone: telefone,
        Email: email,
        Endereco: endereco,
        Contato: contato
    };

    const response = await fetch(`${apiUrl}/Fornecedores`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(fornecedor)
    });

    if (response.ok) {
        alert('Fornecedor adicionado com sucesso.');
        fecharFormularioAdicionar();
        carregarFornecedores();
    } else {
        const error = await response.text();
        alert(`Erro ao adicionar fornecedor: ${error}`);
    }
}

async function mostrarFormularioEditar(id) {
    try {
        // Obter dados do fornecedor pela API usando o ID
        const response = await fetch(`/api/Fornecedores/${id}`);
        if (!response.ok) {
            alert('Erro ao carregar fornecedor para edição.');
            return;
        }
        const fornecedor = await response.json();

        // Garantir que o modal seja exibido
        const modalEditar = document.getElementById('formularioEditar');
        modalEditar.style.display = 'block';

        // Preencher os campos do formulário com os dados do fornecedor
        document.getElementById('fornecedorId').value = fornecedor.fornecedorId;
        document.getElementById('editarNomeFornecedor').value = fornecedor.nome || '';
        document.getElementById('editarCnpjFornecedor').value = fornecedor.cnpj || '';
        document.getElementById('editarEmailFornecedor').value = fornecedor.email || '';
        document.getElementById('editarContatoFornecedor').value = fornecedor.contato || '';
        document.getElementById('editarTelefoneFornecedor').value = fornecedor.telefone || '';
        document.getElementById('editarEnderecoFornecedor').value = fornecedor.endereco || '';
    } catch (error) {
        alert('Erro ao conectar com o servidor: ' + error.message);
    }
}


// Função para fechar o formulário de edição
function fecharFormularioEditar() {
    const modalEditar = document.getElementById('formularioEditar');
    modalEditar.style.display = 'none';

    // Limpar os campos do formulário ao fechar
    document.getElementById('fornecedorId').value = '';
    document.getElementById('editarNomeFornecedor').value = '';
    document.getElementById('editarCnpjFornecedor').value = '';
    document.getElementById('editarEmailFornecedor').value = '';
    document.getElementById('editarTelefoneFornecedor').value = '';
    document.getElementById('editarEnderecoFornecedor').value = '';
}

// Função para atualizar os dados do fornecedor
async function atualizarFornecedor() {
    const id = document.getElementById('fornecedorId').value;
    const nome = document.getElementById('editarNomeFornecedor').value.trim();
    const cnpj = document.getElementById('editarCnpjFornecedor').value.trim();
    const email = document.getElementById('editarEmailFornecedor').value.trim();
    const contato = document.getElementById('editarContatoFornecedor').value.trim();
    const telefone = document.getElementById('editarTelefoneFornecedor').value.trim();
    const endereco = document.getElementById('editarEnderecoFornecedor').value.trim();

    // Validação de campos obrigatórios
    if (!nome || !cnpj || !email || !contato) {
        alert('Os campos Nome, CNPJ, Email e Contato são obrigatórios.');
        return;
    }

    try {
        // Enviar atualização para a API
        const response = await fetch(`/api/Fornecedores/${id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                fornecedorId: id,
                nome,
                cnpj,
                email,
                contato,
                telefone,
                endereco
            })
        });

        if (response.ok) {
            alert('Fornecedor atualizado com sucesso.');
            fecharFormularioEditar();
            carregarFornecedores(); // Atualizar a tabela de fornecedores
        } else {
            const error = await response.json();
            alert('Erro ao atualizar fornecedor: ' + JSON.stringify(error));
        }
    } catch (error) {
        alert('Erro ao conectar com o servidor: ' + error.message);
    }
}

// Função para excluir fornecedor
async function excluirFornecedor(id) {
    if (confirm('Tem certeza que deseja excluir este fornecedor?')) {
        const response = await fetch(`${apiUrl}/Fornecedores/${id}`, {
            method: 'DELETE'
        });
        if (response.ok) {
            alert('Fornecedor excluído com sucesso.');
            carregarFornecedores();
        } else {
            const error = await response.text();
            alert(`Erro ao excluir fornecedor: ${error}`);
        }
    }
}
