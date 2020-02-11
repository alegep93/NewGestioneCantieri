using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class Amministratori : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ResetToInitial();
                BindGrid();
            }
        }

        private void ResetToInitial()
        {
            txtNomeAmministratore.Text = "";
            btnAggiungiAmministratore.Visible = true;
            btnModificaAmministratore.Visible = false;
        }

        private void BindGrid()
        {
            try
            {
                grdAmministratori.DataSource = AmministratoriDAO.GetAll();
                grdAmministratori.DataBind();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Errore durante l'aggiornamento dei dati della griglia, " + ex.Message;
                lblMsg.ForeColor = Color.Red;
            }
        }

        protected void grdAmministratori_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long idAmministratore = Convert.ToInt64(e.CommandArgument);
            hfIdAmministratore.Value = idAmministratore.ToString();

            if (e.CommandName == "Modifica")
            {
                txtNomeAmministratore.Text = AmministratoriDAO.GetSingle(idAmministratore).Nome;
                btnAggiungiAmministratore.Visible = false;
                btnModificaAmministratore.Visible = true;
            }
            else if (e.CommandName == "Elimina")
            {
                AmministratoriDAO.Delete(idAmministratore);
                ResetToInitial();
                BindGrid();
            }
        }

        protected void btnAggiungiAmministratore_Click(object sender, EventArgs e)
        {
            try
            {
                AmministratoriDAO.Insert(txtNomeAmministratore.Text);
                ResetToInitial();
                BindGrid();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Errore durante l'aggiunta di un nuovo amministratore, " + ex.Message;
                lblMsg.ForeColor = Color.Red;
            }
        }

        protected void btnModificaAmministratore_Click(object sender, EventArgs e)
        {
            try
            {
                AmministratoriDAO.Update(new Amministratore
                {
                    IdAmministratori = Convert.ToInt64(hfIdAmministratore.Value),
                    Nome = txtNomeAmministratore.Text
                });
                ResetToInitial();
                BindGrid();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Errore durante la modifica di un amministratore, " + ex.Message;
                lblMsg.ForeColor = Color.Red;
            }
        }
    }
}