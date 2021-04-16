Create table fornecedor(
	NIF						int				not null	IDENTITY(50000000,1),
	codigo_interno			int				not null	IDENTITY(1,1),
	nome				varchar(256)		not null,
	num_de_fax			varchar(32)			not null,
	endereco			varchar(1024)		not null,
	condicoes_pagamento	varChar(16)			not null,
	primary key (NIF),
	check (condicoes_pagamento = 'pronto' or condicoes_pagamento = '30 dias' or condicoes_pagamento = '60 dias')
);



Create table Encomenda(
	Num_encomenda			int				not null	IDENTITY(1,1),
	F_NIF					int				not null,
	Data_realizacao			date			not null,
	primary key (Num_encomenda),
	CONSTRAINT FK_Encomenda_Fornecedor foreign key	(F_NIF)	references fornecedor(NIF)
);

Create table Produto(
	codigo					int				not null	IDENTITY(1,1),
	quantidade				int				not null	DEFAULT 0,
	Nome				varchar(256)		not null,
	preco				decimal(10,2)		not null,
	taxa_iva			decimal(3,2)		not null,
	primary key (codigo),
	check(quantidade >= 0),
	check(preco >= 0),
	check(taxa_iva >= 0)

);

Create table contem(
	P_codigo				int				not null,
	Num_encomenda			int				not null,
	primary key (P_codigo, num_encomenda),
	CONSTRAINT FK_contem_produto foreign key(P_codigo) references produto(codigo), 
	CONSTRAINT FK_contem_Encomenda foreign key(num_encomenda) references encomenda(num_encomenda) 
);