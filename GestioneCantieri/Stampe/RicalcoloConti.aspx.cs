using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using GestioneCantieri.Utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class RicalcoloConti : System.Web.UI.Page
    {
        public static decimal totRicalcoloConti = 0m;
        public string idCant = "";
        public decimal percentuale = 0.00m;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnStampaContoCliente.Visible = false;
                FillDdlScegliCantiere();
            }
        }

        #region Helpers
        public decimal CalcolaPercentualeTotaleMaterialiNascosti()
        {
            decimal matVisibileConRicalcolo = MaterialiCantieriDAO.TotaleVisibile(idCant);
            decimal matNascosto = MaterialiCantieriDAO.TotaleNascosto(idCant);

            if (matVisibileConRicalcolo != 0)
            {
                percentuale = ((matNascosto * 100) / matVisibileConRicalcolo);
            }
            else if (matNascosto == 0 && matVisibileConRicalcolo == 0)
            {
                percentuale = 0;
            }
            else if (matNascosto != 0 && matVisibileConRicalcolo == 0)
            {
                percentuale = -1;
            }

            return percentuale;
        }

        protected void FillDdlScegliCantiere()
        {
            ddlScegliCant.Items.Clear();
            ddlScegliCant.Items.Add(new System.Web.UI.WebControls.ListItem("", "-1"));
            DropDownListManager.FillDdlCantieri(CantieriDAO.GetCantieri(txtAnno.Text, txtCodCant.Text, "", chkChiuso.Checked, chkRiscosso.Checked), ref ddlScegliCant);
        }
        public List<MaterialiCantieri> GetMaterialiCantieri()
        {
            decimal perc = CalcolaPercentualeTotaleMaterialiNascosti();

            if (perc == -1)
            {
                return null;
            }
            else
            {
                return MaterialiCantieriDAO.GetMaterialeCantiereForRicalcoloConti(idCant, perc);
            }
        }

        public void BindGridExcel(GridView grd, GridView grdExcel)
        {
            grdExcel.DataSource = grd.DataSource;
            grdExcel.DataBind();

            //Imposto la colonna del valore
            for (int i = 0; i < grd.Rows.Count; i++)
            {
                grdExcel.Rows[i].Cells[0].Text = grd.Rows[i].Cells[0].Text;
                grdExcel.Rows[i].Cells[1].Text = grd.Rows[i].Cells[1].Text;
                grdExcel.Rows[i].Cells[2].Text = grd.Rows[i].Cells[2].Text;
                grdExcel.Rows[i].Cells[3].Text = grd.Rows[i].Cells[6].Text;
                grdExcel.Rows[i].Cells[4].Text = grd.Rows[i].Cells[7].Text;
                grdExcel.Rows[i].Cells[5].Text = grd.Rows[i].Cells[11].Text;
                grdExcel.Rows[i].Cells[6].Text = grd.Rows[i].Cells[12].Text;
            }
        }
        protected decimal CalcolaTotAcconti()
        {
            decimal totAcconti = 0m;
            List<Pagamenti> pagList = PagamentiDAO.GetAll().Where(w => w.IdTblCantieri == Convert.ToInt32(idCant)).ToList();

            foreach (Pagamenti p in pagList)
            {
                totAcconti += p.Imporo;
            }

            return totAcconti;
        }
        protected void CreateExcel()
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=RicalcoloConti-" + ddlScegliCant.SelectedItem.Text + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

            htmlWrite.WriteLine("<strong><font size='4'>" + ddlScegliCant.SelectedItem.Text + "</font></strong>");

            // viene reindirizzato il rendering verso la stringa in uscita
            grdStampaMateCantExcel.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());

            Response.End();
        }
        #endregion

        #region Stampa PDF
        //Stampa PDF
        public PdfPTable InitializePdfTableDDT()
        {
            float[] columns = { 150f, 380f, 65f, 140f, 85f };
            PdfPTable table = new PdfPTable(5);
            table.WidthPercentage = 100;
            table.SetTotalWidth(columns);

            return table;
        }
        public void ExportToPdfPerContoFinCli(List<MaterialiCantieri> matCantList)
        {
            decimal totale = 0m;
            Cantieri cant = CantieriDAO.GetSingle(Convert.ToInt32(idCant));
            MaterialiCantieri mc = new MaterialiCantieri
            {
                RagSocCli = cant.RagSocCli,
                CodCant = cant.CodCant,
                DescriCodCant = cant.DescriCodCant
            };

            //Apro lo stream verso il file PDF
            Document pdfDoc = new Document(PageSize.A4, 8f, 2f, 2f, 2f);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            PdfPTable table = InitializePdfTableDDT();

            Phrase title = new Phrase("Ragione Sociale Cliente: " + mc.RagSocCli, FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
            pdfDoc.Add(title);

            GeneraPDFPerContoFinCli(pdfDoc, mc, table, matCantList, totale);

            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + mc.RagSocCli + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
        }
        public void GeneraPDFPerContoFinCli(Document pdfDoc, MaterialiCantieri mc, PdfPTable table, List<MaterialiCantieri> matCantList, decimal totale)
        {
            Phrase intestazione = GeneraIntestazioneContoFinCli(mc);
            Phrase dataPhrase = new Phrase(Server.HtmlDecode("Data"), FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
            Phrase descriptionPhrase = new Phrase(Server.HtmlDecode("Descrizione Codice Articolo"), FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
            Phrase quantityPhrase = new Phrase(Server.HtmlDecode("Qtà"), FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
            Phrase unitaryPricePhrase = new Phrase(Server.HtmlDecode("Prezzo Unitario"), FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
            Phrase valuePhrase = new Phrase(Server.HtmlDecode("Valore"), FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));

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
                unitaryPricePhrase = new Phrase(Server.HtmlDecode(String.Format("{0:n}", Convert.ToDecimal(materiale.PzzoFinCli)).ToString()), FontFactory.GetFont("Arial", 10, BaseColor.BLACK));
                valuePhrase = new Phrase(Server.HtmlDecode(String.Format("{0:n}", Convert.ToDecimal(materiale.Valore)).ToString()), FontFactory.GetFont("Arial", 10, BaseColor.BLACK));
                dataPhrase = new Phrase(Server.HtmlDecode(materiale.Data.ToString("dd/MM/yyyy")), FontFactory.GetFont("Arial", 10, BaseColor.BLACK));
                descriptionPhrase = new Phrase(Server.HtmlDecode(materiale.DescriCodArt), FontFactory.GetFont("Arial", 10, BaseColor.BLACK));
                quantityPhrase = new Phrase(Server.HtmlDecode(materiale.Qta.ToString()), FontFactory.GetFont("Arial", 10, BaseColor.BLACK));
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

                if (ddlScegliTipoNote != null)
                {
                    // Aggiunta della riga contenente le note in base alla scelta della DDLSCegliTipoNote
                    if (ddlScegliTipoNote.SelectedValue == "note1note2")
                    {
                        PdfPCell note1Cell = null;
                        PdfPCell note2Cell = null;

                        note1Cell = new PdfPCell(new Phrase(materiale.Note, FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.ITALIC, BaseColor.BLACK)));
                        note2Cell = new PdfPCell(new Phrase(materiale.Note2, FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.ITALIC, BaseColor.BLACK)));

                        note1Cell.Colspan = note2Cell.Colspan = 5;
                        note1Cell.BorderWidth = note2Cell.BorderWidth = 0;
                        note1Cell.HorizontalAlignment = note2Cell.HorizontalAlignment = 0;

                        table.AddCell(note1Cell);
                        table.AddCell(note2Cell);
                    }
                    else if (ddlScegliTipoNote.SelectedValue != "noNote")
                    {
                        if (materiale.Note != "" && materiale.Note != null)
                        {
                            PdfPCell noteCell = null;

                            if (ddlScegliTipoNote.SelectedValue == "note1")
                            {
                                noteCell = new PdfPCell(new Phrase(materiale.Note, FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.ITALIC, BaseColor.BLACK)));
                            }
                            else if (ddlScegliTipoNote.SelectedValue == "note2")
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

            PdfPTable tblTotali = new PdfPTable(1);
            tblTotali.WidthPercentage = 100;

            GeneraTotalePerContoFinCli(tblTotali, totale);

            pdfDoc.Add(new Paragraph(""));
            pdfDoc.Add(intestazione);
            pdfDoc.Add(table);
            pdfDoc.Add(new Paragraph(""));
            pdfDoc.Add(tblTotali);

            table = InitializePdfTableDDT();
            totale = 0m;
        }
        protected Phrase GeneraIntestazioneContoFinCli(MaterialiCantieri mc)
        {
            string codCant = "Codice Cantiere: " + mc.CodCant;
            string descriCodCant = "Descrizione Codice Cantiere: " + mc.DescriCodCant;
            string intestazioneObj = codCant + "    -    " + descriCodCant;

            Phrase intestazione = new Phrase(intestazioneObj, FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.ITALIC, BaseColor.BLACK));

            return intestazione;
        }
        protected void GeneraTotalePerContoFinCli(PdfPTable tblTotali, decimal totale)
        {
            decimal totValAcconti = CalcolaTotAcconti();
            totRicalcoloConti = totale;

            //Totale No Iva
            Phrase totContoFinCli = new Phrase("Totale: " + String.Format("{0:n}", totale), FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.ITALIC, BaseColor.BLACK));
            PdfPCell totContoFinCliCell = new PdfPCell(totContoFinCli);

            //Totale Acconti
            Phrase totAcconti = new Phrase("Totale Acconti: " + String.Format("{0:n}", totValAcconti), FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.ITALIC, BaseColor.BLACK));
            PdfPCell totAccontiCell = new PdfPCell(totAcconti);
            Phrase totaleFinale = new Phrase("Totale Finale Escluso IVA: " + String.Format("{0:n}", totale - totValAcconti), FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.ITALIC, BaseColor.BLACK));
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
        #endregion

        #region Eventi Click
        /* EVENTI CLICK */
        protected void btnStampaContoCliente_Click(object sender, EventArgs e)
        {
            idCant = ddlScegliCant.SelectedItem.Value;
            List<MaterialiCantieri> matCantList = GetMaterialiCantieri();
            //ExportPDF(matCantList);

            if (percentuale == -1)
            {
                lblControlloMatVisNasc.Text = "Materiale visibile con ricalcolo = 0, ma è presente del Materiale nascosto. --- Oppure sono presenti record con PzzoFinCli.";
                lblControlloMatVisNasc.ForeColor = Color.Red;
                return;
            }
            else
            {
                ExportToPdfPerContoFinCli(matCantList);
            }
        }
        protected void btnFiltraCantieri_Click(object sender, EventArgs e)
        {
            FillDdlScegliCantiere();
        }
        protected void btnStampaExcel_Click(object sender, EventArgs e)
        {
            idCant = ddlScegliCant.SelectedItem.Value;
            GetMaterialiCantieri();
            BindGridExcel(grdStampaMateCant, grdStampaMateCantExcel);
            CreateExcel();
        }
        #endregion

        #region Eventi Text-Changed
        /* EVENTI TEXT-CHANGED */
        protected void ddlScegliCant_TextChanged(object sender, EventArgs e)
        {
            if (ddlScegliCant.SelectedIndex != 0)
            {
                btnStampaContoCliente.Visible = true;
            }
            else
            {
                btnStampaContoCliente.Visible = false;
            }
        }
        #endregion

        /* Override per il corretto funzionamento della creazione del foglio Excel */
        public override void VerifyRenderingInServerForm(Control control)
        {
            //Do nothing
        }
    }
}