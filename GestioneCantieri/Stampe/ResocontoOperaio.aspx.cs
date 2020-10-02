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
        }

        protected void btnPagaOperaio_Click(object sender, EventArgs e)
        {
            if (MaterialiCantieriDAO.UpdateOperaioPagato(txtDataDa.Text, txtDataA.Text, ddlScegliOperaio.SelectedItem.Value))
            {
                lblIsOperaioPagato.Text = "Campo \"OperaioPagato\" aggiornato con successo";
                lblIsOperaioPagato.ForeColor = Color.Blue;
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
    }
}