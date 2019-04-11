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


