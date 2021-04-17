CREATE TABLE Medic_Farmacia(
    NIF                       INT                     NOT NULL		                                         PRIMARY KEY,
    Nome                      VARCHAR(256)            NOT NULL, 
    Endereco                  VARCHAR(1024)           NOT NULL, 
    Telefone                  VARCHAR(15)             NOT NULL,
	CHECK(NIF > 100000000)
);
GO

CREATE TABLE Medic_Medico(
    Numero_id                 INT                     NOT NULL      IDENTITY(1,1)                                         PRIMARY KEY,
    Nome                      VARCHAR(256)            NOT NULL, 
    Endereco                  VARCHAR(256)            NOT NULL    
);
GO

CREATE TABLE Medic_Paciente(
    Numero_utente             INT                     NOT NULL      IDENTITY(1,1)                                    PRIMARY KEY,
    Nome                      VARCHAR(256)            NOT NULL, 
    Data_nascimento           DATETIME                NOT NULL, 
    Endereco                  VARCHAR(256)            NOT NULL
);
GO

CREATE TABLE Medic_Precricao(
    Numero_unico              INT                     NOT NULL       IDENTITY(1,1)                                       PRIMARY KEY,
    F_NIF                     INT                     NOT NULL            CONSTRAINT FK_Medic_Prescricao_Medic_F_NIF                    FOREIGN KEY (F_NIF)                        REFERENCES Medic_Farmacia(NIF), 
    C_consulta_ID             INT                     NOT NULL            CONSTRAINT FK_Medic_Prescricao_Medic_consulta_ID              FOREIGN KEY (C_consulta_ID)                REFERENCES Medic_Consulta(consulta_ID), 
    [Data]                    DATETIME                NOT NULL, 
    Data_de_processo          DATETIME                NOT NULL  
);
GO

CREATE TABLE Medic_Consulta(
    consulta_ID               INT                     NOT NULL        IDENTITY(1,1)                                      PRIMARY KEY,
    P_num_unico               INT                     NOT NULL           CONSTRAINT FK_Medic_Consulta_Medic_P_num_unico                 FOREIGN KEY (P_num_unico)                  REFERENCES Medic_Precricao(Numero_unico), 
    M_numero_id               INT                     NOT NULL           CONSTRAINT FK_Medic_Consulta_Medic_M_numero_id                 FOREIGN KEY (M_numero_id)                  REFERENCES Medic_Medico(Numero_id), 
    P_no_utente               INT                     NOT NULL           CONSTRAINT FK_Medic_Consulta_Medic_P_no_utente                 FOREIGN KEY (P_no_utente)                  REFERENCES Medic_Paciente(Numero_utente)
);
GO

CREATE TABLE Medic_Farmaceutica(
    No_registo_nacional       INT                     NOT NULL      IDENTITY(1,1)                                        PRIMARY KEY,
    Nome                      VARCHAR(256)            NOT NULL, 
    Telefone                  VARCHAR(15)             NOT NULL, 
    Endereco                  VARCHAR(256)            NOT NULL 
);
GO

CREATE TABLE Medic_Farmaco(
    Formula                   VARCHAR(1024)           NOT NULL                                              PRIMARY KEY,
    P_numero_unico            INT                     NOT NULL          CONSTRAINT FK_Medic_Farmaco_Medic_P_numero_unico                FOREIGN KEY (P_numero_unico)               REFERENCES Medic_Precricao(Numero_unico), 
    F_no_registo_nacional     INT                     NOT NULL          CONSTRAINT FK_Medic_Farmaco_Medic_F_no_registo_nacional         FOREIGN KEY (F_no_registo_nacional)        REFERENCES Medic_Farmaceutica(No_registo_nacional), 
    nome_unico                VARCHAR(256)            NOT NULL, 
    nome_comercial            VARCHAR(256)            NOT NULL    
);
GO

CREATE TABLE Medic_Vende(
    Farmacia_NIFNIF           INT                     NOT NULL          CONSTRAINT FK_Medic_Vende_Medic_Farmacia_NIF                    FOREIGN KEY (Farmacia_NIF)                 REFERENCES Medic_Farmacia(NIF), 
    Farmaco_Formula           VARCHAR(1024)           NOT NULL          CONSTRAINT FK_Medic_Vende_Medic_Farmaco_Formula                 FOREIGN KEY (Farmaco_Formula)              REFERENCES Medic_Farmaco(Formula),
    PRIMARY KEY (Farmacia_NIF, Farmaco_Formula) 
);
GO
