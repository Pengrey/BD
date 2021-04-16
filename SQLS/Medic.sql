CREATE TABLE Medic_Farmacia(
    NIF INT NOT NULL PRIMARY KEY,
    Nome varchar(256) NOT NULL, Endereco varchar(1024) NOT NULL, Telefone VARCHAR(15) NOT NULL
);
GO

CREATE TABLE Medic_Medico(
    Numero_id int NOT NULL PRIMARY KEY,
    Nome varchar(256) NOT NULL, Endereco varchar(256) NOT NULL    
);
GO

CREATE TABLE Medic_Paciente(
    Numero_utente int NOT NULL PRIMARY KEY,
    Nome varchar(256) NOT NULL, Data_nascimento DATE NOT NULL, Endereco varchar(256) NOT NULL
);
GO

CREATE TABLE Medic_Precricao(
    Numero_unico int NOT NULL PRIMARY KEY,
    FOREIGN KEY (F_NIF) REFERENCES Medic_Farmacia(NIF), FOREIGN KEY (consulta_ID) REFERENCES /*TODO*/, [Data] DATE NOT NULL, Data_de_processo DATE NOT NULL  
);
GO

CREATE TABLE Medic_Consulta(
    FOREIGN KEY (P_num_unico) REFERENCES Medic_Precricao(Numero_unico), FOREIGN KEY (M_numero_id) REFERENCES Medic_Medico(Numero_id), FOREIGN KEY (P_no_utente) REFERENCES Medic_Paciente(Numero_utente)
);
GO

CREATE TABLE Medic_Farmaceutica(
    No_registo_nacional in NOT NULL PRIMARY KEY,
    Nome varchar(256) NOT NULL, Telefone VARCHAR(15) NOT NULL, Endereco varchar(256) NOT NULL 
);
GO

CREATE TABLE Medic_Farmaco(
    Formula VARCHAR(1024) NOT NULL PRIMARY KEY,
    FOREIGN KEY (P_numero_unico) REFERENCES Medic_Precricao(Numero_unico), FOREIGN KEY (F_no_registo_nacional) REFERENCES Medic_Farmaceutica(No_registo_nacional), nome_unico VARCHAR(256) NOT NULL, nome_comercial VARCHAR(256) NOT NULL    
);
GO

CREATE TABLE Medic_Vende(
    FOREIGN KEY (Farmacia_NIF) REFERENCES Medic_Farmacia(NIF), FOREIGN KEY (Farmaco_Formula) REFERENCES Medic_Farmaco(Formula),
    PRIMARY KEY (Farmacia_NIF, Farmaco_Formula) 
);
GO
