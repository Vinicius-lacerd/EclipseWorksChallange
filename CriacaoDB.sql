
CREATE DATABASE EclipseChallengeDB;
GO

USE EclipseChallengeDB;
GO

CREATE TABLE Usuario (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    DataCriacao DATETIME NOT NULL DEFAULT GETDATE(),
    NivelID INT NOT NULL DEFAULT 1  --1 comum - 2 adm
);
GO

CREATE TABLE Projeto (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    Descricao NVARCHAR(500) NULL,
    UsuarioId INT NOT NULL,
    DataCriacao DATETIME NOT NULL DEFAULT GETDATE(),
    DataFinalizacao DATETIME NULL,
    StatusProjeto INT NOT NULL DEFAULT 0, -- 0 pendente, 1 em andamento, 2 concluido, 3 cancelado
    
    CONSTRAINT FK_projetos_usuario FOREIGN KEY (UsuarioId) 
        REFERENCES Usuario(ID) ON DELETE CASCADE
);
GO

CREATE TABLE Tarefa (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    Descricao NVARCHAR(MAX) NULL,
    DataCriacao DATETIME NOT NULL DEFAULT GETDATE(),
    DataVencimento DATETIME NULL,
	DataConclusao DATETIME NOT NULL,
    StatusTarefa INT NOT NULL DEFAULT 0, -- 0 pendente, 1 em andamento, 2 concl, 3 cancelada
	Prioridade INT NOT NULL DEFAULT 2, -- 1=alta, 2=média, 3=baixa
    ProjetoId INT NOT NULL,
    UsuarioResponsavelId INT NOT NULL,
    
    CONSTRAINT FK_tarefas_projeto FOREIGN KEY (ProjetoId) 
        REFERENCES Projeto(id) ON DELETE CASCADE,
    CONSTRAINT FK_tarefas_responsavel FOREIGN KEY (UsuarioResponsavelId) 
        REFERENCES Usuario(id),
);
GO

CREATE TABLE HistoricoTarefa (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TarefaId INT NOT NULL,
    UsuarioId INT NOT NULL,
    Comentario NVARCHAR(MAX) NULL,
    DataAtualizacao DATETIME NOT NULL DEFAULT GETDATE(),
    CampoAlterado NVARCHAR(50) NULL, 
    ValorAnterior NVARCHAR(MAX) NULL,
    NovoValor NVARCHAR(MAX) NULL,
    
    CONSTRAINT FK_historico_tarefa FOREIGN KEY (TarefaId) 
        REFERENCES Tarefa(id) ON DELETE CASCADE,
    CONSTRAINT FK_historico_usuario FOREIGN KEY (UsuarioID) 
        REFERENCES Usuario(id)
);
GO

CREATE TABLE NiveisUsuario (
    ID INT PRIMARY KEY,
    Nome NVARCHAR(50) NOT NULL,
    Descricao NVARCHAR(200) NULL,
    Permissoes NVARCHAR(MAX) NULL
);
GO

ALTER TABLE Usuario
ADD CONSTRAINT FK_usuario_nivel FOREIGN KEY (NivelID) 
    REFERENCES NiveisUsuario(id);
GO

INSERT INTO NiveisUsuario (ID, Nome, Descricao) VALUES
(1, 'Usuário Base', 'Acesso basico ao sistema'),
(2, 'Gerente', 'Funcionalidades intermediarias'),
(3, 'Administrador', 'Acesso completo ao sistema');
GO

CREATE INDEX IX_tarefas_projeto ON Tarefa(ProjetoId);
CREATE INDEX IX_tarefas_vencimento ON Tarefa(DataVencimento);
CREATE INDEX IX_projetos_usuario ON Projeto(UsuarioId);
CREATE INDEX IX_historico_tarefa ON HistoricoTarefa(TarefaId);
CREATE INDEX IX_Niveis_Usuario ON NiveisUsuario(ID);
GO
