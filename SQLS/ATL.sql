CREATE TABLE ATL_Pessoa(
    N_CC                      INT                     NOT NULL           IDENTITY(30000000,1)                                   PRIMARY KEY,
    Data_nascimento           DATETIME                NOT NULL, 
    Morada                    VARCHAR(1024)           NOT NULL, 
    Telefone                  VARCHAR(15)             NOT NULL, 
    Nome                      VARCHAR(256)            NOT NULL, 
    Email                     VARCHAR(256)            NOT NULL, 
    Parentesco                VARCHAR(256)            NOT NULL
);
GO

CREATE TABLE ATL_Encarregado_edu(
    P_N_CC                    INT                     NOT NULL           PRIMARY KEY CONSTRAINT FK_ATL_Encarregado_edu_P_N_CC           FOREIGN KEY (P_N_CC)                      REFERENCES ATL_Pessoa(N_CC),
    Data_nascimento           DATETIME                NOT NULL, 
    Morada                    VARCHAR(1024)           NOT NULL, 
    Telefone                  VARCHAR(15)             NOT NULL,
    Nome                      VARCHAR(256)            NOT NULL, 
    Email                     VARCHAR(256)            NOT NULL,
    Parentesco                VARCHAR(256)            NOT NULL
);
GO

CREATE TABLE ATL_Professor(
    No_funcionario            INT                     NOT NULL           IDENTITY(1,1)											PRIMARY KEY,
    Nome                      VARCHAR(256)            NOT NULL, 
    Telefone                  VARCHAR(15)             NOT NULL, 
    Data_nascimento           DATETIME                NOT NULL, 
    Email                     VARCHAR(256)            NOT NULL, 
    Morada                    VARCHAR(1024)           NOT NULL, 
    N_CC                      INT                     NOT NULL,
);
GO

CREATE TABLE ATL_Turma(
    Identificador             INT                     NOT NULL           IDENTITY(1,1)                                   PRIMARY KEY, 
    Prof_no_funcionario       INT                     NOT NULL           CONSTRAINT FK_ATL_Turma_Prof_no_funcionario                   FOREIGN KEY (Prof_no_funcionario)          REFERENCES ATL_Professor(No_funcionario), 
    N_max_Aluno               INT                     NOT NULL, 
    Ano_letivo                VARCHAR(30)             NOT NULL, 
    Classes                   VARCHAR(15)             NOT NULL,              
    Designacao                VARCHAR(1024)           NOT NULL,
	CHECK(N_max_Aluno > 0)
);
GO

CREATE TABLE ATL_Aluno(
    N_CC                      INT                     NOT NULL           IDENTITY(30000000,1)                                  PRIMARY KEY,
    Turma_ID                  INT                     NOT NULL           CONSTRAINT FK_ATL_Aluno_Turma_ID                               FOREIGN KEY (Turma_ID)                     REFERENCES ATL_Turma(Identificador), 
    E_N_CC                    INT                     NOT NULL           CONSTRAINT FK_ATL_Aluno_E_N_CC                                 FOREIGN KEY (E_N_CC)                       REFERENCES ATL_Encarregado_edu(P_N_CC), 
    Data_nascimento           DATETIME                NOT NULL, 
    Morada                    VARCHAR(1024)           NOT NULL, 
    Nome                      VARCHAR(256)            NOT NULL, 
    Escalao                   VARCHAR(15)             NOT NULL
);
GO

CREATE TABLE ATL_Entrega_ou_levanta(
    P_N_CC                    INT                     NOT NULL           CONSTRAINT FK_ATL_Entrega_ou_levanta_P_N_CC                    FOREIGN KEY (P_N_CC)                       REFERENCES ATL_Encarregado_edu(P_N_CC),
    A_N_CC                    INT                     NOT NULL           CONSTRAINT FK_ATL_Entrega_ou_levanta_A_N_CC                    FOREIGN KEY (A_N_CC)                       REFERENCES ATL_Aluno(N_CC),
    PRIMARY KEY (P_N_CC, A_N_CC)
);
GO

CREATE TABLE ATL_Processos_fin(
    A_N_CC                    INT                     NOT NULL            PRIMARY KEY CONSTRAINT FK_ATL_Processos_fin_A_N_CC             FOREIGN KEY (A_N_CC)                      REFERENCES ATL_Aluno(N_CC),
    Desconto_familia          VARCHAR(15)             NOT NULL, 
    Pagamentos                INT                     DEFAULT 0, 
    Mensalidades              INT                     DEFAULT 0, 
    Atividades                VARCHAR(256)            NOT NULL,
	CHECK (pagamentos >= 0),
	CHECK (Mensalidades >= 0)
);
GO

CREATE TABLE ATL_Atividade(
    Identificador            INT                      NOT NULL				IDENTITY(1,1)                                            PRIMARY KEY,
    Designacao               VARCHAR(1024)            NOT NULL, 
    Custo                    INT                      DEFAULT 0,
	CHECK(custo >= 0)
);
GO

CREATE TABLE ATL_Pratica(
    Turma_ID                  INT                     NOT NULL           CONSTRAINT FK_ATL_Pratica_Turma_ID                             FOREIGN KEY (Turma_ID)                     REFERENCES ATL_Turma(Identificador),
    Ativ_ID                   INT                     NOT NULL           CONSTRAINT FK_ATL_Pratica_Ativ_ID                              FOREIGN KEY (Ativ_ID)                      REFERENCES ATL_Atividade(Identificador),
    PRIMARY KEY (Turma_ID, Ativ_ID)
);
GO

CREATE TABLE ATL_Deriva(
    A_Identificador           INT                     NOT NULL           CONSTRAINT FK_ATL_Deriva_A_Identificador                       FOREIGN KEY (A_Identificador)              REFERENCES ATL_Atividade(Identificador),
    A_N_CC                    INT                     NOT NULL           CONSTRAINT FK_ATL_Deriva_A_N_CC                                FOREIGN KEY (A_N_CC)                       REFERENCES ATL_Processos_fin(A_N_CC),
    PRIMARY KEY (A_Identificador, A_N_CC)
);
GO
