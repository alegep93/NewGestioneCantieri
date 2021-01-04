using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace GestioneCantieri
{
    public partial class GestisciFrutti : Page
    {
        public List<Frutto> fruttiList = new List<Frutto>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
                txtNomeFrutto.Text = "";
                lblMsg.Text = "";
            }
        }

        /* EVENTI CLICK */
        protected void btnInsFrutto_Click(object sender, EventArgs e)
        {
            if (txtNomeFrutto.Text != "")
            {
                bool isAggiunto = FruttiDAO.InserisciFrutto(txtNomeFrutto.Text);

                if (isAggiunto)
                {
                    lblMsg.Text = "Frutto '" + txtNomeFrutto.Text + "' inserito correttamente!";
                    lblMsg.ForeColor = Color.Blue;
                }
                else
                {
                    lblMsg.Text = "Esiste già un frutto con lo stesso nome";
                    lblMsg.ForeColor = Color.Red;
                }

                txtNomeFrutto.Text = "";
                BindGrid();
            }
            else
            {
                lblMsg.Text = "Il campo 'Nome Frutto' deve essere compilato";
                lblMsg.ForeColor = Color.Red;
            }
        }

        protected void btnSaveModFrutto_Click(object sender, EventArgs e)
        {
            if (txtNomeFrutto.Text != "")
            {
                bool isSaved = FruttiDAO.UpdateFrutto(new Frutto { Id1 = Convert.ToInt32(hfIdFrutto.Value), Descr001 = txtNomeFrutto.Text });
                if (isSaved)
                {
                    lblMsg.Text = $"Frutto modificato con successo in '{txtNomeFrutto.Text}'";
                    lblMsg.ForeColor = Color.Blue;
                }
                else
                {
                    lblMsg.Text = "Errore durante la modifica del frutto";
                    lblMsg.ForeColor = Color.Red;
                }
                txtNomeFrutto.Text = "";
                BindGrid();
            }
            else
            {
                lblMsg.Text = "Il campo 'Nome Frutto' deve essere compilato";
                lblMsg.ForeColor = Color.Red;
            }
        }

        /* EVENTI TEXT-CAHNGED */
        protected void txtFiltroListaFrutti_TextChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        /* HELPERS */
        private void BindGrid()
        {
            grdFrutti.DataSource = FruttiDAO.GetFrutti(txtFiltroFrutti1.Text, txtFiltroFrutti2.Text, txtFiltroFrutti3.Text);
            grdFrutti.DataBind();

            // Ripristino la visibilità del pulsante di inserimento frutti
            btnInsFrutto.Visible = true;
            btnSaveModFrutto.Visible = !btnInsFrutto.Visible;
        }

        /* GRID VIEW EVENTS */
        protected void grdFrutti_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int idFrutto = Convert.ToInt32(e.CommandArgument);
                if (e.CommandName == "Modifica")
                {
                    Frutto frutto = FruttiDAO.GetSingle(idFrutto);
                    txtNomeFrutto.Text = frutto.Descr001;
                    hfIdFrutto.Value = frutto.Id1.ToString();
                    btnInsFrutto.Visible = false;
                    btnSaveModFrutto.Visible = !btnInsFrutto.Visible;
                }
                else if (e.CommandName == "Elimina")
                {
                    bool isDeleted = FruttiDAO.DeleteFrutto(idFrutto);
                    if (isDeleted)
                    {
                        lblMsg.Text = $"Frutto eliminato con successo";
                        lblMsg.ForeColor = Color.Blue;
                    }
                    else
                    {
                        lblMsg.Text = $"Impossibile eliminare il frutto, verificare che non sia referenziato in altre tabelle";
                        lblMsg.ForeColor = Color.Red;
                    }
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = $"Errore durante il grdFrutti_RowCommand in GestisciFrutti.aspx.cs ===> {ex.Message}";
            }
        }
    }
}