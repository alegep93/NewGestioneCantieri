using Database.DAO;
using Database.Models;
using GestioneCantieri.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class StampaPerTipologia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDdlScegliCantiere();
                //FillDdlScegliOperaio();
            }
        }

        #region Helpers
        protected void FillDdlScegliCantiere()
        {
            ddlScegliCant.Items.Clear();
            ddlScegliCant.Items.Add(new ListItem("", "-1"));
            DropDownListManager.FillDdlCantieri(CantieriDAO.GetCantieri(txtAnno.Text, txtCodCant.Text, "", chkChiuso.Checked, chkRiscosso.Checked), ref ddlScegliCant);
        }
        //protected void FillDdlScegliOperaio()
        //{
        //    ddlScegliOperaio.Items.Clear();
        //    ddlScegliOperaio.Items.Add(new ListItem("", "-1"));
        //    DropDownListManager.FillDdlOperaio(OperaiDAO.GetAll(), ref ddlScegliOperaio);
        //}
        protected void BindGrid()
        {
            pnlShowGridAndLabel.Visible = true;
            List<MaterialiCantieri> mcList = MaterialiCantieriDAO.GetMaterialeCantierePerTipologia(Convert.ToInt32(ddlScegliCant.SelectedItem.Value), txtDataDa.Text, txtDataA.Text, "MANODOPERA");

            grdStampaPerTipologia.DataSource = mcList;
            grdStampaPerTipologia.DataBind();

            lblTotOre.Text = $"<strong>Totale Ore</strong>: {Math.Round(mcList.Sum(s => (decimal)s.Qta), 2)}";
            lblTotale.Text = $"<strong>Totale Valore</strong>: {Math.Round(mcList.Sum(s => (decimal)s.Qta * s.PzzoUniCantiere), 2)}";
        }
        #endregion

        #region Eventi Click
        protected void btnFiltraCantieri_Click(object sender, EventArgs e)
        {
            FillDdlScegliCantiere();
        }
        protected void btnStampaVerificaCant_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
        #endregion

        protected void ddlScegliCant_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlShowGridAndLabel.Visible = false;
        }
    }
}