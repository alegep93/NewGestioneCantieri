using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using GestioneCantieri.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri.GestioneOrdineFrutti
{
    public partial class GestisciDefaultOrdineFrutti : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDdlLocali();
                FillDdlScegliSerie();
                FillDdlGruppi();
                FillDdlFrutti();
                BindGrid();
            }
        }

        private void FillDdlLocali()
        {
            try
            {
                DropDownListManager.FillDdlLocali(LocaliDAO.GetOnlyFirstTypeOfLocale(), ref ddlScegliLocale);
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante il FillDdl ==> {ex.Message}");
            }
        }

        private void BindGrid()
        {
            try
            {
                grdDefMatOrdFrut.DataSource = DefaultMatOrdFrutDAO.GetDefaultLocale(Convert.ToInt32(ddlScegliLocale.SelectedValue));
                grdDefMatOrdFrut.DataBind();
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante il BindGrid ==> {ex.Message}");
            }
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

        protected void grdDefMatOrdFrut_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void grdDefMatOrdFrut_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void ddlScegliLocale_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnFiltroGruppi_Click(object sender, EventArgs e)
        {
            FillDdlGruppi();
            ddlScegliGruppo.SelectedIndex = 0;
        }
        protected void btnFiltraFrutti_Click(object sender, EventArgs e)
        {
            FillDdlFrutti();
            ddlScegliFrutto.SelectedIndex = 0;
        }

        protected void btnInserisciGruppo_Click(object sender, EventArgs e)
        {
            long? idSerie = ddlScegliSerie.SelectedValue != "-1" ? Convert.ToInt64(ddlScegliSerie.SelectedValue) : (long?)null;
            bool isAggiunto = DefaultMatOrdFrutDAO.InserisciGruppo(new DefaultMatOrdFrut
            {
                IdGruppiFrutti = Convert.ToInt32(ddlScegliGruppo.SelectedValue),
                IdLocale = Convert.ToInt32(ddlScegliLocale.SelectedValue),
                IdSerie = Convert.ToInt64(idSerie)
            });

            if (isAggiunto)
            {
                (Master as layout).SetAlert("alert-success", $"Componente {ddlScegliGruppo.SelectedItem.Text} aggiunto correttamente");
            }
            else
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante l'inserimento del gruppo {ddlScegliGruppo.SelectedItem.Text}");
            }
            BindGrid();
            ddlScegliGruppo.SelectedIndex = 0;
        }

        protected void btnInserisciFrutto_Click(object sender, EventArgs e)
        {
            if (ddlScegliFrutto.SelectedValue != "-1")
            {
                if (txtQtaFrutto.Text != "" && Convert.ToInt32(txtQtaFrutto.Text) > 0)
                {
                    long? idSerie = ddlScegliSerie.SelectedValue != "-1" ? Convert.ToInt64(ddlScegliSerie.SelectedValue) : (long?)null;
                    bool isInserito = DefaultMatOrdFrutDAO.InserisciGruppo(new DefaultMatOrdFrut
                    {
                        IdLocale = Convert.ToInt32(ddlScegliLocale.SelectedValue),
                        IdFrutto = Convert.ToInt32(ddlScegliFrutto.SelectedItem.Value),
                        QtaFrutti = Convert.ToInt32(txtQtaFrutto.Text),
                        IdSerie = Convert.ToInt64(idSerie)
                    });

                    if (isInserito)
                    {

                        (Master as layout).SetAlert("alert-success", "Frutto inserito con successo");
                    }
                    else
                    {
                        (Master as layout).SetAlert("alert-danger", "Errore durante l'inserimento del frutto");
                    }
                    ddlScegliFrutto.SelectedIndex = 0;
                    txtQtaFrutto.Text = "";
                }
                else
                {
                    (Master as layout).SetAlert("alert-warning", "Il campo quantità deve essere compilato e deve essere inserito un valore maggiore di 0");
                }
            }
            else
            {
                (Master as layout).SetAlert("alert-warning", "È necessario scegliere un frutto prima di inserirlo");
            }
            BindGrid();
        }

        protected void ddlScegliGruppo_TextChanged(object sender, EventArgs e)
        {
            btnInserisciGruppo.Visible = ddlScegliGruppo.SelectedItem.Text != "";
        }

        protected void ddlScegliFrutto_TextChanged(object sender, EventArgs e)
        {
            lblQtaFrutto.Visible = txtQtaFrutto.Visible = btnInserisciFrutto.Visible = ddlScegliFrutto.SelectedIndex != 0;
        }
    }
}