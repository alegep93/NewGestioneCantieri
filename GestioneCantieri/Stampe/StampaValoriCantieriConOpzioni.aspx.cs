using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using GestioneCantieri.Utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;

namespace GestioneCantieri
{
    public partial class StampaValoriCantieriConOpzioni : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDdlScegliCliente();
            }
        }

        #region Helpers
        protected void FillDdlScegliCliente()
        {
            ddlScegliCliente.Items.Clear();
            ddlScegliCliente.Items.Add(new System.Web.UI.WebControls.ListItem("", "-1"));
            DropDownListManager.FillDdlCliente(ClientiDAO.GetClienti(txtFiltraCliente.Text), ref ddlScegliCliente);
        }

        protected void CreateExcel(List<Data.StampaValoriCantieriConOpzioni> items)
        {
            if (items.Count > 0)
            {
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=StampaValoriCantieriConOpzioni-" + ddlScegliCliente.SelectedItem.Text + ".xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.xls";

                System.IO.StringWriter stringWrite = new System.IO.StringWriter();

                HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

                htmlWrite.WriteLine("<strong><font size='4'>" + ddlScegliCliente.SelectedItem.Text + "</font></strong>");

                // viene reindirizzato il rendering verso la stringa in uscita
                grdStampaConOpzioni.DataSource = items;
                grdStampaConOpzioni.DataBind();
                grdStampaConOpzioni.RenderControl(htmlWrite);
                Response.Write(stringWrite.ToString());

                Response.End();
            }
            else
            {
                (Master as layout).SetAlert("alert-warning", "Per effettuare la stampa excel è necessario vedere i dati a video");
            }
        }
        #endregion

        #region Eventi Click
        protected void btnStampaContoCliente_Click(object sender, EventArgs e)
        {
            try
            {
                List<Cantieri> listaCantieri = CantieriDAO.GetCantieri(txtAnno.Text, Convert.ToInt32(ddlScegliCliente.SelectedValue), chkFatturato.Checked, chkChiuso.Checked, chkRiscosso.Checked, chkNonRiscuotibile.Checked);

                if (listaCantieri.Count() > 0)
                {
                    List<MaterialiCantieri> materiali = MaterialiCantieriDAO.GetByListOfCantieri(RicalcoloContiManager.GetStringFromListForQuery(listaCantieri.Select(s => s.IdCantieri).ToList()));
                    List<Pagamenti> pagamenti = PagamentiDAO.GetAll();
                    List<Data.StampaValoriCantieriConOpzioni> gridViewItems = new List<Data.StampaValoriCantieriConOpzioni>();

                    materiali.ForEach(f =>
                    {
                        Cantieri cantiere = listaCantieri.Where(w => w.IdCantieri == f.IdTblCantieri).FirstOrDefault();
                        Data.StampaValoriCantieriConOpzioni objStampa = new Data.StampaValoriCantieriConOpzioni
                        {
                            CodCant = cantiere.CodCant,
                            DescriCodCAnt = cantiere.DescriCodCant,
                            RagSocCli = cantiere.RagSocCli
                        };

                        objStampa.TotaleAcconti = objStampa.TotaleConto = objStampa.TotaleFinale = 0m;

                        //Popolo il campo Conto/Preventivo
                        objStampa.TotaleConto = cantiere.Preventivo ? cantiere.ValorePreventivo : Math.Round(RicalcoloContiManager.GetMaterialiCantieri(cantiere.IdCantieri).Sum(s => s.Valore), 2);

                        //Popolo il campo Tot. Acconti
                        decimal totAcconti = 0m;
                        totAcconti = pagamenti.Where(w => w.IdTblCantieri == cantiere.IdCantieri).ToList().Sum(s => s.Imporo);
                        objStampa.TotaleAcconti = totAcconti;

                        //Popolo il campo Tot. Finale
                        decimal totContoPreventivo = objStampa.TotaleConto;
                        decimal totFin = totContoPreventivo - totAcconti;
                        objStampa.TotaleFinale = totFin;

                        if (RicalcoloContiManager.CalcolaPercentualeTotaleMaterialiNascosti(f.IdTblCantieri) == -1)
                        {
                            objStampa.TotaleAcconti = objStampa.TotaleConto = objStampa.TotaleFinale = -999.99m;
                        }

                        //Aggiungo l'oggetto alla lista
                        gridViewItems.Add(objStampa);
                    });

                    gridViewItems = gridViewItems.GroupBy(s => new { s.CodCant, s.DescriCodCAnt, s.RagSocCli, s.TotaleConto, s.TotaleAcconti, s.TotaleFinale }).Distinct().Select(s => s.First()).ToList();
                    grdStampaConOpzioni.DataSource = gridViewItems;
                    grdStampaConOpzioni.DataBind();

                    // Metto i dati in sessione per poter successivamente creare l'excel senza dover rifare tutto il giro
                    Session["StampaValConOpzData"] = gridViewItems;

                    // Assegno il valore alla label che mostra il totale generale
                    decimal totGen = gridViewItems.Sum(s => s.TotaleFinale);

                    for (int i = 0; i < grdStampaConOpzioni.Rows.Count; i++)
                    {
                        //if (grdStampaConOpzioni.Rows[i].Cells[5].Text == "0")
                        //{
                        //    grdStampaConOpzioni.Rows[i].Visible = false;
                        //}

                        // Se il totale Conto mostra un valore palesemente errato, la cella viene modificata mostrando l'errore invece del valore del cantiere
                        if (grdStampaConOpzioni.Rows[i].Cells[3].Text == "-999,99")
                        {
                            grdStampaConOpzioni.Rows[i].Cells[3].Text = grdStampaConOpzioni.Rows[i].Cells[4].Text = grdStampaConOpzioni.Rows[i].Cells[5].Text = "VEDI VALORI NON VISIBILI";
                            grdStampaConOpzioni.Rows[i].Cells[3].BackColor = grdStampaConOpzioni.Rows[i].Cells[4].BackColor = grdStampaConOpzioni.Rows[i].Cells[5].BackColor = Color.Red;
                        }
                    }

                    lblTotaleGeneraleStampa.Text = $"Totale: {totGen:N2} €";

                    // Nascondo eventuali alert visualizzati in precedenza
                    (Master as layout).HideAlert();
                }
                else
                {
                    Session["StampaValConOpzData"] = null;
                    (Master as layout).SetAlert("alert-warning", "Non ci sono cantieri che corrispondano ai filtri impostati");
                }
            }
            catch (Exception ex)
            {
                Session["StampaValConOpzData"] = null;
                (Master as layout).SetAlert("alert-danger", $"Errore durante la stampa del cantiere ==> {ex.Message}");
            }
        }

        protected void btnFiltraCantieri_Click(object sender, EventArgs e)
        {
            FillDdlScegliCliente();
        }

        protected void btnGeneraExcel_Click(object sender, EventArgs e)
        {
            try
            {
                // Controllo fatto per verificare che sia stata popolata la tabella prima di effettuare la stampa excel
                if (Session["StampaValConOpzData"] != null)
                {
                    List<Data.StampaValoriCantieriConOpzioni> items = (List<Data.StampaValoriCantieriConOpzioni>)Session["StampaValConOpzData"];
                    CreateExcel(items);
                }
                else
                {
                    (Master as layout).SetAlert("alert-warning", "Prima di effettuare la stampa excel è necessario visualizzare i dati a video, tramite il pulsante \"Stampa Valori Cantieri\"");
                }
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante la stampa excel ==> {ex.Message}");
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