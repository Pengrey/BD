create table artigo(
	No_registo			int				not null			IDENTITY(1,1),
	titulo				varchar(256)	not null,
	Primary key (no_registo)
);



create table Instituicao(
	endereco			varchar(1024)	not null,
	Nome				varchar(256)	not null,
	Primary key (endereco)
);

create table Autor(
	Email				varchar(256)	not null,
	inst_endereco		varchar(1024)	not null,
	Nome				varchar(256)	not null,
	primary key	(email),
	foreign key (inst_endereco) references	instituicao(endereco)
);

create table tem(
	Auto_email			varchar(256)	not null,
	art_no_registo			int			not null,
	Primary key(auto_email,art_no_registo),
	foreign key	(auto_email) references autor(email),
	foreign key	(art_no_registo) references artigo(no_registo),

);

create table participante(
	Email				varchar(256)	not null,
	nome				varchar(256)	not null,
	morada				varchar(256)	not null,
	data_inscricao		date			not null,
	inst_endereco		varchar(1024)	not null,
	Primary key (email),
	foreign key (inst_endereco) references instituicao(endereco)

);

create table N_estudante(
	Email			varchar(256)		not null,
	ref_bancaria	varchar(16)			not null,
	Primary key (email, ref_bancaria),
	foreign key (email) references participante(email)

);

create table estudante(
	Email			varchar(256)		not null,
	inst_endereco	varchar(1024)		not null,
	Primary key (email),
	foreign key (email) references participante (email),
	foreign key (inst_endereco) references instituicao(endereco)

);