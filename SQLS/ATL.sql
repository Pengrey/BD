CREATE TABLE ATL_Pessoa(
    N_CC int NOT NULL PRIMARY KEY,
    Data_nascimento DATE NOT NULL, Morada VARCHAR(1024) NOT NULL, Telefone VARCHAR(15) NOT NULL, Nome VARCHAR(256) NOT NULL, Email VARCHAR(256) NOT NULL, Parentesco VARCHAR(256)
);
GO

CREATE TABLE ATL_Encarregado_edu(
    FOREIGN KEY (P_N_CC) REFERENCES ATL_Pessoa(N_CC) PRIMARY KEY,
    Data_nascimento DATE NOT NULL, Morada VARCHAR(1024) NOT NULL, Telefone VARCHAR(15) NOT NULL, Nome VARCHAR(256) NOT NULL, Email VARCHAR(256) NOT NULL, Parentesco VARCHAR(256)
);
GO

CREATE TABLE ATL_Professor(
    No_funcionario INT NOT NULL PRIMARY KEY,
    Nome VARCHAR(256) NOT NULL, Telefone VARCHAR(15) NOT NULL, Data_nascimento DATE NOT NULL, Email VARCHAR(256) NOT NULL, Morada VARCHAR(1024) NOT NULL, N_CC INT NOT NULL,
);
GO

CREATE TABLE ATL_Turma(
    Identificador INT NOT NULL PRIMARY KEY,
    FOREIGN KEY (Prof_no_funcionario) REFERENCES ATL_Professor(No_funcionario), N_max_Aluno INT NOT NULL, Ano_letivo VARCHAR(30) NOT NULL, Classes VARCHAR(15), Designacao VARCHAR(1024)
);
GO

CREATE TABLE ATL_Aluno(
    N_CC INT NOT NULL PRIMARY KEY,
    FOREIGN KEY (Turma_ID) REFERENCES ATL_Turma(Identificador), FOREIGN KEY (E_N_CC) REFERENCES ATL_Encarregado_edu(P_N_CC)), Data_nascimento DATE NOT NULL, Morada VARCHAR(1024) NOT NULL, Nome VARCHAR(256) NOT NULL, Escalao varchar(15) NOT NULL
);
GO

CREATE ATL_Entrega_ou_levanta(
    FOREIGN KEY (P_N_CC) REFERENCES ATL_Encarregado_edu(P_N_CC),
    FOREIGN KEY (A_N_CC) REFERENCES ATL_Aluno(N_CC),
    PRIMARY KEY (P_N_CC, A_N_CC)
);
GO

CREATE ATL_Processos_fin(
    FOREIGN KEY (A_N_CC) REFERENCES ATL_Aluno(N_CC) PRIMARY KEY,
    Desconto_familia VARCHAR(15) NOT NULL, Pagamentos INT DEFAULT 0, Mensalidades INT DEFAULT 0, Atividades VARCHAR(256)
);
GO

CREATE ATL_Atividade(
    Identificador INT NOT NULL PRIMARY KEY,
    Designacao VARCHAR(1024), Custo INT DEFAULT 0
);
GO

CREATE ATL_Pratica(
    FOREIGN KEY (Turma_ID) REFERENCES ATL_Turma(Identificador),
    FOREIGN KEY (Ativ_ID) REFERENCES ATL_Atividade(Identificador),
    PRIMARY KEY (Turma_ID, Ativ_ID)
);
GO

CREATE ATL_Deriva(
    FOREIGN KEY (A_Identificador) REFERENCES ATL_Atividade(Identificador),
    FOREIGN KEY (A_N_CC) REFERENCES ATL_Processos_fin(A_N_CC),
    PRIMARY KEY (A_Identificador, A_N_CC)
);
GO