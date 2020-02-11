CREATE TABLE [dbo].[TblFattureAcquisto](
	[id_fatture_acquisto] [bigint] IDENTITY(1,1) NOT NULL,
	[id_fornitore] [int] NOT NULL,
	[numero] [int] NOT NULL,
	[data] [date] NOT NULL,
	[is_nota_di_credito] [bit] NOT NULL,
	[imponibile] [float] NOT NULL,
	[iva] [int] NOT NULL,
	[ritenuta_acconto] [float] NOT NULL,
	[reverse_charge] [bit] NOT NULL,
 CONSTRAINT [PK_FattureAcquisto] PRIMARY KEY CLUSTERED 
(
	[id_fatture_acquisto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TblFattureAcquisto]  WITH CHECK ADD  CONSTRAINT [FK_TblFattureAcquisto_TblForitori] FOREIGN KEY([id_fornitore])
REFERENCES [dbo].[TblForitori] ([IdFornitori])
GO

ALTER TABLE [dbo].[TblFattureAcquisto] CHECK CONSTRAINT [FK_TblFattureAcquisto_TblForitori]
GO

CREATE TABLE [dbo].[TblBollette](
	[id_bollette] [bigint] IDENTITY(1,1) NOT NULL,
	[id_fornitori] [int] NOT NULL,
	[data_scadenza] [date] NULL,
	[data_pagamento] [date] NULL,
	[totale_bolletta] [decimal](10, 2) NULL,
 CONSTRAINT [PK_TblBollette] PRIMARY KEY CLUSTERED 
(
	[id_bollette] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TblBollette]  WITH CHECK ADD  CONSTRAINT [FK_TblBollette_TblForitori] FOREIGN KEY([id_fornitori])
REFERENCES [dbo].[TblForitori] ([IdFornitori])
GO

ALTER TABLE [dbo].[TblBollette] CHECK CONSTRAINT [FK_TblBollette_TblForitori]
GO

