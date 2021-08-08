using Database.DAO;
using Database.Models;
using GestioneCantieri.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class OrdineFrutti : System.Web.UI.Page
    {
        /* Lista pubblica per la visualizzazione dinamica di record */
        public List<MatOrdFrut> compList = new List<MatOrdFrut>();
        public List<MatOrdFrut> fruttiList = new List<MatOrdFrut>();
        int progressivo = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlInserisciDati.Visible = pnlScegliGruppo.Visible = false;
                /*pnlMostraGruppiInseriti.Visible = */
                btnInserisciGruppo.Visible = false;
                lblQtaFrutto.Visible = txtQtaFrutto.Visible = btnInserisciFrutto.Visible = false;
                FillDdlScegliCantiere();
                FillDdlScegliLocale();
                FillDdlScegliSerie();
                FillDdlGruppi();
                FillDdlFrutti();
                //FillNomiGruppoOrdine();
            }
        }

        /* EVENTI CLICK */
        protected void btnFiltroGruppi_Click(object sender, EventArgs e)
        {
            FillDdlGruppi();
            PopolaListe();
            ddlScegliGruppo.SelectedIndex = 0;
        }
        protected void btnFiltraFrutti_Click(object sender, EventArgs e)
        {
            FillDdlFrutti();
            PopolaListe();
            ddlScegliFrutto.SelectedIndex = 0;
        }
        protected void btnInserisciGruppo_Click(object sender, EventArgs e)
        {
            int? idGruppoOrdine = null; // (ddlScegliGruppoOrdine != null && ddlScegliGruppoOrdine.SelectedIndex != 0) ? Convert.ToInt32(ddlScegliGruppoOrdine.SelectedValue) : (int?)null;
            long? idSerie = ddlScegliSerie.SelectedValue != "-1" ? Convert.ToInt64(ddlScegliSerie.SelectedValue) : (long?)null;
            bool isAggiunto = OrdineFruttiDAO.InserisciGruppo(ddlScegliCantiere.SelectedItem.Value, ddlScegliGruppo.SelectedItem.Value, ddlScegliLocale.SelectedItem.Value, idGruppoOrdine, idSerie);

            if (isAggiunto)
            {
                lblIsGruppoInserito.Text = "Componente '" + ddlScegliGruppo.SelectedItem.Text + "' aggiunto correttamente!";
                lblIsGruppoInserito.ForeColor = Color.Blue;
            }
            else
            {
                lblIsGruppoInserito.Text = "Errore durante l'inserimento del gruppo '" + ddlScegliGruppo.SelectedItem.Text + "'";
                lblIsGruppoInserito.ForeColor = Color.Red;
            }

            PopolaListe();
            BindGrid();
            ddlScegliGruppo.SelectedIndex = 0;
        }
        protected void btnInserisciFrutto_Click(object sender, EventArgs e)
        {
            if (ddlScegliFrutto.SelectedValue != "-1")
            {
                if (txtQtaFrutto.Text != "" && Convert.ToInt32(txtQtaFrutto.Text) > 0)
                {
                    int? idGruppoOrdine = null; // (ddlScegliGruppoOrdine != null && ddlScegliGruppoOrdine.SelectedIndex != 0) ? Convert.ToInt32(ddlScegliGruppoOrdine.SelectedValue) : (int?)null;
                    long? idSerie = ddlScegliSerie.SelectedValue != "-1" ? Convert.ToInt64(ddlScegliSerie.SelectedValue) : (long?)null;
                    bool isInserito = OrdineFruttiDAO.InserisciFruttoNonInGruppo(ddlScegliCantiere.SelectedItem.Value, ddlScegliLocale.SelectedItem.Value, ddlScegliFrutto.SelectedItem.Value, txtQtaFrutto.Text, idGruppoOrdine, idSerie);

                    if (isInserito)
                    {
                        lblIsFruttoInserito.Text = "Frutto inserito con successo";
                        lblIsFruttoInserito.ForeColor = Color.Blue;
                    }
                    else
                    {
                        lblIsFruttoInserito.Text = "Errore durante l'inserimento del frutto";
                        lblIsFruttoInserito.ForeColor = Color.Red;
                    }

                    ddlScegliFrutto.SelectedIndex = 0;
                    txtQtaFrutto.Text = "";
                }
                else
                {
                    lblIsFruttoInserito.Text = "Il campo quantità deve essere compilato e deve essere inserito un valore maggiore di 0";
                    lblIsFruttoInserito.ForeColor = Color.Red;
                }
            }
            else
            {
                lblIsFruttoInserito.Text = "È necessario scegliere un frutto prima di inserirlo";
                lblIsFruttoInserito.ForeColor = Color.Red;
            }
            PopolaListe();
            BindGrid();
        }
        //protected void btnCreaNuovoGruppoOrdine_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int rows = MatOrdFrutGroupDAO.Insert(txtNomeGruppoOrdine.Text);

        //        if (rows > 0)
        //        {
        //            lblNuovoGruppoOrdine.Text = "Nuovo gruppo inserito correttamente";
        //            lblNuovoGruppoOrdine.ForeColor = Color.Blue;
        //        }
        //        else
        //        {
        //            lblNuovoGruppoOrdine.Text = "Gruppo già esistente";
        //            lblNuovoGruppoOrdine.ForeColor = Color.Yellow;
        //        }

        //        FillNomiGruppoOrdine();
        //    }
        //    catch
        //    {
        //        lblNuovoGruppoOrdine.Text = "Errore durante la creazione di un nuovo gruppo ordine";
        //        lblNuovoGruppoOrdine.ForeColor = Color.Red;
        //    }
        //}

        protected void ddlScegliCantiere_TextChanged(object sender, EventArgs e)
        {
            if (ddlScegliCantiere.SelectedIndex != 0)
            {
                pnlInserisciDati.Visible = true;
                //pnlInserisciNuovoGruppoOrdine.Visible = true;
                pnlScegliGruppo.Visible = false;
                lblQtaFrutto.Visible = txtQtaFrutto.Visible = btnInserisciFrutto.Visible = false;
            }
            else
            {
                pnlInserisciDati.Visible = pnlInserisciDati.Visible = pnlScegliGruppo.Visible = false;
                //pnlInserisciNuovoGruppoOrdine.Visible = false;
                lblQtaFrutto.Visible = txtQtaFrutto.Visible = btnInserisciFrutto.Visible = false;
            }
            SvuotaCampi();
        }

        protected void ddlScegliLocale_TextChanged(object sender, EventArgs e)
        {
            if (ddlScegliLocale.SelectedIndex != 0)
            {
                pnlScegliGruppo.Visible = true;
                PopolaListe();
                BindGrid();
            }
        }
        protected void ddlScegliGruppo_TextChanged(object sender, EventArgs e)
        {
            if (ddlScegliGruppo.SelectedItem.Text != "")
            {
                PopolaListe();
                btnInserisciGruppo.Visible = true;
                lblIsGruppoInserito.Text = "";
            }
            else
            {
                PopolaListe();
                btnInserisciGruppo.Visible = false;
            }
        }
        protected void ddlScegliFrutto_TextChanged(object sender, EventArgs e)
        {
            if (ddlScegliFrutto.SelectedIndex != 0)
            {
                lblQtaFrutto.Visible = txtQtaFrutto.Visible = btnInserisciFrutto.Visible = true;
                PopolaListe();
            }
            else
            {
                lblQtaFrutto.Visible = txtQtaFrutto.Visible = btnInserisciFrutto.Visible = false;
                PopolaListe();
            }
        }

        /* EVENTI ROW-COMMAND */
        protected void grdOrdini_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "EliminaOrdine")
            {
                OrdineFruttiDAO.Delete(id);
            }

            BindGrid();
        }

        /* EVENTI SELECT INDEX CHANGED */
        //protected void ddlScegliGruppoOrdine_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlScegliGruppoOrdine != null && ddlScegliGruppoOrdine.SelectedIndex != -1)
        //    {
        //        if (ddlScegliGruppoOrdine.SelectedIndex == 0)
        //        {
        //            pnlInserisciNuovoGruppoOrdine.Visible = true;
        //        }
        //        else
        //        {
        //            pnlInserisciNuovoGruppoOrdine.Visible = false;
        //        }
        //    }
        //}

        /* HELPERS */
        protected void FillDdlScegliCantiere()
        {
            ddlScegliCantiere.Items.Clear();
            ddlScegliCantiere.Items.Add(new ListItem("", "-1"));
            DropDownListManager.FillDdlCantieri(CantieriDAO.GetAll().Where(w => !w.Chiuso).ToList(), ref ddlScegliCantiere);
        }
        protected void FillDdlScegliLocale()
        {
            ddlScegliLocale.Items.Clear();
            ddlScegliLocale.Items.Add(new ListItem("", "-1"));
            DropDownListManager.FillDdlLocali(LocaliDAO.GetAll(), ref ddlScegliLocale);
        }
        protected void FillDdlScegliSerie()
        {
            ddlScegliSerie.Items.Clear();
            ddlScegliSerie.Items.Add(new ListItem("", "-1"));
            DropDownListManager.FillDdlSerie(SerieDAO.GetAll(), ref ddlScegliSerie);
        }
        protected void FillDdlGruppi()
        {
            List<GruppiFrutti> listGruppiFrutti = GruppiFruttiDAO.GetGruppi(txtFiltroGruppo1.Text, txtFiltroGruppo2.Text, txtFiltroGruppo3.Text);
            ddlScegliGruppo.Items.Clear();
            ddlScegliGruppo.Items.Add(new ListItem("", "-1"));
            foreach (GruppiFrutti gf in listGruppiFrutti)
            {
                string nomeDescrGruppo = gf.NomeGruppo + " - " + gf.Descrizione;
                ddlScegliGruppo.Items.Add(new ListItem(nomeDescrGruppo, gf.Id.ToString()));
            }
        }
        protected void FillDdlFrutti()
        {
            ddlScegliFrutto.Items.Clear();
            ddlScegliFrutto.Items.Add(new ListItem("", "-1"));
            DropDownListManager.FillDdlFrutti(FruttiDAO.GetFrutti(txtFiltroFrutto1.Text, txtFiltroFrutto2.Text, txtFiltroFrutto3.Text), ref ddlScegliFrutto);
        }
        
        //protected void FillNomiGruppoOrdine()
        //{
        //    ddlScegliGruppoOrdine.Items.Clear();
        //    ddlScegliGruppoOrdine.Items.Add(new ListItem("", "-1"));
        //    DropDownListManager.FillDdlMatOrdFrutGroup(MatOrdFrutGroupDAO.GetAll(), ref ddlScegliGruppoOrdine);
        //}

        protected void PopolaListe()
        {
            string idCantiere = ddlScegliCantiere.SelectedItem.Value;
            string idLocale = ddlScegliLocale.SelectedItem.Value;
            fruttiList = OrdineFruttiDAO.GetFruttiNonInGruppo(idCantiere, idLocale);
            compList = OrdineFruttiDAO.GetGruppi(idCantiere, idLocale);
        }
        private void BindGrid()
        {
            List<MatOrdFrut> ordFrutList = OrdineFruttiDAO.GetInfoForCantiereAndLocale(ddlScegliCantiere.SelectedValue, ddlScegliLocale.SelectedValue);
            grdOrdini.DataSource = ordFrutList;
            grdOrdini.DataBind();
        }

        private void SvuotaCampi()
        {
            txtFiltroFrutto1.Text = txtFiltroFrutto2.Text = txtFiltroFrutto3.Text = txtFiltroGruppo1.Text = txtFiltroGruppo2.Text = txtFiltroGruppo3.Text = txtQtaFrutto.Text = "";
            ddlScegliFrutto.SelectedIndex = ddlScegliGruppo.SelectedIndex = ddlScegliLocale.SelectedIndex = 0;
            //txtNomeGruppoOrdine.Text = "";
            //ddlScegliGruppoOrdine.SelectedIndex = 0;
        }

        protected void btnEliminaOrdine_Click(object sender, EventArgs e)
        {
            try
            {
                OrdineFruttiDAO.DeleteOrdine(Convert.ToInt32(ddlScegliCantiere.SelectedValue));
                lblMsg.Text = "Ordine eliminato con successo";
                lblMsg.ForeColor = Color.Blue;
                BindGrid();
                SvuotaCampi();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Errore durante l'eliminazione dell'ordine ==> " + ex.Message;
                lblMsg.ForeColor = Color.Red;
            }
        }

        protected void grdOrdini_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = "Progressivo";
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (++progressivo).ToString();
            }
        }
    }
}