CREATE DATABASE dbcnab;
GO
USE dbcnab;
GO
IF OBJECT_ID(N'arquivo', N'U') IS NOT NULL  
   DROP TABLE [arquivo];  
GO
IF OBJECT_ID(N'tipo_transacao', N'U') IS NOT NULL  
   DROP TABLE [tipo_transacao];  
GO
CREATE TABLE [tipo_transacao](
	[id] [int] NOT NULL,
	[descricao] [varchar](30) NOT NULL,
	[natureza] [varchar](7) NOT NULL,
	[sinal] [char](1) NOT NULL,
 CONSTRAINT [PK_tipo_transacao] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [arquivo](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[tipo] [int] NOT NULL,
	[data] [date] NOT NULL,
	[valor] [decimal](15, 2) NOT NULL,
	[cpf] [varchar](11) NOT NULL,
	[cartao] [varchar](12) NOT NULL,
	[hora] [varchar](8) NOT NULL,
	[dono_loja] [varchar](14) NOT NULL,
	[nome_loja] [varchar](19) NOT NULL,
	[data_inclusao] [datetime] NOT NULL,
 CONSTRAINT [PK_importfiles] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [arquivo]  WITH CHECK ADD  CONSTRAINT [FK_arquivo_tipo_transacao] FOREIGN KEY([tipo])
REFERENCES [tipo_transacao] ([id])
GO

ALTER TABLE [arquivo] CHECK CONSTRAINT [FK_arquivo_tipo_transacao]
GO
INSERT INTO [tipo_transacao]([id],[descricao],[natureza],[sinal]) VALUES (1,'Débito','Entrada','+');
INSERT INTO [tipo_transacao]([id],[descricao],[natureza],[sinal]) VALUES (2,'Boleto','Saída','-');
INSERT INTO [tipo_transacao]([id],[descricao],[natureza],[sinal]) VALUES (3,'Financiamento','Saída','-');
INSERT INTO [tipo_transacao]([id],[descricao],[natureza],[sinal]) VALUES (4,'Crédito','Entrada','+');
INSERT INTO [tipo_transacao]([id],[descricao],[natureza],[sinal]) VALUES (5,'Recebimento Empréstimo','Entrada','+');
INSERT INTO [tipo_transacao]([id],[descricao],[natureza],[sinal]) VALUES (6,'Vendas','Entrada','+');
INSERT INTO [tipo_transacao]([id],[descricao],[natureza],[sinal]) VALUES (7,'Recebimento TED','Entrada','+');
INSERT INTO [tipo_transacao]([id],[descricao],[natureza],[sinal]) VALUES (8,'Recebimento DOC','Entrada','+');
INSERT INTO [tipo_transacao]([id],[descricao],[natureza],[sinal]) VALUES (9,'Aluguel','Saída','-');
GO