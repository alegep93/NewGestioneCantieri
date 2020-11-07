using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using GestioneCantieri.Utils;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Utils;

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

        private void FillDdlGruppi()
        {
            ddlScegliGruppo.Items.Clear();
            ddlScegliGruppo.Items.Add(new ListItem("", "-1"));
            DropDownListManager.FillDdlGruppi(GruppiFruttiDAO.GetGruppi(txtFiltroGruppo1.Text, txtFiltroGruppo2.Text, txtFiltroGruppo3.Text), ref ddlScegliGruppo);
        }

        private void BindGrid()
        {
            try
            {
                grdGruppi.DataSource = GruppiFruttiDAO.GetGruppi(txtFiltroGruppo1.Text, txtFiltroGruppo2.Text, txtFiltroGruppo3.Text);
                grdGruppi.DataBind();

                txtNomeGruppo.Text = txtDescrizioneGruppo.Text = "";
                btnInserisciGruppo.Visible = true;
                btnModificaGruppo.Visible = !btnInserisciGruppo.Visible;

                FillDdlGruppi();
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
                    txtNomeGruppo.Enabled = txtDescrizioneGruppo.Enabled = false;
                    txtNomeGruppo.Text = gruppo.NomeGruppo;
                    txtDescrizioneGruppo.Text = gruppo.Descrizione;
                    btnInserisciGruppo.Visible = btnModificaGruppo.Visible = false;
                }
                if (e.CommandName == "Modifica")
                {
                    txtNomeGruppo.Enabled = txtDescrizioneGruppo.Enabled = true;
                    txtNomeGruppo.Text = gruppo.NomeGruppo;
                    txtDescrizioneGruppo.Text = gruppo.Descrizione;
                    hfIdGruppo.Value = idGruppo.ToString();
                    btnInserisciGruppo.Visible = false;
                    btnModificaGruppo.Visible = !btnInserisciGruppo.Visible;
                }
                if (e.CommandName == "Elimina")
                {
                    if (CompGruppoFrutDAO.GetCompGruppo(idGruppo).Count <= 0)
                    {
                        GruppiFruttiDAO.DeleteGruppo(idGruppo);
                        (Master as layout).SetAlert("alert-success", $"Gruppo {gruppo.NomeGruppo} eliminato con successo");
                        BindGrid();
                    }
                    else
                    {
                        (Master as layout).SetAlert("alert-danger", $"Impossibile eliminare il gruppo, perchè contiene dei componenti");
                    }
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

        protected void btnClonaGruppo_Click(object sender, EventArgs e)
        {
            DBTransaction tr = new DBTransaction();
            tr.Begin();
            try
            {
                int idGruppo = Convert.ToInt32(ddlScegliGruppo.SelectedItem.Value);
                GruppiFrutti gf = GruppiFruttiDAO.GetSingle(idGruppo, tr);
                int idGruppoCopia = GruppiFruttiDAO.InserisciGruppo("Copia" + gf.NomeGruppo, gf.Descrizione, tr);
                List<CompGruppoFrut> components = CompGruppoFrutDAO.GetCompGruppo(idGruppo, tr);
                components.ForEach(f => f.IdTblGruppo = idGruppoCopia);
                CompGruppoFrutDAO.InsertList(components, tr);
                tr.Commit();
            }
            catch (Exception ex)
            {
                tr.Rollback();
                (Master as layout).SetAlert("alert-danger", $"Errore durante la clonazione del gruppo selezionato - {ex.Message}");
            }

            (Master as layout).SetAlert("alert-success", "Gruppo clonato con successo");
            BindGrid();
        }

        protected void txtFiltroGruppo_TextChanged(object sender, EventArgs e)
        {
            BindGrid();
        }
    }
}