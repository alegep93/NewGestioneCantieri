using Database.DAO;
using Database.Models;
using GestioneCantieri.Utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class StampeVarie : Page
    {
        protected DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillAllDdl();
                pnlCampiStampaDDT_MatCant.Visible = false;
                txtDataDa.Text = new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy-MM-dd");
                txtDataA.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtDataDa.TextMode = txtDataA.TextMode = TextBoxMode.Date;
            }
        }

        #region Helpers
        /* HELPERS */
        protected void FillDdlScegliStampa()
        {
            ddlScegliStampa.Items.Clear();
            ddlScegliStampa.Items.Add(new System.Web.UI.WebControls.ListItem("", "-1"));
            StampeDAO.GetAll().ForEach(f =>
            {
                ddlScegliStampa.Items.Add(new System.Web.UI.WebControls.ListItem(f.NomeStampa, f.Id.ToString()));
            });
        }
        protected void FillDdlScegliFornitore()
        {
            ddlScegliFornitore.Items.Clear();
            ddlScegliFornitore.Items.Add(new System.Web.UI.WebControls.ListItem("", "-1"));
            DropDownListManager.FillDdlFornitore(FornitoriDAO.GetFornitori(), ref ddlScegliFornitore);
        }
        protected void FillDdlScegliAcquirente()
        {
            ddlScegliAcquirente.Items.Clear();
            ddlScegliAcquirente.Items.Add(new System.Web.UI.WebControls.ListItem("", "-1"));
            DropDownListManager.FillDdlOperaio(OperaiDAO.GetAll(), ref ddlScegliAcquirente);
        }
        protected void FillAllDdl()
        {
            FillDdlScegliStampa();
            FillDdlScegliFornitore();
            FillDdlScegliAcquirente();
        }

        //Popola la griglia con i dati da SQL
        protected void BindGridStampaDDT()
        {
            grdStampaDDT.DataSource = DDTMefDAO.GetDDTForPDF(txtDataDa.Text, txtDataA.Text, ddlScegliAcquirente.SelectedItem.Text, txtNumDDT.Text);
            grdStampaDDT.DataBind();
        }
        protected void BindGridStampaMatCant()
        {
            grdStampaMateCant.DataSource = MaterialiCantieriDAO.GetMaterialeCantiere(txtDataDa.Text, txtDataA.Text, ddlScegliAcquirente.SelectedItem.Text, ddlScegliFornitore.SelectedItem.Text, txtNumDDT.Text);
            grdStampaMateCant.DataBind();
        }
        #endregion

        #region Stampa PDF
        protected void AddPageNumber()
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];
            byte[] bytes = File.ReadAllBytes(@"C:\\Users\\" + userName + "\\Downloads\\" + txtNomeFile.Text + ".pdf");
            iTextSharp.text.Font blackFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            using (MemoryStream stream = new MemoryStream())
            {
                PdfReader reader = new PdfReader(bytes);
                using (PdfStamper stamper = new PdfStamper(reader, stream))
                {
                    int pages = reader.NumberOfPages;
                    for (int i = 1; i <= pages; i++)
                    {
                        ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, new Phrase(i.ToString() + " / " + pages.ToString(), blackFont), 570f, 15f, 0);
                    }
                }
                bytes = stream.ToArray();
            }
            File.WriteAllBytes(@"C:\\Users\\" + userName + "\\Downloads\\" + txtNomeFile.Text + ".pdf", bytes);
        }

        protected PdfPTable InitializePdfTableDDT()
        {
            float[] columns = { 150f, 220f, 340f, 100f, 150f, 120f };
            PdfPTable table = new PdfPTable(grdStampaDDT.Columns.Count) { WidthPercentage = 100 };
            table.SetTotalWidth(columns);
            return table;
        }
        protected PdfPTable InitializePdfTableMatCant()
        {
            float[] columns = { 170f, 130f, 170f, 160f, 140f, 240f, 110f, 150f, 130f };
            PdfPTable table = new PdfPTable(grdStampaMateCant.Columns.Count) { WidthPercentage = 100 };
            table.SetTotalWidth(columns);
            return table;
        }
        protected void ExportToPdfPerDDT()
        {
            decimal totale = 0m;
            decimal totaleFinale = 0m;
            long numDdtAttuale = 0;
            List<DDTMef> ddtList = DDTMefDAO.GetDDTForPDF(txtDataDa.Text, txtDataA.Text, ddlScegliAcquirente.SelectedItem.Text, txtNumDDT.Text);

            //Apro lo stream verso il file PDF
            Document pdfDoc = new Document(PageSize.A4, 8f, 2f, 0f, 10f);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            PdfPTable table = InitializePdfTableDDT();
            Phrase title = new Phrase(txtNomeFile.Text, FontFactory.GetFont("Arial", 22, iTextSharp.text.Font.BOLD, BaseColor.RED));
            pdfDoc.Add(title);

            GeneraPDFPerNumDDT(pdfDoc, ddtList, table, totale, numDdtAttuale, totaleFinale);

            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + txtNomeFile.Text + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
        }
        protected void ExportToPdfPerMatCant()
        {
            decimal totale = 0m;
            decimal totaleFinale = 0m;
            long numDdtAttuale = 0;
            List<MaterialiCantieri> matCantList = MaterialiCantieriDAO.GetMaterialeCantiere(txtDataDa.Text, txtDataA.Text, ddlScegliAcquirente.SelectedItem.Text, ddlScegliFornitore.SelectedItem.Text, txtNumDDT.Text);

            //Apro lo stream verso il file PDF
            Document pdfDoc = new Document(PageSize.A4, 8f, 2f, 2f, 2f);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            PdfPTable table = InitializePdfTableMatCant();
            Phrase title = new Phrase(txtNomeFile.Text, FontFactory.GetFont("Arial", 24, iTextSharp.text.Font.BOLD, BaseColor.RED));
            pdfDoc.Add(title);
            GeneraPDFPerMatCant(pdfDoc, matCantList, table, totale, numDdtAttuale, totaleFinale);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + txtNomeFile.Text + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
        }
        protected void GeneraPDFPerNumDDT(Document pdfDoc, List<DDTMef> ddtList, PdfPTable table, decimal totale, long numDdtAttuale, decimal totFin)
        {
            PdfPTable tblTotali;
            for (int k = 0; k < ddtList.Count; k++)
            {
                if (numDdtAttuale != ddtList[k].N_DDT)
                {
                    numDdtAttuale = ddtList[k].N_DDT;
                    Phrase intestazione = GeneraIntestazioneDDT(ddtList, k);

                    //Transfer rows from GridView to table
                    for (int i = 0; i < grdStampaDDT.Columns.Count; i++)
                    {
                        Phrase cellText = new Phrase(Server.HtmlDecode(grdStampaDDT.Columns[i].HeaderText), FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                        PdfPCell cell = new PdfPCell(cellText)
                        {
                            BorderWidth = 0,
                            BorderWidthBottom = 1,
                            BorderColorBottom = BaseColor.BLUE,
                            HorizontalAlignment = Element.ALIGN_LEFT
                        };
                        table.AddCell(cell);

                        if (i == 4 || i == 5)
                        {
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                    }

                    for (int i = 0; i < grdStampaDDT.Rows.Count; i++)
                    {
                        if (grdStampaDDT.Rows[i].RowType == DataControlRowType.DataRow)
                        {
                            for (int j = 0; j < grdStampaDDT.Columns.Count; j++)
                            {
                                if (grdStampaDDT.Rows[i].Cells[0].Text == numDdtAttuale.ToString())
                                {
                                    if (j != 5)
                                    {
                                        Phrase cellText;
                                        if (j == 4)
                                        {
                                            cellText = new Phrase(Server.HtmlDecode(Math.Round(Convert.ToDecimal(grdStampaDDT.Rows[i].Cells[j].Text), 2).ToString()), FontFactory.GetFont("Arial", 10, BaseColor.BLACK));
                                        }
                                        else
                                        {
                                            cellText = new Phrase(Server.HtmlDecode(grdStampaDDT.Rows[i].Cells[j].Text), FontFactory.GetFont("Arial", 10, BaseColor.BLACK));
                                        }

                                        PdfPCell cell = new PdfPCell(cellText) { BorderWidth = 0 };
                                        switch (j)
                                        {
                                            case 3:
                                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                                break;
                                            case 4:
                                            case 5:
                                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                break;
                                        }

                                        //Set Color of Alternating row
                                        if (i % 2 != 0)
                                        {
                                            cell.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F7F7F7"));
                                        }
                                        table.AddCell(cell);
                                    }
                                    else
                                    {
                                        decimal valore = Convert.ToDecimal(Server.HtmlDecode(grdStampaDDT.Rows[i].Cells[3].Text)) * Convert.ToDecimal(Server.HtmlDecode(grdStampaDDT.Rows[i].Cells[4].Text));
                                        Phrase val = new Phrase(Math.Round(valore, 2).ToString(), FontFactory.GetFont("Arial", 10, BaseColor.BLACK));
                                        PdfPCell valCel = new PdfPCell(val) { HorizontalAlignment = Element.ALIGN_RIGHT };
                                        grdStampaDDT.Rows[i].Cells[5].Text = Math.Round(valore, 2).ToString();
                                        valCel.BorderWidth = 0;
                                        table.AddCell(valCel);
                                        totale += Math.Round(valore, 2);
                                    }
                                }
                                else { break; }
                            }
                        }
                    }

                    tblTotali = new PdfPTable(1) { WidthPercentage = 100 };
                    GeneraTotalePerNumDDT(tblTotali, totale);
                    pdfDoc.Add(new Paragraph(""));
                    pdfDoc.Add(intestazione);
                    pdfDoc.Add(table);
                    pdfDoc.Add(new Paragraph(""));
                    pdfDoc.Add(tblTotali);
                    totFin += totale;
                    table = InitializePdfTableDDT();
                    totale = 0m;
                }
                else
                {
                    continue;
                }
            }

            tblTotali = new PdfPTable(1) { WidthPercentage = 100 };
            GeneraTotaliFinali(tblTotali, totFin);
            pdfDoc.Add(new Paragraph(""));
            pdfDoc.Add(tblTotali);
        }
        protected void GeneraPDFPerMatCant(Document pdfDoc, List<MaterialiCantieri> matCantList, PdfPTable table, decimal totale, long numDdtAttuale, decimal totFin)
        {
            for (int k = 0; k < matCantList.Count; k++)
            {
                if (numDdtAttuale != Convert.ToInt64(matCantList[k].NumeroBolla))
                {
                    numDdtAttuale = Convert.ToInt64(matCantList[k].NumeroBolla);
                    Phrase intestazione = GeneraIntestazioneMatCant(matCantList, k);

                    //Transfer rows from GridView to table
                    for (int i = 0; i < grdStampaMateCant.Columns.Count; i++)
                    {
                        Phrase cellText = new Phrase(Server.HtmlDecode(grdStampaMateCant.Columns[i].HeaderText), FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                        PdfPCell cell = new PdfPCell(cellText)
                        {
                            BorderWidth = 0,
                            BorderWidthBottom = 1,
                            BorderColorBottom = BaseColor.BLUE,
                            HorizontalAlignment = Element.ALIGN_LEFT
                        };
                        table.AddCell(cell);

                        if (i == 8 || i == 9)
                        {
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                    }

                    for (int i = 0; i < grdStampaMateCant.Rows.Count; i++)
                    {
                        if (grdStampaMateCant.Rows[i].RowType == DataControlRowType.DataRow)
                        {
                            for (int j = 0; j < grdStampaMateCant.Columns.Count; j++)
                            {
                                if (grdStampaMateCant.Rows[i].Cells[0].Text == numDdtAttuale.ToString())
                                {
                                    if (j != 8)
                                    {
                                        Phrase cellText;
                                        if (j == 7)
                                        {
                                            cellText = new Phrase(Server.HtmlDecode(string.Format("{0:n}", Math.Round(Convert.ToDecimal(grdStampaMateCant.Rows[i].Cells[j].Text)), 2).ToString()), FontFactory.GetFont("Arial", 10, BaseColor.BLACK));
                                        }
                                        else
                                        {
                                            cellText = new Phrase(Server.HtmlDecode(grdStampaMateCant.Rows[i].Cells[j].Text), FontFactory.GetFont("Arial", 10, BaseColor.BLACK));
                                        }

                                        PdfPCell cell = new PdfPCell(cellText) { BorderWidth = 0 };
                                        switch (j)
                                        {
                                            case 6:
                                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                                break;
                                            case 7:
                                            case 8:
                                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                break;
                                        }

                                        //Set Color of Alternating row
                                        if (i % 2 != 0)
                                        {
                                            cell.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#F7F7F7"));
                                        }
                                        table.AddCell(cell);
                                    }
                                    else
                                    {
                                        decimal valore = Convert.ToDecimal(Server.HtmlDecode(grdStampaMateCant.Rows[i].Cells[6].Text)) * Convert.ToDecimal(Server.HtmlDecode(grdStampaMateCant.Rows[i].Cells[7].Text));
                                        Phrase val = new Phrase(String.Format("{0:n}", Math.Round(valore, 2)), FontFactory.GetFont("Arial", 10, BaseColor.BLACK));
                                        PdfPCell valCel = new PdfPCell(val) { HorizontalAlignment = Element.ALIGN_RIGHT };
                                        grdStampaMateCant.Rows[i].Cells[8].Text = string.Format("{0:n}", Math.Round(valore, 2));
                                        valCel.BorderWidth = 0;
                                        table.AddCell(valCel);
                                        totale += Math.Round(valore, 2);
                                    }
                                }
                                else { break; }
                            }
                        }
                    }

                    PdfPTable tblTotali = new PdfPTable(1){WidthPercentage = 100};
                    GeneraTotalePerNumDDT(tblTotali, totale);
                    pdfDoc.Add(new Paragraph(""));
                    pdfDoc.Add(intestazione);
                    pdfDoc.Add(table);
                    pdfDoc.Add(new Paragraph(""));
                    pdfDoc.Add(tblTotali);
                    totFin += totale;
                    table = InitializePdfTableMatCant();
                    totale = 0m;
                }
                else
                {
                    continue;
                }
            }
        }

        //Intestazione PDF
        protected Phrase GeneraIntestazioneDDT(List<DDTMef> ddtList, int counter)
        {
            string n_ddt = $"N_DDT: {ddtList[counter].N_DDT}";
            string acquirente = $"Acquirente: {ddtList[counter].Acquirente}";
            string data = $"Data: {ddtList[counter].Data.ToString().Split(' ')[0]}";
            string intestazioneString = $"{n_ddt}    -    {acquirente}    -    {data}";
            Phrase intestazione = new Phrase(intestazioneString, FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.ITALIC, BaseColor.BLUE));
            return intestazione;
        }
        protected Phrase GeneraIntestazioneMatCant(List<MaterialiCantieri> ddtList, int counter)
        {
            string n_ddt = $"Num Bolla: {ddtList[counter].NumeroBolla}";
            string data = $"Data: {ddtList[counter].Data.ToString().Split(' ')[0]}";
            string intestazioneString = $"{n_ddt}    -    {data}";
            Phrase intestazione = new Phrase(intestazioneString, FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.ITALIC, BaseColor.BLUE));
            return intestazione;
        }
        protected void GeneraTotalePerNumDDT(PdfPTable tblTotali, decimal totale)
        {
            //Creazione Totali
            //Totale No Iva
            Phrase totalePerNumDDT = new Phrase("Totale: " + string.Format("{0:n}", totale), FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.ITALIC, BaseColor.BLUE));
            PdfPCell totPerNumDDTCell = new PdfPCell(totalePerNumDDT)
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                BorderWidth = 0,
                BorderWidthTop = 1,
                BorderColorTop = BaseColor.BLUE,
                PaddingBottom = 20
            };
            tblTotali.AddCell(totPerNumDDTCell);
        }
        protected void GeneraTotaliFinali(PdfPTable tblTotali, decimal totaleFinale)
        {
            //IVA
            decimal iva = Math.Round(totaleFinale * 22 / 100, 2);
            PdfPCell ivaCell = new PdfPCell(new Phrase("IVA 22%: " + iva, FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.ITALIC, BaseColor.BLUE)));

            //Totale + IVA
            PdfPCell totaleConIvaCell = new PdfPCell(new Phrase("Imponibile: " + string.Format("{0:n}", totaleFinale + iva), FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.ITALIC, BaseColor.BLUE)));

            //Stile celle totali
            Phrase totFinNoIva = new Phrase("Tot: " + string.Format("{0:n}", totaleFinale), FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.ITALIC, BaseColor.BLUE));
            PdfPCell totFinNoIvaCell = new PdfPCell(totFinNoIva)
            {
                HorizontalAlignment = ivaCell.HorizontalAlignment = totaleConIvaCell.HorizontalAlignment = Element.ALIGN_RIGHT,
                BorderWidth = ivaCell.BorderWidth = totaleConIvaCell.BorderWidth = 0,
                BorderColorTop = BaseColor.BLUE,
                BorderWidthTop = 1
            };
            tblTotali.AddCell(totFinNoIvaCell);
            tblTotali.AddCell(ivaCell);
            tblTotali.AddCell(totaleConIvaCell);
        }
        #endregion

        #region Eventi Click
        /* EVENTI CLICK */
        protected void btnStampaDDT_Click(object sender, EventArgs e)
        {
            lblIsNomeFileInserito.Text = "";
            if (txtNomeFile.Text != "")
            {
                if (txtNumDDT.Text != "")
                {
                    if (DDTMefDAO.CheckIfDdtExistBetweenData(txtNumDDT.Text, txtDataDa.Text, txtDataA.Text))
                    {
                        BindGridStampaDDT();
                        ExportToPdfPerDDT();
                    }
                    else
                    {
                        lblIsNomeFileInserito.Text = "Il DDT cercato NON è presente nella lista dei DDT";
                        lblIsNomeFileInserito.ForeColor = Color.Red;
                    }
                }
                else
                {
                    BindGridStampaDDT();
                    ExportToPdfPerDDT();
                }
            }
            else
            {
                lblIsNomeFileInserito.Text = "Campo \"Nome File\" obbligatorio!";
                lblIsNomeFileInserito.ForeColor = Color.Red;
            }
        }
        protected void btnStampaMatCant_Click(object sender, EventArgs e)
        {
            if (txtNomeFile.Text != "")
            {
                BindGridStampaMatCant();
                ExportToPdfPerMatCant();
                AddPageNumber();
            }
            else
            {
                lblIsNomeFileInserito.Text = "Campo \"Nome File\" obbligatorio!";
                lblIsNomeFileInserito.ForeColor = Color.Red;
            }
        }
        protected void btnAggiungiNumPagine_Click(object sender, EventArgs e)
        {
            AddPageNumber();
        }
        #endregion

        #region Eventi TextChanged
        /* EVENTI TEXT-CHANGED */
        protected void ddlScegliStampa_TextChanged(object sender, EventArgs e)
        {
            switch (ddlScegliStampa.SelectedIndex)
            {
                case 0:
                    pnlCampiStampaDDT_MatCant.Visible = false;
                    break;
                case 1:
                    pnlCampiStampaDDT_MatCant.Visible = true;
                    btnStampaDDT.Visible = true;
                    btnStampaMatCant.Visible = !btnStampaDDT.Visible;
                    break;
                case 2:
                    pnlCampiStampaDDT_MatCant.Visible = true;
                    btnStampaDDT.Visible = false;
                    btnStampaMatCant.Visible = !btnStampaDDT.Visible;
                    break;
            }
        }
        #endregion

        public override void VerifyRenderingInServerForm(Control control)
        {
            //Do nothing
        }
    }
}