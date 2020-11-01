using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using GestioneCantieri.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class DistintaBase : System.Web.UI.Page
    {
        /* Liste pubbliche per la visualizzazione dinamica di record */
        public List<CompGruppoFrut> compList = new List<CompGruppoFrut>();
        public List<GruppiFrutti> gruppiList = new List<GruppiFrutti>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDdlGruppi();
            }
        }

        #region Helpers
        protected void FillDdlFrutti()
        {
            ddlScegliFrutto.Items.Clear();
            ddlScegliFrutto.Items.Add(new ListItem("", "-1"));
            DropDownListManager.FillDdlFrutti(FruttiDAO.GetFrutti(), ref ddlScegliFrutto);
        }

        protected void FillDdlGruppi()
        {
            ddlScegliGruppo.Items.Clear();
            ddlScegliGruppo.Items.Add(new ListItem("", "-1"));
            DropDownListManager.FillDdlGruppi(GruppiFruttiDAO.GetGruppi(txtFiltroGruppo1.Text, txtFiltroGruppo2.Text, txtFiltroGruppo3.Text), ref ddlScegliGruppo);
        }
        #endregion

        #region TextChanged
        protected void txtFiltroGruppo_TextChanged(object sender, EventArgs e)
        {
            FillDdlGruppi();
        }

        protected void ddlScegliGruppo_TextChanged(object sender, EventArgs e)
        {
            pnlAssociaFruttoAGruppo.Visible = true;
            FillDdlFrutti();
        }
        #endregion

        #region Click
        protected void btnInserisciFruttoInGruppo_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void btnModificaFruttoInGruppo_Click(object sender, EventArgs e)
        {
        }
        #endregion
    }
}