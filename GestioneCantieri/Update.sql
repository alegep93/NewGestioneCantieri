CREATE TABLE [dbo].[TblDefaultMatOrdFrut](
	[IdTblDefaultMatOrdFrut] [bigint] IDENTITY(1,1) NOT NULL,
	[IdGruppiFrutti] [int] NULL,
	[IdLocale] [int] NULL,
	[IdFrutto] [int] NULL,
	[QtaFrutti] [int] NULL,
	[IdSerie] [bigint] NULL,
 CONSTRAINT [PK_TblDefaultMatOrdFrut] PRIMARY KEY CLUSTERED 
(
	[IdTblDefaultMatOrdFrut] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


ALTER TABLE [dbo].[TblMatOrdFrut]  DROP [FK_TblMatOrdFrut_TblSerie]


-- TODO
ALTER TABLE TblCantieri ADD id_preventivo int null