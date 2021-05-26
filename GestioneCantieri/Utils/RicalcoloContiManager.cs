using Database.DAO;
using Database.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace GestioneCantieri.Utils
{
    public class RicalcoloContiManager
    {
        public static List<MaterialiCantieri> GetMaterialiCantieri(int idCantiere)
        {
            decimal percentuale = CalcolaPercentualeTotaleMaterialiNascosti(idCantiere);
            return percentuale == -1 ? null : MaterialiCantieriDAO.GetMaterialeCantiereForRicalcoloConti(idCantiere, percentuale);
        }

        public static decimal CalcolaPercentualeTotaleMaterialiNascosti(int idCantiere)
        {
            decimal ret = -15;
            List<MaterialiCantieri> items = MaterialiCantieriDAO.GetByIdCantiere(idCantiere);
            decimal matVisibileConRicalcolo = items.Where(w => (w.Tipologia.ToUpper() == "MATERIALE" || w.Tipologia.ToUpper() == "A CHIAMATA") && w.Visibile && w.Ricalcolo && w.PzzoFinCli == 0)
                                                   .Sum(s => s.PzzoUniCantiere * (decimal)s.Qta);

            decimal matNascosto = items.Where(w => !w.Visibile).Sum(s => s.PzzoUniCantiere * (decimal)s.Qta);

            if (matVisibileConRicalcolo != 0)
            {
                ret = matNascosto * 100 / matVisibileConRicalcolo;
            }
            else if (matNascosto == 0 && matVisibileConRicalcolo == 0)
            {
                ret = 0;
            }
            else if (matNascosto != 0 && matVisibileConRicalcolo == 0)
            {
                ret = -1;
            }

            return ret;
        }

        public static PdfPTable InitializePdfTableDDT()
        {
            float[] columns = { 150f, 380f, 65f, 140f, 85f };
            PdfPTable table = new PdfPTable(5) { WidthPercentage = 100 };
            table.SetTotalWidth(columns);
            return table;
        }

        public static void GeneraPDFPerContoFinCli(Document pdfDoc, MaterialiCantieri mc, PdfPTable table, List<MaterialiCantieri> matCantList, decimal totale, int idCantiere, string tipoNote = "")
        {
            Phrase intestazione = GeneraIntestazioneContoFinCli(mc);
            Phrase dataPhrase = new Phrase("Data", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
            Phrase descriptionPhrase = new Phrase("Descrizione Codice Articolo", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
            Phrase quantityPhrase = new Phrase("Qtà", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
            Phrase unitaryPricePhrase = new Phrase("Prezzo Unitario", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
            Phrase valuePhrase = new Phrase("Valore", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));

            PdfPCell dataCell = new PdfPCell(dataPhrase);
            PdfPCell descriptionCell = new PdfPCell(descriptionPhrase);
            PdfPCell quantityCell = new PdfPCell(quantityPhrase);
            PdfPCell unitaryPriceCell = new PdfPCell(unitaryPricePhrase);
            PdfPCell valueCell = new PdfPCell(valuePhrase);

            dataCell.BorderWidth = descriptionCell.BorderWidth = quantityCell.BorderWidth = unitaryPriceCell.BorderWidth = valueCell.BorderWidth = 0;
            dataCell.BorderWidthBottom = descriptionCell.BorderWidthBottom = quantityCell.BorderWidthBottom = unitaryPriceCell.BorderWidthBottom = valueCell.BorderWidthBottom = 1;
            dataCell.BorderColorBottom = descriptionCell.BorderColorBottom = quantityCell.BorderColorBottom = unitaryPriceCell.BorderColorBottom = valueCell.BorderColorBottom = BaseColor.BLUE;
            dataCell.HorizontalAlignment = descriptionCell.HorizontalAlignment = quantityCell.HorizontalAlignment = unitaryPriceCell.HorizontalAlignment = valueCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            quantityCell.HorizontalAlignment = unitaryPriceCell.HorizontalAlignment = valueCell.HorizontalAlignment = Element.ALIGN_CENTER;

            table.AddCell(dataCell);
            table.AddCell(descriptionCell);
            table.AddCell(quantityCell);
            table.AddCell(unitaryPriceCell);
            table.AddCell(valueCell);

            int j = 0;

            foreach (MaterialiCantieri materiale in matCantList)
            {
                unitaryPricePhrase = new Phrase($"{Convert.ToDecimal(materiale.PzzoFinCli):n}", FontFactory.GetFont("Arial", 10, BaseColor.BLACK));
                valuePhrase = new Phrase($"{Convert.ToDecimal(materiale.Valore):n}", FontFactory.GetFont("Arial", 10, BaseColor.BLACK));
                dataPhrase = new Phrase(materiale.Data.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", 10, BaseColor.BLACK));
                descriptionPhrase = new Phrase(materiale.DescriCodArt, FontFactory.GetFont("Arial", 10, BaseColor.BLACK));
                quantityPhrase = new Phrase(materiale.Qta.ToString(), FontFactory.GetFont("Arial", 10, BaseColor.BLACK));
                dataCell = new PdfPCell(dataPhrase);
                descriptionCell = new PdfPCell(descriptionPhrase);
                quantityCell = new PdfPCell(quantityPhrase);
                unitaryPriceCell = new PdfPCell(unitaryPricePhrase);
                valueCell = new PdfPCell(valuePhrase);
                dataCell.BorderWidth = descriptionCell.BorderWidth = quantityCell.BorderWidth = unitaryPriceCell.BorderWidth = valueCell.BorderWidth = 0;
                quantityCell.HorizontalAlignment = unitaryPriceCell.HorizontalAlignment = valueCell.HorizontalAlignment = Element.ALIGN_CENTER;

                //Set Color of Alternating row
                if (j % 2 != 0)
                {
                    dataCell.BackgroundColor = descriptionCell.BackgroundColor = quantityCell.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F7F7F7"));
                    unitaryPriceCell.BackgroundColor = valueCell.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F7F7F7"));
                }
                table.AddCell(dataCell);
                table.AddCell(descriptionCell);
                table.AddCell(quantityCell);
                table.AddCell(unitaryPriceCell);
                table.AddCell(valueCell);

                if (tipoNote != "")
                {
                    // Aggiunta della riga contenente le note in base alla scelta della tipoNote
                    if (tipoNote == "note1note2")
                    {
                        PdfPCell note1Cell = new PdfPCell(new Phrase(materiale.Note, FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.ITALIC, BaseColor.BLACK)));
                        PdfPCell note2Cell = new PdfPCell(new Phrase(materiale.Note2, FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.ITALIC, BaseColor.BLACK)));
                        note1Cell.Colspan = note2Cell.Colspan = 5;
                        note1Cell.BorderWidth = note2Cell.BorderWidth = 0;
                        note1Cell.HorizontalAlignment = note2Cell.HorizontalAlignment = 0;
                        table.AddCell(note1Cell);
                        table.AddCell(note2Cell);
                    }
                    else if (tipoNote != "noNote")
                    {
                        if (materiale.Note != "" && materiale.Note != null)
                        {
                            PdfPCell noteCell = null;
                            if (tipoNote == "note1")
                            {
                                noteCell = new PdfPCell(new Phrase(materiale.Note, FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.ITALIC, BaseColor.BLACK)));
                            }
                            else if (tipoNote == "note2")
                            {
                                noteCell = new PdfPCell(new Phrase(materiale.Note2, FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.ITALIC, BaseColor.BLACK)));
                            }
                            noteCell.Colspan = 5;
                            noteCell.BorderWidth = 0;
                            noteCell.HorizontalAlignment = 0;
                            table.AddCell(noteCell);
                        }
                    }
                }
                totale += Convert.ToDecimal(materiale.Valore);
                j++;
            }

            PdfPTable tblTotali = new PdfPTable(1) { WidthPercentage = 100 };
            GeneraTotalePerContoFinCli(tblTotali, totale, idCantiere);
            pdfDoc.Add(new Paragraph(""));
            pdfDoc.Add(intestazione);
            pdfDoc.Add(table);
            pdfDoc.Add(new Paragraph(""));
            pdfDoc.Add(tblTotali);
            InitializePdfTableDDT();
        }

        public static Phrase GeneraIntestazioneContoFinCli(MaterialiCantieri mc)
        {
            string codCant = $"Codice Cantiere: {mc.CodCant}";
            string descriCodCant = $"Descrizione Codice Cantiere: {mc.DescriCodCant}";
            string intestazioneObj = $"{codCant}    -    {descriCodCant}";
            Phrase intestazione = new Phrase(intestazioneObj, FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.ITALIC, BaseColor.BLACK));
            return intestazione;
        }

        public static void GeneraTotalePerContoFinCli(PdfPTable tblTotali, decimal totale, int idCantiere)
        {
            decimal totValAcconti = PagamentiDAO.GetAll().Where(w => w.IdTblCantieri == Convert.ToInt32(idCantiere)).ToList().Sum(s => s.Imporo);

            //Totale No Iva
            Phrase totContoFinCli = new Phrase($"Totale: {totale:n}", FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.ITALIC, BaseColor.BLACK));
            PdfPCell totContoFinCliCell = new PdfPCell(totContoFinCli);

            //Totale Acconti
            Phrase totAcconti = new Phrase($"Totale Acconti: {totValAcconti:n}", FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.ITALIC, BaseColor.BLACK));
            PdfPCell totAccontiCell = new PdfPCell(totAcconti);
            Phrase totaleFinale = new Phrase($"Totale Finale Escluso IVA: {totale - totValAcconti:n}", FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.ITALIC, BaseColor.BLACK));
            PdfPCell totaleFinaleCell = new PdfPCell(totaleFinale);

            totContoFinCliCell.HorizontalAlignment = totAccontiCell.HorizontalAlignment = totaleFinaleCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            totContoFinCliCell.BorderWidth = totAccontiCell.BorderWidth = totaleFinaleCell.BorderWidth = 0;
            totContoFinCliCell.BorderWidthTop = 1;
            totContoFinCliCell.BorderColorTop = BaseColor.BLUE;
            totaleFinaleCell.PaddingBottom = 20;

            tblTotali.AddCell(totContoFinCliCell);
            tblTotali.AddCell(totAccontiCell);
            tblTotali.AddCell(totaleFinaleCell);
        }

        public static string GetStringFromListForQuery(List<int> list)
        {
            // Converte una lista di int in una stringa formattata per fare una query
            string ret = "";
            foreach (int id in list)
            {
                ret += ret == "" ? $"'{id}'" : $", '{id}'";
            }
            return ret;
        }
    }
}