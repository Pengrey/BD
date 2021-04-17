CREATE TABLE RentACar_TipoVeiculo(
    Codigo                    INT                     NOT NULL            IDENTITY(1,1)                     PRIMARY KEY,
    Designacao                VARCHAR(128)            NOT NULL, 
    ArCondicionado            bit                     NOT NULL
);
GO

CREATE TABLE RentACar_Cliente(
    NIF                       INT                     NOT NULL            IDENTITY(500000000,1)        PRIMARY KEY,
    Nome                      VARCHAR(256)            NOT NULL, 
    Endereco                  VARCHAR(1024)           NOT NULL, 
    num_carta                 VARCHAR(32)             NOT NULL
);
GO

CREATE TABLE RentACar_Balcao(
    Numero                       INT                     NOT NULL            IDENTITY(1,1)                     PRIMARY KEY,
    Nome                      VARCHAR(256)            NOT NULL, 
    Endereco                  VARCHAR(1024)           NOT NULL
);
GO

CREATE TABLE RentACar_Veiculo(
    Matricula                 VARCHAR(16)             NOT NULL                                              PRIMARY KEY,
    Ano                       INT                     NOT NULL, 
    Marca                     VARCHAR(256)            NOT NULL,
    TipoVeiculo_Codigo        INT                     NOT NULL            CONSTRAINT FK_RentACar_Veiculo_RentACar_tipoVeiculo           FOREIGN KEY(TipoVeiculo_Codigo)            REFERENCES RentACar_TipoVeiculo (Codigo)
);
GO

CREATE TABLE RentACar_Aluguer(
    Numero                    INT                     NOT NULL            IDENTITY(1,1)                     PRIMARY KEY,
    Duracao                   INT                     NOT NULL, 
    [Data]                    DATETIME                NOT NULL,
    Cliente_NIF               INT                     NOT NULL            CONSTRAINT FK_RentACar_Aluguer_RentACar_Cliente               FOREIGN KEY(Cliente_NIF)            REFERENCES RentACar_Cliente (NIF),
    Balcao_numero             INT                     NOT NULL            CONSTRAINT FK_RentACar_Aluguer_RentACar_Balcao	            FOREIGN KEY(Balcao_numero)            REFERENCES RentACar_Balcao (numero),
    Veiculo_matricula         varchar(16)                     NOT NULL            CONSTRAINT FK_RentACar_Aluguer_RentACar_veiculo              FOREIGN KEY(Veiculo_matricula)            REFERENCES RentACar_veiculo (matricula)
	
);
GO

CREATE TABLE RentACar_Similariedade(
    TipoVeiculo_Codigo1       INT                     NOT NULL            CONSTRAINT FK_RentACar_Similariedade_RentACar_TipoVeiculo1    FOREIGN KEY(TipoVeiculo_Codigo1)           REFERENCES RentACar_TipoVeiculo (Codigo),
    TipoVeiculo_Codigo2       INT                     NOT NULL            CONSTRAINT FK_RentACar_Similariedade_RentACar_TipoVeiculo2    FOREIGN KEY(TipoVeiculo_Codigo2)           REFERENCES RentACar_TipoVeiculo (Codigo),
    PRIMARY KEY (TipoVeiculo_Codigo1, TipoVeiculo_Codigo2)
);
GO

CREATE TABLE RentACar_Ligeiro(
    Codigo                    INT                     NOT NULL            PRIMARY KEY CONSTRAINT FK_RentACar_TipoVeiculo_RentACar_Ligeiro FOREIGN KEY(Codigo)                      REFERENCES RentACar_TipoVeiculo (Codigo),
    NumLugares                INT                     NOT NULL, 
    Portas                    INT                     NOT NULL, 
    Combustivel               INT                     NOT NULL 
);
GO

CREATE TABLE RentACar_Pesado(
    Codigo                    INT                     NOT NULL            PRIMARY KEY CONSTRAINT FK_RentACar_TipoVeiculo_RentACar_Pesado FOREIGN KEY(Codigo)                       REFERENCES RentACar_TipoVeiculo (Codigo),
    Peso                      INT                     NOT NULL, 
    Passageiros               INT                     NOT NULL ,
	CHECK(Peso > 0),
	CHECK(Passageiros > 0)
);
GO
