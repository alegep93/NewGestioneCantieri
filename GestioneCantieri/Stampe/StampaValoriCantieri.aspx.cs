using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GestioneCantieri
{
    public partial class StampaValoriCantieri : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDdlScegliCantiere();
                btnStampaValoriCantieri.Visible = false;
                pnlRisultati.Visible = false;
            }
        }

        #region Helpers
        protected void FillDdlScegliCantiere()
        {
            ddlScegliCant.Items.Clear();
            ddlScegliCant.Items.Add(new System.Web.UI.WebControls.ListItem("", "-1"));
            CantieriDAO.GetCantieri(txtAnno.Text, txtCodCant.Text, chkFatturato.Checked, chkChiuso.Checked, chkRiscosso.Checked).ForEach(f =>
            {
                ddlScegliCant.Items.Add(new System.Web.UI.WebControls.ListItem($"{f.CodCant} - {f.DescriCodCant}", f.IdCantieri.ToString()));
            });
        }
        protected void CompilaCampi(string idCantiere)
        {

            //Popolo il campo Conto/Preventivo
            Cantieri c = CantieriDAO.GetSingle(Convert.ToInt32(idCantiere));
            txtContoPreventivo.Text = c.Preventivo ? string.Format("{0:n}", c.ValorePreventivo) : Math.Round(RicalcoloConti.totRicalcoloConti, 2).ToString();

            //Popolo il campo Tot. Acconti
            decimal totAcconti = 0m;
            List<Pagamenti> pagList = PagamentiDAO.GetAll().Where(w => w.IdTblCantieri == Convert.ToInt32(idCantiere)).ToList();
            foreach (Pagamenti p in pagList)
            {
                totAcconti += p.Imporo;
            }
            txtTotPagamenti.Text = String.Format("{0:n}", totAcconti).ToString();

            //Popolo il campo Tot. Finale
            decimal totContoPreventivo = Convert.ToDecimal(txtContoPreventivo.Text);
            decimal totFin = totContoPreventivo - totAcconti;
            txtTotFinale.Text = String.Format("{0:n}", totFin).ToString();
        }
        #endregion

        #region Eventi Click
        protected void btnFiltraCantieri_Click(object sender, EventArgs e)
        {
            FillDdlScegliCantiere();
        }
        protected void btnStampaContoCliente_Click(object sender, EventArgs e)
        {
            //Ricreo i passaggi della "Stampa Ricalcolo Conti" per ottenere il valore del "Totale Ricalcolo"
            string idCantiere = ddlScegliCant.SelectedItem.Value;
            Cantieri cant = CantieriDAO.GetSingle(Convert.ToInt32(idCantiere));
            MaterialiCantieri mc = new MaterialiCantieri
            {
                RagSocCli = cant.RagSocCli,
                CodCant = cant.CodCant,
                DescriCodCant = cant.DescriCodCant
            };
            RicalcoloConti rc = new RicalcoloConti();
            decimal totale = 0m;
            PdfPTable pTable = rc.InitializePdfTableDDT();
            Document pdfDoc = new Document(PageSize.A4, 8f, 2f, 2f, 2f);
            pdfDoc.Open();
            rc.idCant = ddlScegliCant.SelectedItem.Value;
            List<MaterialiCantieri> materiali = rc.GetMaterialiCantieri();
            //rc.BindGridPDF(grdStampaMateCant, grdStampaMateCantPDF);
            rc.GeneraPDFPerContoFinCli(pdfDoc, mc, pTable, materiali, totale);
            pdfDoc.Close();

            //Popolo i campi di riepilogo con i dati necessari
            CompilaCampi(idCantiere);
        }
        #endregion

        #region Eventi Text-Changed
        protected void ddlScegliCant_TextChanged(object sender, EventArgs e)
        {
            if (ddlScegliCant.SelectedIndex != 0)
            {
                btnStampaValoriCantieri.Visible = true;
                pnlRisultati.Visible = true;
                txtContoPreventivo.Text = txtTotPagamenti.Text = txtTotFinale.Text = "";
            }
            else
            {
                btnStampaValoriCantieri.Visible = false;
                pnlRisultati.Visible = false;
            }
        }
        #endregion
    }
}