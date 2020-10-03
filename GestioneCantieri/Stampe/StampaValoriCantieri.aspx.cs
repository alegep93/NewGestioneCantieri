using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using GestioneCantieri.Utils;
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
            DropDownListManager.FillDdlCantieri(CantieriDAO.GetCantieri(txtAnno.Text, txtCodCant.Text, chkFatturato.Checked, chkChiuso.Checked, chkRiscosso.Checked), ref ddlScegliCant);
        }

        protected void CompilaCampi(int idCantiere, decimal totale)
        {
            //Popolo il campo Conto/Preventivo
            Cantieri c = CantieriDAO.GetSingle(idCantiere);
            txtContoPreventivo.Text = c.Preventivo ? string.Format("{0:n}", c.ValorePreventivo) : Math.Round(totale, 2).ToString();

            //Popolo il campo Tot. Acconti
            decimal totAcconti = 0m;
            totAcconti = PagamentiDAO.GetAll().Where(w => w.IdTblCantieri == idCantiere).ToList().Sum(s => s.Imporo);
            txtTotPagamenti.Text = $"{totAcconti:n}";

            //Popolo il campo Tot. Finale
            decimal totContoPreventivo = Convert.ToDecimal(txtContoPreventivo.Text);
            decimal totFin = totContoPreventivo - totAcconti;
            txtTotFinale.Text = $"{totFin:n}";
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
            int idCantiere = Convert.ToInt32(ddlScegliCant.SelectedItem.Value);
            Cantieri cant = CantieriDAO.GetSingle(idCantiere);
            MaterialiCantieri mc = new MaterialiCantieri
            {
                RagSocCli = cant.RagSocCli,
                CodCant = cant.CodCant,
                DescriCodCant = cant.DescriCodCant
            };

            PdfPTable pTable = RicalcoloContiManager.InitializePdfTableDDT();
            Document pdfDoc = new Document(PageSize.A4, 8f, 2f, 2f, 2f);
            pdfDoc.Open();
            List<MaterialiCantieri> materiali = RicalcoloContiManager.GetMaterialiCantieri(idCantiere);
            RicalcoloContiManager.GeneraPDFPerContoFinCli(pdfDoc, mc, pTable, materiali, 0, idCantiere);
            pdfDoc.Close();

            //Popolo i campi di riepilogo con i dati necessari
            CompilaCampi(idCantiere, materiali.Sum(s => s.Valore));
        }
        #endregion

        #region Eventi Text-Changed
        protected void ddlScegliCant_TextChanged(object sender, EventArgs e)
        {
            if (ddlScegliCant.SelectedIndex != 0)
            {
                btnStampaValoriCantieri.Visible = pnlRisultati.Visible = true;
                txtContoPreventivo.Text = txtTotPagamenti.Text = txtTotFinale.Text = "";
            }
            else
            {
                btnStampaValoriCantieri.Visible = pnlRisultati.Visible = false;
            }
        }
        #endregion
    }
}