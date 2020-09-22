using GestioneCantieri.DAO;
using GestioneCantieri.Utils;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class StampaExcell : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnPrint.Visible = false;
                ddlScegliCantiere.SelectedIndex = 0;
                FillDdlScegliCantiere();
            }
        }

        #region Eventi Click
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            CreateExcel();
        }
        #endregion

        #region Eventi Text-Changed
        protected void ddlScegliCantiere_TextChanged(object sender, EventArgs e)
        {
            BindGrid();
            btnPrint.Visible = ddlScegliCantiere.SelectedIndex != 0;
        }
        #endregion

        #region Helpers
        protected void BindGrid()
        {
            grdFruttiInLocale.DataSource = OrdineFruttiDAO.GetFruttiPerStampaExcel(ddlScegliCantiere.SelectedItem.Value);
            grdFruttiInLocale.DataBind();
        }

        protected void CreateExcel()
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=OrdFrut-" + ddlScegliCantiere.SelectedItem.Text + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            htmlWrite.WriteLine("<strong><font size='4'>" + ddlScegliCantiere.SelectedItem.Text + "</font></strong>");
            // viene reindirizzato il rendering verso la stringa in uscita
            grdFruttiInLocale.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }

        protected void FillDdlScegliCantiere()
        {
            ddlScegliCantiere.Items.Clear();
            ddlScegliCantiere.Items.Add(new ListItem("", "-1"));
            CantiereManager.FillDdlCantieri(CantieriDAO.GetAll().Where(w => !w.Chiuso).ToList(), ref ddlScegliCantiere);
        }
        #endregion

        /* Override per il corretto funzionamento della creazione del foglio Excel */
        public override void VerifyRenderingInServerForm(Control control)
        {
            //Do nothing
        }
    }

}