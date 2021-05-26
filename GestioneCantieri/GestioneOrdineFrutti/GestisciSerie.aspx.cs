using Database.DAO;
using Database.Models;
using GestioneCantieri.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri.GestioneOrdineFrutti
{
    public partial class GestisciSerie : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
            Page.MaintainScrollPositionOnPostBack = true;
        }

        private void BindGrid()
        {
            try
            {
                grdSerie.DataSource = SerieDAO.GetAll();
                grdSerie.DataBind();
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante il BindGrid in GestisciSerie ===> {ex.Message}");
            }
        }

        protected void btnInsSerie_Click(object sender, EventArgs e)
        {
            try
            {
                SerieDAO.Insert(txtNomeSerie.Text);
                BindGrid();
                (Master as layout).SetAlert("alert-success", $"Serie inserita con successo");
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante l'inserimento di una Serie in GestisciSerie ===> {ex.Message}");
            }
        }

        protected void btnModificaSerie_Click(object sender, EventArgs e)
        {
            try
            {
                SerieDAO.Update(new Serie { IdSerie = Convert.ToInt64(hfIdSerie.Value), NomeSerie = txtNomeSerie.Text });
                BindGrid();
                (Master as layout).SetAlert("alert-success", $"Serie modificata con successo");
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante l'aggiornamento di una Serie in GestisciSerie ===> {ex.Message}");
            }
        }

        protected void grdSerie_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                long idSerie = Convert.ToInt64(e.CommandArgument);
                if (e.CommandName == "Modifica")
                {
                    hfIdSerie.Value = idSerie.ToString();
                    txtNomeSerie.Text = SerieDAO.GetAll().Where(w => w.IdSerie == idSerie).FirstOrDefault().NomeSerie;
                    btnInserisciSerie.Visible = false;
                    btnModificaSerie.Visible = !btnInserisciSerie.Visible;
                }
                else if (e.CommandName == "Elimina")
                {
                    SerieDAO.Delete(idSerie);
                    BindGrid();
                    (Master as layout).SetAlert("alert-success", $"Serie eliminata con successo");
                }
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante il grdSerie_RowCommand in GestisciSerie ===> {ex.Message}");
            }
        }
    }
}