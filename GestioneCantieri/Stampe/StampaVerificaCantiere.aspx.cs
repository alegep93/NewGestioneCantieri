using Database.DAO;
using Database.Models;
using GestioneCantieri.Utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class StampaVerificaCantiere : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDdlScegliCantiere();
                btnStampaVerificaCant.Visible = false;
            }
        }

        #region Helpers
        protected void FillDdlScegliCantiere()
        {
            ddlScegliCant.Items.Clear();
            ddlScegliCant.Items.Add(new System.Web.UI.WebControls.ListItem("", "-1"));
            DropDownListManager.FillDdlCantieri(CantieriDAO.GetCantieri(txtAnno.Text, txtCodCant.Text, "", chkChiuso.Checked, chkRiscosso.Checked), ref ddlScegliCant);
        }
        protected void BindGrid(Cantieri cant, decimal totaleValore)
        {
            decimal totMate = 0m;
            decimal totRientro = 0m;
            decimal totManodop = 0m;
            decimal totOreManodop = 0m;
            decimal totOper = 0m;
            decimal totArrot = 0m;
            decimal totChiam = 0m;
            decimal totSpese = 0m;
            int idCantiere = Convert.ToInt32(ddlScegliCant.SelectedItem.Value);

            List<MaterialiCantieri> matCantList = MaterialiCantieriDAO.GetMaterialeCantiere(idCantiere.ToString());
            grdStampaVerificaCant.DataSource = matCantList;
            grdStampaVerificaCant.DataBind();

            lblIntestStampa.Text = $"<strong>CodCant</strong>: {cant.CodCant} --- <strong>DescriCodCant</strong>: {cant.DescriCodCant} --- <strong>Cliente</strong>: {cant.RagSocCli}";
            lblTotContoCliente.Text = "<strong>Tot. Conto/Preventivo</strong>: ";
            lblTotContoCliente.Text += (cant.Preventivo ? Math.Round(cant.ValorePreventivo, 2) : Math.Round(totaleValore, 2)).ToString();

            totMate += matCantList.Where(w => w.Tipologia.ToUpper() == "MATERIALE").Sum(s => (decimal)s.Qta * s.PzzoUniCantiere);
            totRientro += matCantList.Where(w => w.Tipologia.ToUpper() == "RIENTRO").Sum(s => (decimal)s.Qta * s.PzzoUniCantiere);
            totManodop += matCantList.Where(w => w.Tipologia.ToUpper() == "MANODOPERA").Sum(s => (decimal)s.Qta * s.PzzoUniCantiere);
            totOreManodop += matCantList.Where(w => w.Tipologia.ToUpper() == "MANODOPERA").Sum(s => (decimal)s.Qta);
            totOper += matCantList.Where(w => w.Tipologia.ToUpper() == "OPERAIO").Sum(s => (decimal)s.Qta * s.PzzoUniCantiere);
            totArrot += matCantList.Where(w => w.Tipologia.ToUpper() == "ARROTONDAMENTO").Sum(s => (decimal)s.Qta * s.PzzoUniCantiere);
            totSpese += matCantList.Where(w => w.Tipologia.ToUpper() == "SPESA" || w.Tipologia.ToUpper() == "SPESE").Sum(s => (decimal)s.Qta * s.PzzoUniCantiere);
            totChiam += matCantList.Where(w => w.Tipologia.ToUpper() == "A CHIAMATA").Sum(s => (decimal)s.Qta * s.PzzoUniCantiere);
            lblTotMate.Text = $"<strong>Tot. Materiale</strong>: {totMate:n}";
            lblTotRientro.Text = $"<strong>Tot. Rientro</strong>: {totRientro:n}";
            lblTotOper.Text = $"<strong>Tot. Operaio</strong>: {totOper:n}";
            lblTotArrot.Text = $"<strong>Tot. Arrotondamento</strong>: {totArrot:n}";
            lblTotAChiamata.Text = $"<strong>Tot. A Chiamata</strong>: {totChiam:n}";
            lblTotSpese.Text = $"<strong>Tot. Spese</strong>: {totSpese:n}";

            decimal sommaTotPerTipol = totMate + totRientro + totOper;
            decimal contoFinCli = Convert.ToDecimal(lblTotContoCliente.Text.Split(':')[1].Trim());
            decimal totGuadagno = Convert.ToDecimal($"{contoFinCli - sommaTotPerTipol - totManodop + totArrot + totChiam:n}");

            lblTotGuadagno.Text = $"<strong>Totale Guadagno</strong>: {totGuadagno:n}";
            lblTotManodop.Text = $"<strong>Tot. Manodopera</strong>: {totManodop:n}";

            decimal totGuadConManodop = totGuadagno + totManodop;
            lblTotGuadagnoConManodop.Text = $"<strong>Tot. Guadagno Con Manodopopera</strong>: {totGuadConManodop}";
            lblTotGuadagnoOrarioManodop.Text = $"<strong>Tot. Guadagno Orario Manodopopera</strong>: {totGuadConManodop / (totOreManodop == 0 ? 1 : totOreManodop):n}";
        }
        #endregion

        #region Eventi Click
        protected void btnFiltraCantieri_Click(object sender, EventArgs e)
        {
            FillDdlScegliCantiere();
        }
        protected void btnStampaVerificaCant_Click(object sender, EventArgs e)
        {
            //Ricreo i passaggi della "Stampa Ricalcolo Conti" per ottenere il valore del "Totale Ricalcolo"
            //MaterialiCantieri mc = new MaterialiCantieri
            //{
            //    RagSocCli = cant.RagSocCli,
            //    CodCant = cant.CodCant,
            //    DescriCodCant = cant.DescriCodCant
            //};
            //PdfPTable pTable = RicalcoloContiManager.InitializePdfTableDDT();
            //Document pdfDoc = new Document(PageSize.A4, 8f, 2f, 2f, 2f);
            //pdfDoc.Open();
            //RicalcoloContiManager.GeneraPDFPerContoFinCli(pdfDoc, mc, pTable, materiali, 0, idCantiere);
            //pdfDoc.Close();

            int idCantiere = Convert.ToInt32(ddlScegliCant.SelectedItem.Value);
            Cantieri cant = CantieriDAO.GetSingle(idCantiere);
            List<MaterialiCantieri> materiali = RicalcoloContiManager.GetMaterialiCantieri(idCantiere);
            pnlViewGridAndLabels.Visible = true;
            BindGrid(cant, materiali.Sum(s => s.Valore));
            GroupGridViewCells();
        }
        #endregion

        #region Eventi Text-Changed
        /* EVENTI TEXT-CHANGED */
        protected void ddlScegliCant_TextChanged(object sender, EventArgs e)
        {
            btnStampaVerificaCant.Visible = ddlScegliCant.SelectedIndex != 0;
            pnlViewGridAndLabels.Visible = false;
        }
        #endregion

        #region Gridview con Intestazioni dinamiche
        //Metodi per la gridView con intestazioni dinamiche
        protected void GroupGridViewCells()
        {
            GridViewHelper helper = new GridViewHelper(grdStampaVerificaCant);
            helper.RegisterGroup("Tipologia", true, true);
            helper.ApplyGroupSort();
        }
        /* Necessario per la creazione della GridView con intestazioni dinamiche */
        /* Definisce l'ordinamento dei dati presenti nella GridView */
        protected void grdStampaVerificaCant_Sorting(object sender, GridViewSortEventArgs e)
        {
            int idCantiere = Convert.ToInt32(ddlScegliCant.SelectedItem.Value);
            Cantieri cant = CantieriDAO.GetSingle(idCantiere);
            List<MaterialiCantieri> materiali = RicalcoloContiManager.GetMaterialiCantieri(idCantiere);
            BindGrid(cant, materiali.Sum(s => s.Valore));
        }
        #endregion
    }
}