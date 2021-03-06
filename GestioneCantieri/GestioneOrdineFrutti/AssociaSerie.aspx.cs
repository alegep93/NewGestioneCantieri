﻿using Database.DAO;
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
    public partial class AssociaSerie : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        private void BindGrid(bool fillDdls = true)
        {
            try
            {
                if (fillDdls)
                {
                    FillDdls();
                }

                grdFruttiSerie.DataSource = FruttiSerieDAO.GetAll().Where(w => w.IdSerie == Convert.ToInt64(ddlFiltroSerie.SelectedValue)).ToList();
                grdFruttiSerie.DataBind();
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante il BindGrid in GestisciSerie ===> {ex.Message}");
            }
        }

        private void FillDdls()
        {
            try
            {
                ddlScegliFrutto.Items.Clear();
                DropDownListManager.FillDdlFrutti(FruttiDAO.GetFrutti(txtFiltroFrutti1.Text, txtFiltroFrutti2.Text, txtFiltroFrutti3.Text), ref ddlScegliFrutto);

                ddlScegliSerie.Items.Clear();
                DropDownListManager.FillDdlSerie(SerieDAO.GetAll(), ref ddlScegliSerie);

                ddlFiltroSerie.Items.Clear();
                DropDownListManager.FillDdlSerie(SerieDAO.GetAll(), ref ddlFiltroSerie);
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante il FillDdls in GestisciSerie ===> {ex.Message}");
            }
        }

        private void FillListino()
        {
            try
            {
                ddlScegliListino.Items.Clear();
                DropDownListManager.FillDdlMamg0WithCodiceListinoUnivoco(Mamg0DAO.GetListino(txtFiltroCodice1.Text, txtFiltroCodice2.Text, txtFiltroCodice3.Text, txtFiltroDescr1.Text, txtFiltroDescr2.Text, txtFiltroDescr3.Text), ref ddlScegliListino);
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante il FillDdls in GestisciSerie ===> {ex.Message}");
            }
        }

        protected void btnInserisciFruttoSerie_Click(object sender, EventArgs e)
        {
            try
            {
                int idFrutto = Convert.ToInt32(ddlScegliFrutto.SelectedValue);
                long idSerie = Convert.ToInt64(ddlScegliSerie.SelectedValue);
                FruttoSerie item = FruttiSerieDAO.GetAll().Where(w => w.IdFrutto == idFrutto && w.IdSerie == idSerie)?.FirstOrDefault() ?? null;

                // Verifico che il frutto selezionato non sia già associato alla serie scelta
                if (item == null)
                {
                    FruttiSerieDAO.Insert(new FruttoSerie
                    {
                        IdFrutto = idFrutto,
                        IdSerie = idSerie,
                        CodiceListinoUnivoco = ddlScegliListino.SelectedValue
                    });

                    BindGrid();
                    (Master as layout).SetAlert("alert-success", $"Inserimento di un'associazione frutto-serie avvenuto con successo");
                }
                else
                {
                    (Master as layout).SetAlert("alert-warning", $"Non è possibile associare lo stesso frutto più di una volta alla stessa serie");
                }
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante l'inserimento di un'associazione frutto-serie in GestisciSerie ===> {ex.Message}");
            }
        }

        protected void btnModificaFruttoSerie_Click(object sender, EventArgs e)
        {
            try
            {
                int idFrutto = Convert.ToInt32(ddlScegliFrutto.SelectedValue);
                long idSerie = Convert.ToInt64(ddlScegliSerie.SelectedValue);
                FruttoSerie item = FruttiSerieDAO.GetAll().Where(w => w.IdFrutto == idFrutto && w.IdSerie == idSerie)?.FirstOrDefault() ?? null;
                // Verifico che il frutto selezionato non sia già associato alla serie scelta
                if (item == null)
                {
                    FruttiSerieDAO.Update(new FruttoSerie
                    {
                        IdFruttoSerie = Convert.ToInt64(hfIdFruttoSerie.Value),
                        IdFrutto = Convert.ToInt32(ddlScegliFrutto.SelectedValue),
                        IdSerie = Convert.ToInt64(ddlScegliSerie.SelectedValue),
                        CodiceListinoUnivoco = ddlScegliListino.SelectedValue
                    });

                    BindGrid();
                    (Master as layout).SetAlert("alert-success", $"Aggiornamento di un'associazione frutto-serie avvenuto con successo");
                }
                else
                {
                    (Master as layout).SetAlert("alert-warning", $"Non è possibile associare lo stesso frutto più di una volta alla stessa serie");
                }
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante la modifica di un'associazione frutto-serie in GestisciSerie ===> {ex.Message}");
            }
        }

        protected void ddlFiltroSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid(false);
        }

        protected void txtFiltroListino_TextChanged(object sender, EventArgs e)
        {
            FillListino();
        }

        protected void txtFiltroFrutti_TextChanged(object sender, EventArgs e)
        {
            FillDdls();
        }

        protected void grdFruttiSerie_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                long idFruttoSerie = Convert.ToInt64(e.CommandArgument);

                if (e.CommandName == "Modifica")
                {
                    FruttoSerie fruttoSerie = FruttiSerieDAO.GetAll().Where(w => w.IdFruttoSerie == idFruttoSerie).FirstOrDefault();
                    hfIdFruttoSerie.Value = idFruttoSerie.ToString();
                    ddlScegliFrutto.SelectedValue = fruttoSerie.IdFrutto.ToString();
                    ddlScegliSerie.SelectedValue = fruttoSerie.IdSerie.ToString();
                    List<Mamg0> listino = new List<Mamg0>() { Mamg0DAO.GetSingle(fruttoSerie.CodiceListinoUnivoco) };
                    ddlScegliListino.Items.Clear();
                    DropDownListManager.FillDdlMamg0WithCodiceListinoUnivoco(listino, ref ddlScegliListino);
                    btnInserisciFruttoSerie.Visible = false;
                    btnModificaFruttoSerie.Visible = !btnInserisciFruttoSerie.Visible;
                }
                else if (e.CommandName == "Elimina")
                {
                    FruttiSerieDAO.Delete(idFruttoSerie);
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante il grdFruttiSerie_RowCommand in GestisciSerie ===> {ex.Message}");
            }
        }
    }
}