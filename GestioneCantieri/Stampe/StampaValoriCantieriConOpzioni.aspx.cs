using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using GestioneCantieri.Utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;

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
        #endregion

        #region Eventi Click
        protected void btnStampaContoCliente_Click(object sender, EventArgs e)
        {
            try
            {
                List<Cantieri> listaCantieri = CantieriDAO.GetCantieri(txtAnno.Text, Convert.ToInt32(ddlScegliCliente.SelectedValue), chkFatturato.Checked, chkChiuso.Checked, chkRiscosso.Checked, chkNonRiscuotibile.Checked);
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
                    objStampa.TotaleConto = cantiere.Preventivo ? cantiere.ValorePreventivo : Math.Round(materiali.Sum(s => s.Valore), 2);

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

                // Assegno il valore alla label che mostra il totale generale
                decimal totGen = gridViewItems.Sum(s => s.TotaleFinale);

                for (int i = 0; i < grdStampaConOpzioni.Rows.Count; i++)
                {
                    if (grdStampaConOpzioni.Rows[i].Cells[5].Text == "0")
                    {
                        grdStampaConOpzioni.Rows[i].Visible = false;
                    }

                    // Se il totale Conto mostra un valore palesemente errato, la cella viene modificata mostrando l'errore invece del valore del cantiere
                    if (grdStampaConOpzioni.Rows[i].Cells[3].Text == "-999,99")
                    {
                        grdStampaConOpzioni.Rows[i].Cells[3].Text = grdStampaConOpzioni.Rows[i].Cells[4].Text = grdStampaConOpzioni.Rows[i].Cells[5].Text = "--- ERRORE ---";
                    }
                }

                lblTotaleGeneraleStampa.Text = $"Totale: {totGen:N2} €";
            }
            catch (Exception ex)
            {
                lblMsg.Text = $"Errore durante la stampa del cantiere ==> {ex.Message}";
            }
        }
        protected void btnFiltraCantieri_Click(object sender, EventArgs e)
        {
            FillDdlScegliCliente();
        }
        #endregion
    }
}