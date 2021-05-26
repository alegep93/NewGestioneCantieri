using Database.DAO;
using Database.Models;
using GestioneCantieri.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class DistintaBase : System.Web.UI.Page
    {
        /* Liste pubbliche per la visualizzazione dinamica di record */
        public List<CompGruppoFrut> compList = new List<CompGruppoFrut>();
        public List<GruppiFrutti> gruppiList = new List<GruppiFrutti>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDdlGruppi();
            }
            ddlScegliFrutto.Enabled = txtQuantitàFrutto.Enabled = true;
        }

        #region Helpers
        private void BindGrid()
        {
            try
            {
                grdFruttiInGruppo.DataSource = CompGruppoFrutDAO.GetCompGruppo(Convert.ToInt32(ddlScegliGruppo.SelectedValue));
                grdFruttiInGruppo.DataBind();

                txtQuantitàFrutto.Text = "";
                btnInserisciFruttoInGruppo.Visible = true;
                btnModificaFruttoInGruppo.Visible = !btnInserisciFruttoInGruppo.Visible;
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante il BindGrid - {ex.Message}");
            }
        }

        protected void FillDdlFrutti()
        {
            ddlScegliFrutto.Items.Clear();
            //ddlScegliFrutto.Items.Add(new ListItem("", "-1"));
            DropDownListManager.FillDdlFrutti(FruttiDAO.GetFrutti(), ref ddlScegliFrutto);
        }

        protected void FillDdlGruppi()
        {
            ddlScegliGruppo.Items.Clear();
            ddlScegliGruppo.Items.Add(new ListItem("", "-1"));
            DropDownListManager.FillDdlGruppi(GruppiFruttiDAO.GetGruppi(txtFiltroGruppo1.Text, txtFiltroGruppo2.Text, txtFiltroGruppo3.Text), ref ddlScegliGruppo);
        }
        #endregion

        #region TextChanged
        protected void txtFiltroGruppo_TextChanged(object sender, EventArgs e)
        {
            FillDdlGruppi();
        }

        protected void ddlScegliGruppo_TextChanged(object sender, EventArgs e)
        {
            pnlAssociaFruttoAGruppo.Visible = ddlScegliGruppo.SelectedIndex > 0;
            FillDdlFrutti();
            BindGrid();
        }
        #endregion

        #region Click
        protected void btnInserisciFruttoInGruppo_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtQuantitàFrutto.Text != "" && txtQuantitàFrutto.Text != "0")
                {
                    CompGruppoFrutDAO.Insert(new CompGruppoFrut
                    {
                        IdTblGruppo = Convert.ToInt32(ddlScegliGruppo.SelectedValue),
                        IdTblFrutto = Convert.ToInt32(ddlScegliFrutto.SelectedValue),
                        Qta = Convert.ToInt32(txtQuantitàFrutto.Text)
                    });
                    (Master as layout).SetAlert("alert-success", "Frutto inserito con successo");
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante l'inserimento del frutto selezionato - {ex.Message}");
            }
        }

        protected void btnModificaFruttoInGruppo_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtQuantitàFrutto.Text != "" && txtQuantitàFrutto.Text != "0")
                {
                    CompGruppoFrutDAO.Update(new CompGruppoFrut
                    {
                        Id = Convert.ToInt32(hfIdCompGruppoFrutto.Value),
                        IdTblGruppo = Convert.ToInt32(ddlScegliGruppo.SelectedValue),
                        IdTblFrutto = Convert.ToInt32(ddlScegliFrutto.SelectedValue),
                        Qta = Convert.ToInt32(txtQuantitàFrutto.Text)
                    });
                    (Master as layout).SetAlert("alert-success", "Frutto modificato con successo");
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante la modifica del frutto selezionato - {ex.Message}");
            }
        }
        #endregion

        #region Gridview Events
        protected void grdGruppi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int idCompGruppoFrutto = Convert.ToInt32(e.CommandArgument);
                CompGruppoFrut componente = CompGruppoFrutDAO.GetSingle(idCompGruppoFrutto);
                if (e.CommandName == "Visualizza")
                {
                    ddlScegliFrutto.Enabled = txtQuantitàFrutto.Enabled = false;
                    ddlScegliFrutto.SelectedValue = componente.IdTblFrutto.ToString();
                    txtQuantitàFrutto.Text = componente.Qta.ToString();
                    btnInserisciFruttoInGruppo.Visible = btnModificaFruttoInGruppo.Visible = false;
                }
                if (e.CommandName == "Modifica")
                {
                    ddlScegliFrutto.Enabled = txtQuantitàFrutto.Enabled = true;
                    hfIdCompGruppoFrutto.Value = idCompGruppoFrutto.ToString();
                    ddlScegliFrutto.SelectedValue = componente.IdTblFrutto.ToString();
                    txtQuantitàFrutto.Text = componente.Qta.ToString();
                    btnInserisciFruttoInGruppo.Visible = false;
                    btnModificaFruttoInGruppo.Visible = !btnInserisciFruttoInGruppo.Visible;
                }
                if (e.CommandName == "Elimina")
                {
                    CompGruppoFrutDAO.Delete(idCompGruppoFrutto);
                    (Master as layout).SetAlert("alert-success", $"Frutto \"{componente.NomeFrutto}\" eliminato con successo");
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante il grdGruppi_RowCommand in GestisciGruppi - {ex.Message}");
            }
        }
        #endregion
    }
}