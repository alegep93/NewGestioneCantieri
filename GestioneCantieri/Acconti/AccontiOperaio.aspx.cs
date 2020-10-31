using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using GestioneCantieri.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri.Acconti
{
    public partial class AccontiOperaio : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDdlOperai();
                BindGrid(Convert.ToInt64(ddlFiltroScegliOperaio.SelectedValue));
            }
        }

        private void FillDdlOperai()
        {
            List<Operai> operai = OperaiDAO.GetAll();
            ddlScegliOperaio.Items.Clear();
            ddlScegliOperaio.Items.Add(new ListItem("", "-1"));
            ddlFiltroScegliOperaio.Items.Add(new ListItem("", "-1"));
            DropDownListManager.FillDdlOperaio(operai, ref ddlScegliOperaio);
            DropDownListManager.FillDdlOperaio(operai, ref ddlFiltroScegliOperaio);
        }

        private void BindGrid(long idOperaio = -1)
        {
            grdAccontiOperai.DataSource = AccontiOperaiDAO.GetAcconti(idOperaio);
            grdAccontiOperai.DataBind();

            txtData.Text = txtImportoAcconto.Text = "";
            btnSalvaAcconto.Visible = true;
            btnModificaAcconto.Visible = !btnSalvaAcconto.Visible;
        }

        protected void btnSalvaAcconto_Click(object sender, EventArgs e)
        {
            try
            {
                AccontiOperaiDAO.Insert(new AccontoOperaio
                {
                    Data = Convert.ToDateTime(txtData.Text),
                    IdOperaio = Convert.ToInt32(ddlScegliOperaio.SelectedValue),
                    Importo = Convert.ToDecimal(txtImportoAcconto.Text),
                    Descrizione = txtDescrizioneAcconto.Text
                });

                lblMsg.Text = $"Acconto inserito con successo";
                lblMsg.ForeColor = Color.Blue;

                BindGrid();
            }
            catch (Exception ex)
            {
                lblMsg.Text = $"Errore durante il salvataggio di un Acconto - {ex.Message}";
                lblMsg.ForeColor = Color.Red;
            }
        }

        protected void btnModificaAcconto_Click(object sender, EventArgs e)
        {
            try
            {
                AccontiOperaiDAO.Update(new AccontoOperaio
                {
                    IdAccontoOperaio = Convert.ToInt64(hfIdAccontoOperaio.Value),
                    Data = Convert.ToDateTime(txtData.Text),
                    IdOperaio = Convert.ToInt32(ddlScegliOperaio.SelectedValue),
                    Importo = Convert.ToDecimal(txtImportoAcconto.Text),
                    Descrizione = txtDescrizioneAcconto.Text
                });

                lblMsg.Text = $"Acconto modificato con successo";
                lblMsg.ForeColor = Color.Blue;

                BindGrid();
            }
            catch (Exception ex)
            {
                lblMsg.Text = $"Errore durante il salvataggio di un Acconto - {ex.Message}";
                lblMsg.ForeColor = Color.Red;
            }
        }

        protected void grdAccontiOperai_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                long idAccontoOperaio = Convert.ToInt64(e.CommandArgument);
                AccontoOperaio accontoOperaio = AccontiOperaiDAO.GetSingle(idAccontoOperaio);
                if (e.CommandName == "Visualizza")
                {
                    PopolaCampi(accontoOperaio);
                    btnSalvaAcconto.Visible = false;
                    btnModificaAcconto.Visible = false;
                }
                if (e.CommandName == "Modifica")
                {
                    PopolaCampi(accontoOperaio, true);
                    hfIdAccontoOperaio.Value = accontoOperaio.IdAccontoOperaio.ToString();
                    btnSalvaAcconto.Visible = false;
                    btnModificaAcconto.Visible = !btnSalvaAcconto.Visible;
                }
                if (e.CommandName == "Elimina")
                {
                    AccontiOperaiDAO.Delete(idAccontoOperaio);

                    lblMsg.Text = $"Acconto eliminato con successo";
                    lblMsg.ForeColor = Color.Blue;

                    btnSalvaAcconto.Visible = true;
                    btnModificaAcconto.Visible = !btnSalvaAcconto.Visible;

                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = $"Errore durante il grdAccontiOperai_RowCommand - {ex.Message}";
                lblMsg.ForeColor = Color.Red;
            }
        }

        private void PopolaCampi(AccontoOperaio item, bool enableFields = false)
        {
            txtData.Enabled = ddlScegliOperaio.Enabled = txtImportoAcconto.Enabled = enableFields;
            txtData.Text = item.Data.ToString("yyyy-MM-dd");
            ddlScegliOperaio.SelectedValue = item.IdOperaio.ToString();
            txtImportoAcconto.Text = item.Importo.ToString("N2");
            txtDescrizioneAcconto.Text = item.Descrizione;
        }

        protected void grdAccontiOperai_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[3].Text = "Pagato";
            }
        }

        protected void ddlFiltroScegliOperaio_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid(Convert.ToInt64(ddlFiltroScegliOperaio.SelectedValue));
        }
    }
}