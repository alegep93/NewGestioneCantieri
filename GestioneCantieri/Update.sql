USE [GestioneCantieri]
GO

/****** Object:  Table [dbo].[TblMatOrdFrutGroup]    Script Date: 23/02/2019 10:50:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TblMatOrdFrutGroup](
	[IdMatOrdFrutGroup] [int] IDENTITY(1,1) NOT NULL,
	[Descrizione] [nvarchar](255) NULL,
 CONSTRAINT [PK_TblMatOrdFrutGroup] PRIMARY KEY CLUSTERED 
(
	[IdMatOrdFrutGroup] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE TblMatOrdFrut ADD IdTblMatOrdFrutGroup int NULL
GO

ALTER TABLE [dbo].[TblMatOrdFrut]  WITH CHECK ADD  CONSTRAINT [FK_TblMatOrdFrut_TblMatOrdFrutGroup] FOREIGN KEY([IdTblMatOrdFrutGroup])
REFERENCES [dbo].[TblMatOrdFrutGroup] ([IdMatOrdFrutGroup])
GO

ALTER TABLE [dbo].[TblMatOrdFrut] CHECK CONSTRAINT [FK_TblMatOrdFrut_TblMatOrdFrutGroup]
GO

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
USE [GestioneCantieri]
GO

/****** Object:  Table [dbo].[ListinoHts]    Script Date: 11/04/2019 17:08:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ListinoHts](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[codice] [nvarchar](255) NOT NULL,
	[codice_prodotto] [nvarchar](255) NOT NULL,
	[descrizione] [nvarchar](255) NOT NULL,
	[prezzo] [decimal](18, 2) NOT NULL,
	[cr] [nvarchar](5) NULL,
	[g] [nvarchar](5) NULL,
	[note_disponibilita] [nvarchar](255) NULL,
 CONSTRAINT [PK_ListinoHts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Campo Non Riscuotibile
ALTER TABLE TblCantieri ADD NonRiscuotibile bit NOT NULL DEFAULT(0)

-- Aggiornamento codRiferCant
UPDATE A
SET A.codRiferCant = UPPER(CONCAT((A.Numero-1) + LEN(A.descricodcant), SUBSTRING(A.descricodcant, 1, 2), SUBSTRING(CONVERT(nvarchar, A.anno), 3, 2), SUBSTRING(B.RagSocCli, 1, 2)))
FROM TblCantieri as A
INNER JOIN TblClienti AS B ON A.IdTblClienti = B.IdCliente

-- Gestione Fatture
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TblAmministratori](
	[id_amministratori] [bigint] IDENTITY(1,1) NOT NULL,
	[nome] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_TblAmministratori] PRIMARY KEY CLUSTERED 
(
	[id_amministratori] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TblFatture](
	[id_fatture] [bigint] IDENTITY(1,1) NOT NULL,
	[id_clienti] [int] NOT NULL,
	[id_amministratori] [bigint] NULL,
	[numero] [int] NOT NULL,
	[data] [date] NOT NULL,
	[riscosso] [bit] NOT NULL,
	[imponibile] [float] NOT NULL,
	[iva] [int] NOT NULL,
	[ritenuta_acconto] [int] NOT NULL,
	[reverse_charge] [bit] NOT NULL,
	[is_nota_di_credito] [bit] NOT NULL,
 CONSTRAINT [PK_TblFatture] PRIMARY KEY CLUSTERED 
(
	[id_fatture] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TblFatture] ADD  CONSTRAINT [DF_TblFatture_riscosso]  DEFAULT ((0)) FOR [riscosso]
GO

ALTER TABLE [dbo].[TblFatture] ADD  CONSTRAINT [DF_TblFatture_reverse_charge]  DEFAULT ((0)) FOR [reverse_charge]
GO

ALTER TABLE [dbo].[TblFatture] ADD  CONSTRAINT [DF_TblFatture_is_nota_di_credito]  DEFAULT ((0)) FOR [is_nota_di_credito]
GO

ALTER TABLE [dbo].[TblFatture]  WITH CHECK ADD  CONSTRAINT [FK_TblFatture_TblAmministratori] FOREIGN KEY([id_amministratori])
REFERENCES [dbo].[TblAmministratori] ([id_amministratori])
GO

ALTER TABLE [dbo].[TblFatture] CHECK CONSTRAINT [FK_TblFatture_TblAmministratori]
GO

ALTER TABLE [dbo].[TblFatture]  WITH CHECK ADD  CONSTRAINT [FK_TblFatture_TblClienti] FOREIGN KEY([id_clienti])
REFERENCES [dbo].[TblClienti] ([IdCliente])
GO

ALTER TABLE [dbo].[TblFatture] CHECK CONSTRAINT [FK_TblFatture_TblClienti]
GO


CREATE TABLE [dbo].[TblFattureAcconti](
	[id_fatture_acconti] [bigint] IDENTITY(1,1) NOT NULL,
	[id_fatture] [bigint] NOT NULL,
	[valore_acconto] [float] NOT NULL,
 CONSTRAINT [PK_TblFattureAcconti] PRIMARY KEY CLUSTERED 
(
	[id_fatture_acconti] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TblFattureAcconti]  WITH CHECK ADD  CONSTRAINT [FK_TblFattureAcconti_TblFatture] FOREIGN KEY([id_fatture])
REFERENCES [dbo].[TblFatture] ([id_fatture])
GO

ALTER TABLE [dbo].[TblFattureAcconti] CHECK CONSTRAINT [FK_TblFattureAcconti_TblFatture]
GO

CREATE TABLE [dbo].[TblFattureCantieri](
	[id_fatture_cantieri] [bigint] IDENTITY(1,1) NOT NULL,
	[id_fatture] [bigint] NOT NULL,
	[id_cantieri] [int] NULL,
 CONSTRAINT [PK_TblFattureCantieri] PRIMARY KEY CLUSTERED 
(
	[id_fatture_cantieri] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TblFattureCantieri]  WITH CHECK ADD  CONSTRAINT [FK_TblFattureCantieri_TblCantieri] FOREIGN KEY([id_cantieri])
REFERENCES [dbo].[TblCantieri] ([IdCantieri])
GO

ALTER TABLE [dbo].[TblFattureCantieri] CHECK CONSTRAINT [FK_TblFattureCantieri_TblCantieri]
GO

ALTER TABLE [dbo].[TblFattureCantieri]  WITH CHECK ADD  CONSTRAINT [FK_TblFattureCantieri_TblFatture] FOREIGN KEY([id_fatture])
REFERENCES [dbo].[TblFatture] ([id_fatture])
GO

ALTER TABLE [dbo].[TblFattureCantieri] CHECK CONSTRAINT [FK_TblFattureCantieri_TblFatture]
GO