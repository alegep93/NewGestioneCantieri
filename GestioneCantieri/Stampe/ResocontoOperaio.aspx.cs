using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using GestioneCantieri.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class ResocontoOperaio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDdlScegliAcquirente();
                txtDataDa.Text = DateTime.Now.Year + "-01-01";
                txtDataA.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtDataDa.TextMode = txtDataA.TextMode = TextBoxMode.Date;
                btnPagaOperaio.Visible = false;
            }
        }

        /* HELPERS */
        protected void FillDdlScegliAcquirente()
        {
            ddlScegliOperaio.Items.Clear();
            ddlScegliOperaio.Items.Add(new ListItem("", "-1"));
            DropDownListManager.FillDdlOperaio(OperaiDAO.GetAll(), ref ddlScegliOperaio);
        }

        protected void BindGrid()
        {
            List<MaterialiCantieri> items = MaterialiCantieriDAO.GetMatCantPerResocontoOperaio(txtDataDa.Text, txtDataA.Text, ddlScegliOperaio.SelectedItem.Value, txtFiltroCantiere.Text, Convert.ToInt32(rblChooseView.SelectedValue));
            grdResocontoOperaio.DataSource = items;
            grdResocontoOperaio.DataBind();
            lblTotaleOre.Text = $"Totale Ore: {items.Sum(s => s.Qta)}";
            lblTotaleValore.Text = $"Totale Valore: {items.Sum(s => s.Valore):N2}";
        }

        protected void btnStampaResoconto_Click(object sender, EventArgs e)
        {
            BindGrid();
            btnPagaOperaio.Visible = true;
            CheckToEnablePagaOperaio();
            grdResocontoOperaio.Visible = true;
            grdResocontoRaggruppato.Visible = !grdResocontoOperaio.Visible;
        }

        protected void btnPagaOperaio_Click(object sender, EventArgs e)
        {
            if (MaterialiCantieriDAO.UpdateOperaioPagato(txtDataDa.Text, txtDataA.Text, ddlScegliOperaio.SelectedItem.Value))
            {
                if (AccontiOperaiDAO.UpdateAccontoPagato(Convert.ToDateTime(txtDataDa.Text), Convert.ToDateTime(txtDataA.Text), Convert.ToInt32(ddlScegliOperaio.SelectedItem.Value)))
                {
                    lblIsOperaioPagato.Text = "Campo \"OperaioPagato\" aggiornato con successo";
                    lblIsOperaioPagato.ForeColor = Color.Blue;
                }
            }
            else
            {
                lblIsOperaioPagato.Text = "Impossibile aggiornare il campo \"OperaioPagato\"";
                lblIsOperaioPagato.ForeColor = Color.Red;
            }
            BindGrid();
        }

        protected void btnFiltra_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void ddlScegliOperaio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckToEnablePagaOperaio();
        }

        private void CheckToEnablePagaOperaio()
        {
            btnPagaOperaio.Enabled = ddlScegliOperaio.SelectedIndex != 0;
        }

        protected void btnResocontoRaggruppato_Click(object sender, EventArgs e)
        {
            try
            {
                List<MaterialiCantieri> items = MaterialiCantieriDAO.GetMatCantPerResocontoOperaio(txtDataDa.Text, txtDataA.Text, ddlScegliOperaio.SelectedItem.Value, txtFiltroCantiere.Text, Convert.ToInt32(rblChooseView.SelectedValue));
                items.Where(w => w.CodCant == "Acconto").ToList().ForEach(f => f.Acquirente = $"Acconto {f.Acquirente}");
                items = items.GroupBy(g => new { g.Data, g.Acquirente })
                    .Select(s => new MaterialiCantieri
                    {
                        Data = s.Key.Data,
                        Acquirente = s.Key.Acquirente,
                        Qta = s.Sum(x => x.Qta),
                        PzzoUniCantiere = s.Min(x => x.PzzoUniCantiere),
                        Valore = s.Sum(x => x.Valore)
                    }).ToList();

                grdResocontoRaggruppato.DataSource = items;
                grdResocontoRaggruppato.DataBind();

                grdResocontoOperaio.Visible = false;
                grdResocontoRaggruppato.Visible = !grdResocontoOperaio.Visible;
            }
            catch (Exception ex)
            {
                lblIsOperaioPagato.Text = $"Errore durante il btnResocontoRaggruppato_Click - {ex.Message}";
                lblIsOperaioPagato.ForeColor = Color.Red;
            }
        }
    }
}