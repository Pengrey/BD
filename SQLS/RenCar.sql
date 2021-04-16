USE BD_2021;
GO

CREATE TABLE RentACar_TipoVeiculo(
    Codigo int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Designacao varchar(128) NOT NULL, ArCondicionado bit NOT NULL
);
GO

CREATE TABLE RentACar_Cliente(
    NIF int NOT NULL IDENTITY(500000000,1) PRIMARY KEY,
    Nome varchar(256) NOT NULL, Endereco varchar(1024) NOT NULL, num_carta varchar(32) NOT NULL
);
GO

CREATE TABLE RentACar_Balcao(
    NIF int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Nome varchar(256) NOT NULL, Endereco varchar(1024) NOT NULL
);
GO

CREATE TABLE RentACar_Veiculo(
    Matricula varchar(16) NOT NULL PRIMARY KEY,
    Ano int NOT NULL, Marca varchar(256) NOT NULL,
    TipoVeiculo_Codigo int NOT NULL CONSTRAINT FK_RentACar_Veiculo_RentACar_tipoVeiculo FOREIGN KEY(TipoVeiculo_Codigo) REFERENCES RentACar_TipoVeiculo (Codigo)
);
GO

CREATE TABLE RentACar_Aluguer(
    Numero int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Duracao int NOT NULL, [Data] datetime NOT NULL,
    Cliente_NIF int NOT NULL CONSTRAINT FK_RentACar_Aluguer_RentACar_Cliente FOREIGN KEY(TipoVeiculo_Codigo) REFERENCES RentACar_TipoVeiculo (Codigo)
);
GO

CREATE TABLE RentACar_Similariedade(
    TipoVeiculo_Codigo1 int NOT NULL CONSTRAINT FK_RentACar_Similariedade_RentACar_TipoVeiculo1 FOREIGN KEY(TipoVeiculo_Codigo1) REFERENCES RentACar_TipoVeiculo (Codigo),
    TipoVeiculo_Codigo2 int NOT NULL CONSTRAINT FK_RentACar_Similariedade_RentACar_TipoVeiculo2 FOREIGN KEY(TipoVeiculo_Codigo2) REFERENCES RentACar_TipoVeiculo (Codigo),
    PRIMARY KEY (TipoVeiculo_Codigo1, TipoVeiculo_Codigo2)
);
GO

CREATE TABLE RentACar_Ligeiro(
    Codigo int NOT NULL PRIMARY KEY CONSTRAINT FK_RentACar_TipoVeiculo_RentACar_Ligeiro FOREIGN KEY(Codigo) REFERENCES RentACar_TipoVeiculo (Codigo),
    NumLugares int NOT NULL, Portas int NOT NULL, Combustivel in NOT NULL 
);
GO

CREATE TABLE RentACar_Pesado(
    Codigo int NOT NULL PRIMARY KEY CONSTRAINT FK_RentACar_TipoVeiculo_RentACar_Pesado FOREIGN KEY(Codigo) REFERENCES RentACar_TipoVeiculo (Codigo),
    Peso int NOT NULL, Passageiros int NOT NULL 
);
GO
