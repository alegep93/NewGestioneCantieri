using Database.DAO;
using Database.Models;
using GestioneCantieri.Utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class RicalcoloConti : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnStampaContoCliente.Visible = false;
                FillDdlScegliCantiere();
            }
        }

        #region Helpers
        protected void FillDdlScegliCantiere()
        {
            ddlScegliCant.Items.Clear();
            ddlScegliCant.Items.Add(new System.Web.UI.WebControls.ListItem("", "-1"));
            DropDownListManager.FillDdlCantieri(CantieriDAO.GetCantieri(txtAnno.Text, txtCodCant.Text, "", chkChiuso.Checked, chkRiscosso.Checked), ref ddlScegliCant);
        }

        public void BindGridExcel()
        {
            grdStampaMateCantExcel.DataSource = RicalcoloContiManager.GetMaterialiCantieri(Convert.ToInt32(ddlScegliCant.SelectedValue));
            grdStampaMateCantExcel.DataBind();
        }

        protected void CreateExcel()
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=RicalcoloConti-" + ddlScegliCant.SelectedItem.Text + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

            htmlWrite.WriteLine("<strong><font size='4'>" + ddlScegliCant.SelectedItem.Text + "</font></strong>");

            // viene reindirizzato il rendering verso la stringa in uscita
            grdStampaMateCantExcel.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());

            Response.End();
        }
        #endregion

        #region Stampa PDF
        //Stampa PDF
        
        public void ExportToPdfPerContoFinCli(List<MaterialiCantieri> matCantList)
        {
            decimal totale = 0m;
            int idCantiere = Convert.ToInt32(ddlScegliCant.SelectedValue);
            Cantieri cant = CantieriDAO.GetSingle(idCantiere);
            MaterialiCantieri mc = new MaterialiCantieri
            {
                RagSocCli = cant.RagSocCli,
                CodCant = cant.CodCant,
                DescriCodCant = cant.DescriCodCant
            };

            //Apro lo stream verso il file PDF
            Document pdfDoc = new Document(PageSize.A4, 8f, 2f, 2f, 2f);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            PdfPTable table = RicalcoloContiManager.InitializePdfTableDDT();

            Phrase title = new Phrase($"Ragione Sociale Cliente: {mc.RagSocCli}", FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
            pdfDoc.Add(title);

            RicalcoloContiManager.GeneraPDFPerContoFinCli(pdfDoc, mc, table, matCantList, totale, idCantiere, ddlScegliTipoNote.SelectedValue);

            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + mc.RagSocCli + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
        }        
        #endregion

        #region Eventi Click
        /* EVENTI CLICK */
        protected void btnStampaContoCliente_Click(object sender, EventArgs e)
        {
            List<MaterialiCantieri> matCantList = RicalcoloContiManager.GetMaterialiCantieri(Convert.ToInt32(ddlScegliCant.SelectedValue));

            if (matCantList == null)
            {
                lblControlloMatVisNasc.Text = "Materiale visibile con ricalcolo = 0, ma è presente del Materiale nascosto. --- Oppure sono presenti record con PzzoFinCli.";
                lblControlloMatVisNasc.ForeColor = Color.Red;
            }
            else
            {
                ExportToPdfPerContoFinCli(matCantList);
            }
        }
        protected void btnFiltraCantieri_Click(object sender, EventArgs e)
        {
            FillDdlScegliCantiere();
        }
        protected void btnStampaExcel_Click(object sender, EventArgs e)
        {
            BindGridExcel();
            CreateExcel();
        }
        #endregion

        #region Eventi Text-Changed
        /* EVENTI TEXT-CHANGED */
        protected void ddlScegliCant_TextChanged(object sender, EventArgs e)
        {
            btnStampaContoCliente.Visible = ddlScegliCant.SelectedIndex != 0;
        }
        #endregion

        /* Override per il corretto funzionamento della creazione del foglio Excel */
        public override void VerifyRenderingInServerForm(Control control)
        {
            //Do nothing
        }
    }
}