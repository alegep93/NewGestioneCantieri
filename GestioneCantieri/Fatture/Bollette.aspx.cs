using Database.DAO;
using Database.Models;
using GestioneCantieri.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class Bollette : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDdlScegliFornitori();
                ResetToInitial();
            }
        }

        private void FillDdlScegliFornitori()
        {
            ddlScegliFornitore.Items.Clear();
            ddlFiltroFornitore.Items.Clear();
            ddlScegliFornitore.Items.Add(new ListItem("", "-1"));
            ddlFiltroFornitore.Items.Add(new ListItem("", "-1"));
            List<Fornitori> fornitori = FornitoriDAO.GetFornitori();
            DropDownListManager.FillDdlFornitore(fornitori, ref ddlScegliFornitore);
            DropDownListManager.FillDdlFornitore(fornitori, ref ddlFiltroFornitore, false);
        }

        private void ResetToInitial()
        {
            int currentYear = DateTime.Today.Year;
            List<Bolletta> bollette = BolletteDAO.GetAll(currentYear, 0);
            txtDataBolletta.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtDataScadenza.Text = txtDataPagamento.Text = txtTotaleBolletta.Text = "";
            txtProgressivo.Text = GetProgressivo();
            ddlScegliFornitore.SelectedIndex = 0;
            btnAggiungiBolletta.Visible = true;
            btnModificaBolletta.Visible = false;
            hfIdBolletta.Value = "";
            txtFiltroAnno.Text = currentYear.ToString();
            BindGrid(bollette);
        }

        private void BindGrid(List<Bolletta> bollette)
        {
            grdBollette.DataSource = bollette;
            grdBollette.DataBind();
        }

        protected void grdBollette_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                long idBolletta = Convert.ToInt64(e.CommandArgument);
                hfIdBolletta.Value = idBolletta.ToString();

                if (e.CommandName == "Modifica")
                {
                    Bolletta bolletta = BolletteDAO.GetSingle(idBolletta);
                    ddlScegliFornitore.SelectedValue = bolletta.IdFornitori.ToString();
                    txtDataBolletta.Text = bolletta.DataBolletta.ToString("yyyy-MM-dd");
                    txtDataScadenza.Text = bolletta.DataScadenza.ToString("yyyy-MM-dd");
                    txtDataPagamento.Text = bolletta.DataPagamento.ToString("yyyy-MM-dd");
                    txtDataScadenza.TextMode = txtDataPagamento.TextMode = TextBoxMode.Date;
                    txtTotaleBolletta.Text = bolletta.TotaleBolletta.ToString("N2");
                    txtProgressivo.Text = bolletta.Progressivo.ToString();
                    btnAggiungiBolletta.Visible = false;
                    btnModificaBolletta.Visible = true;
                }
                else
                {
                    BolletteDAO.Delete(idBolletta);
                    ResetToInitial();
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = $"Errore durante il grdBollette_RowCommand, " + ex.Message;
            }
        }

        protected void btnAggiungiBolletta_Click(object sender, EventArgs e)
        {
            try
            {
                BolletteDAO.Insert(new Bolletta
                {
                    IdFornitori = Convert.ToInt64(ddlScegliFornitore.SelectedValue),
                    DataBolletta = Convert.ToDateTime(txtDataBolletta.Text),
                    DataScadenza = Convert.ToDateTime(txtDataScadenza.Text),
                    DataPagamento = Convert.ToDateTime(txtDataPagamento.Text),
                    TotaleBolletta = Convert.ToDecimal(txtTotaleBolletta.Text.Replace(".", ",")),
                    Progressivo = Convert.ToInt32(txtProgressivo.Text)
                });
                ResetToInitial();
            }
            catch (Exception ex)
            {
                lblMsg.Text = $"Errore durante l'aggiunta di una bolletta, " + ex.Message;
            }
        }

        protected void btnModificaBolletta_Click(object sender, EventArgs e)
        {
            try
            {
                BolletteDAO.Update(new Bolletta
                {
                    IdBollette = Convert.ToInt64(hfIdBolletta.Value),
                    IdFornitori = Convert.ToInt64(ddlScegliFornitore.SelectedValue),
                    DataBolletta = Convert.ToDateTime(txtDataBolletta.Text),
                    DataScadenza = Convert.ToDateTime(txtDataScadenza.Text),
                    DataPagamento = Convert.ToDateTime(txtDataPagamento.Text),
                    TotaleBolletta = Convert.ToDecimal(txtTotaleBolletta.Text.Replace(".", ",")),
                    Progressivo = Convert.ToInt32(txtProgressivo.Text)
                });
                ResetToInitial();
            }
            catch (Exception ex)
            {
                lblMsg.Text = $"Errore durante la modifica di una bolletta, " + ex.Message;
            }
        }

        protected void txtDataBolletta_TextChanged(object sender, EventArgs e)
        {
            if (hfIdBolletta.Value == "")
            {
                txtProgressivo.Text = GetProgressivo(Convert.ToDateTime(txtDataBolletta.Text).Year);
            }
        }

        private string GetProgressivo(int anno = 0)
        {
            int progressivo = 1;
            List<Bolletta> items = BolletteDAO.GetAll(0, 0).Where(w => w.DataBolletta.Year == (anno > 0 ? anno : DateTime.Now.Year)).ToList();
            if (items.Count > 0)
            {
                progressivo = items.Select(s => s.Progressivo).Max() + 1;
            }
            return progressivo.ToString();
        }

        protected void btnFiltraBollette_Click(object sender, EventArgs e)
        {
            int anno = txtFiltroAnno.Text != "" ? Convert.ToInt32(txtFiltroAnno.Text) : 0;
            int idFornitore = ddlFiltroFornitore.SelectedValue != "" ? Convert.ToInt32(ddlFiltroFornitore.SelectedValue) : 0;
            BindGrid(BolletteDAO.GetAll(anno, idFornitore));
        }
    }
}