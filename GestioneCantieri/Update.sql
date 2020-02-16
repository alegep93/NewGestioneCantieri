ALTER TABLE TblBollette ADD progressivo int NULL
GO
ALTER TABLE TblBollette ADD data_bolletta date NULL
GO
UPDATE TblFattureAcquisto SET imponibile = imponibile * (-1) WHERE is_nota_di_credito = 1
GO