--ALTER TABLE TblFattureAcquisto ADD file_path nvarchar(200) NULL
--GO

ALTER TABLE TblDDTMef ADD
FTVRF0 nvarchar(50) NULL, 
FTDT30  nvarchar(50) NULL,
FTAIN nvarchar(50) NULL,
descrizione_articolo_2 nvarchar(200) NOT NULL DEFAULT (''),
iva int NOT NULL DEFAULT (22),	
prezzo_listino decimal (10,2) NOT NULL DEFAULT(0),
data2 date NOT NULL DEFAULT(GETDATE()),
valuta nvarchar(10) NOT NULL DEFAULT('EUR'),
FTFOM nvarchar(50) NULL,
FTCMA nvarchar(50) NULL,
FTCDO nvarchar(50) NULL,
FLFLAG nvarchar(50) NULL,
FLFLQU nvarchar(50) NULL,
data3 date NOT NULL DEFAULT(GETDATE()),
FTORAG nvarchar(50) NULL,
importo2 decimal (10,2) NOT NULL DEFAULT(0),
FTIMRA nvarchar(50) NULL,
FTMLT0 nvarchar(50) NULL
GO

ALTER TABLE TblDDTMefTemp ADD
FTVRF0 nvarchar(50) NULL, 
FTDT30  nvarchar(50) NULL,
FTAIN nvarchar(50) NULL,
descrizione_articolo_2 nvarchar(200) NOT NULL DEFAULT (''),
iva int NOT NULL DEFAULT (22),	
prezzo_listino decimal (10,2) NOT NULL DEFAULT(0),
data2 date NOT NULL DEFAULT(GETDATE()),
valuta nvarchar(10) NOT NULL DEFAULT('EUR'),
FTFOM nvarchar(50) NULL,
FTCMA nvarchar(50) NULL,
FTCDO nvarchar(50) NULL,
FLFLAG nvarchar(50) NULL,
FLFLQU nvarchar(50) NULL,
data3 date NOT NULL DEFAULT(GETDATE()),
FTORAG nvarchar(50) NULL,
importo2 decimal (10,2) NOT NULL DEFAULT(0),
FTIMRA nvarchar(50) NULL,
FTMLT0 nvarchar(50) NULL
GO

USE [GestioneCantieri]
GO

/****** Object:  Table [dbo].[TblDDTMef]    Script Date: 22/03/2020 12:12:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TblDDTMefProva](
	[IdDDTMef] [int] IDENTITY(1,1) NOT NULL,
	[Anno] [int] NULL,
	[Data] [datetime] NULL,
	[N_DDT] [int] NULL,
	[CodArt] [nvarchar](50) NULL,
	[DescriCodArt] [nvarchar](50) NULL,
	[Qta] [int] NULL,
	[Importo] [money] NULL,
	[Acquirente] [nvarchar](50) NULL,
	[PrezzoUnitario] [money] NULL,
	[AnnoN_DDT] [int] NULL,
	[IdFornitore] [int] NULL,
	[FTVRF0] [nvarchar](50) NULL,
	[FTDT30] [nvarchar](50) NULL,
	[FTAIN] [nvarchar](50) NULL,[descrizione_articolo_2] [nvarchar](200) NOT NULL,
	[iva] [int] NOT NULL,
	[prezzo_listino] [decimal](10, 2) NOT NULL,
	[data2] [date] NOT NULL,
	[valuta] [nvarchar](10) NOT NULL,
	[FTFOM] [nvarchar](50) NULL,
	[FTCMA] [nvarchar](50) NULL,
	[FTCDO] [nvarchar](50) NULL,
	[FLFLAG] [nvarchar](50) NULL,
	[FLFLQU] [nvarchar](50) NULL,
	[data3] [date] NOT NULL,
	[FTORAG] [nvarchar](50) NULL,
	[importo2] [decimal](10, 2) NOT NULL,
	[FTIMRA] [nvarchar](50) NULL,
	[FTMLT0] [nvarchar](50) NULL,
 CONSTRAINT [PK_TblDDTMefProva] PRIMARY KEY CLUSTERED 
(
	[IdDDTMef] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TblDDTMefProva] ADD  DEFAULT ('') FOR [descrizione_articolo_2]
GO

ALTER TABLE [dbo].[TblDDTMefProva] ADD  DEFAULT ((22)) FOR [iva]
GO

ALTER TABLE [dbo].[TblDDTMefProva] ADD  DEFAULT ((0)) FOR [prezzo_listino]
GO

ALTER TABLE [dbo].[TblDDTMefProva] ADD  DEFAULT (getdate()) FOR [data2]
GO

ALTER TABLE [dbo].[TblDDTMefProva] ADD  DEFAULT ('EUR') FOR [valuta]
GO

ALTER TABLE [dbo].[TblDDTMefProva] ADD  DEFAULT (getdate()) FOR [data3]
GO

ALTER TABLE [dbo].[TblDDTMefProva] ADD  DEFAULT ((0)) FOR [importo2]
GO


--------------------------------------------------------- DA DBF -----------------------------------------------------------
USE [GestioneCantieri]
GO

/****** Object:  Table [dbo].[TblDDTMef]    Script Date: 22/03/2020 17:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TblDDTMefFromDbf](
	FTANNO nvarchar(200) NULL,
	FTDT nvarchar(200) NULL,
	FTNR nvarchar(200) NULL,
	FTVRF0 nvarchar(200) NULL,
	FTDT30 nvarchar(200) NULL,
	FTAFO nvarchar(200) NULL,
	FTAIN nvarchar(200) NULL,
	FTDEX1 nvarchar(200) NULL,
	FTDEX2 nvarchar(200) NULL,
	FTAIV nvarchar(200) NULL,
	FTPUN nvarchar(200) NULL,
	FTQTA nvarchar(200) NULL,
	FTPU nvarchar(200) NULL,
	FTDTC nvarchar(200) NULL,
	FTCVA nvarchar(200) NULL,
	FTFOM nvarchar(200) NULL,
	FTCMA nvarchar(200) NULL,
	FTCDO nvarchar(200) NULL,
	FLFLAG nvarchar(200) NULL,
	FLFLQU nvarchar(200) NULL,
	FTDTAG nvarchar(200) NULL,
	FTORAG nvarchar(200) NULL,
	FTTSCA nvarchar(200) NULL,
	FTIMRA nvarchar(200) NULL,
	FTMLT0 nvarchar(200) NULL
)
