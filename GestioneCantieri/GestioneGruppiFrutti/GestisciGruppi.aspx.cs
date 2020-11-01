using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using GestioneCantieri.Utils;
using System;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class GestisciGruppi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            try
            {
                grdGruppi.DataSource = GruppiFruttiDAO.GetAll();
                grdGruppi.DataBind();

                txtNomeGruppo.Text = txtDescrizioneGruppo.Text = "";
                btnInserisciGruppo.Visible = true;
                btnModificaGruppo.Visible = !btnInserisciGruppo.Visible;
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante la visualizzazione dei gruppi {ex.Message}");
            }
        }

        protected void grdGruppi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int idGruppo = Convert.ToInt32(e.CommandArgument);
                GruppiFrutti gruppo = GruppiFruttiDAO.GetSingle(idGruppo);
                if (e.CommandName == "Visualizza")
                {
                    txtNomeGruppo.Text = gruppo.NomeGruppo;
                    txtDescrizioneGruppo.Text = gruppo.Descrizione;
                    btnInserisciGruppo.Visible = btnModificaGruppo.Visible = false;
                }
                if (e.CommandName == "Modifica")
                {
                    txtNomeGruppo.Text = gruppo.NomeGruppo;
                    txtDescrizioneGruppo.Text = gruppo.Descrizione;
                    hfIdGruppo.Value = idGruppo.ToString();
                    btnInserisciGruppo.Visible = false;
                    btnModificaGruppo.Visible = !btnInserisciGruppo.Visible;
                }
                if (e.CommandName == "Elimina")
                {
                    GruppiFruttiDAO.DeleteGruppo(idGruppo);
                    (Master as layout).SetAlert("alert-success", $"Gruppo {gruppo.NomeGruppo} eliminato con successo");
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante il grdGruppi_RowCommand in GestisciGruppi - {ex.Message}");
            }
        }

        protected void btnInserisciGruppo_Click(object sender, EventArgs e)
        {
            try
            {
                string nomeGruppo = txtNomeGruppo.Text;
                if (GruppiFruttiDAO.GetByNome(nomeGruppo) == null)
                {
                    GruppiFruttiDAO.InserisciGruppo(nomeGruppo, txtDescrizioneGruppo.Text);
                    (Master as layout).SetAlert("alert-success", $"Gruppo {txtNomeGruppo.Text} inserito con successo");
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante il btnInserisciGruppo_Click in GestisciGruppi - {ex.Message}");
            }
        }

        protected void btnModificaGruppo_Click(object sender, EventArgs e)
        {
            try
            {
                GruppiFruttiDAO.UpdateGruppo(new GruppiFrutti
                {
                    Id = Convert.ToInt32(hfIdGruppo.Value),
                    NomeGruppo = txtNomeGruppo.Text,
                    Descrizione = txtDescrizioneGruppo.Text
                });
                (Master as layout).SetAlert("alert-success", $"Gruppo {txtNomeGruppo.Text} inserito con successo");
                BindGrid();
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante il btnInserisciGruppo_Click in GestisciGruppi - {ex.Message}");
            }
        }
    }
}