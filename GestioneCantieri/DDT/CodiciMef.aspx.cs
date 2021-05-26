using Database.DAO;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace GestioneCantieri
{
    public partial class CodiciMef : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtAnnoDa.Text = txtAnnoA.Text = DateTime.Now.Year.ToString();
                BindGrid();
            }
        }

        private void BindGrid()
        {
            grdCodiciMef.DataSource = DDTMefDAO.GetCodiciMef(Convert.ToInt32(txtAnnoDa.Text), Convert.ToInt32(txtAnnoA.Text));
            grdCodiciMef.DataBind();
        }

        protected void txtAnno_TextChanged(object sender, EventArgs e)
        {
            BindGrid();
        }
    }
}