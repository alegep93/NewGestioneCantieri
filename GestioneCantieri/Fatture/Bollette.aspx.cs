using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
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
            List<Fornitori> fornitori = FornitoriDAO.GetFornitori();
            ddlScegliFornitore.Items.Clear();
            ddlScegliFornitore.Items.Add(new ListItem("", "-1"));
            fornitori.ForEach(f => ddlScegliFornitore.Items.Add(new ListItem(f.RagSocForni, f.IdFornitori.ToString())));
        }

        private void ResetToInitial()
        {
            txtDataScadenza.Text = txtDataPagamento.Text = txtTotaleBolletta.Text = "";
            ddlScegliFornitore.SelectedIndex = 0;
            btnAggiungiBolletta.Visible = true;
            btnModificaBolletta.Visible = false;

            grdBollette.DataSource = BolletteDAO.GetAll();
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
                    txtDataScadenza.Text = bolletta.DataScadenza.ToString("yyyy-MM-dd");
                    txtDataPagamento.Text = bolletta.DataPagamento.ToString("yyyy-MM-dd");
                    txtDataScadenza.TextMode = txtDataPagamento.TextMode = TextBoxMode.Date;
                    txtTotaleBolletta.Text = bolletta.TotaleBolletta.ToString("N2");
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
                    DataScadenza = Convert.ToDateTime(txtDataScadenza.Text),
                    DataPagamento = Convert.ToDateTime(txtDataPagamento.Text),
                    TotaleBolletta = Convert.ToDecimal(txtTotaleBolletta.Text.Replace(".",","))
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
                    DataScadenza = Convert.ToDateTime(txtDataScadenza.Text),
                    DataPagamento = Convert.ToDateTime(txtDataPagamento.Text),
                    TotaleBolletta = Convert.ToDecimal(txtTotaleBolletta.Text.Replace(".", ","))
                });
                ResetToInitial();
            }
            catch (Exception ex)
            {
                lblMsg.Text = $"Errore durante la modifica di una bolletta, " + ex.Message;
            }
        }
    }
}