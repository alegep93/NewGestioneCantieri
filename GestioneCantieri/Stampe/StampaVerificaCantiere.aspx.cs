using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using GestioneCantieri.Utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
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
            DropDownListManager.FillDdlCantieri(CantieriDAO.GetCantieri(txtAnno.Text, txtCodCant.Text, "", chkChiuso.Checked, chkRiscosso.Checked),ref ddlScegliCant);
        }
        protected void BindGrid()
        {
            decimal valore = 0m;
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

            Cantieri cant = CantieriDAO.GetSingle(idCantiere);
            MaterialiCantieri mc = new MaterialiCantieri
            {
                RagSocCli = cant.RagSocCli,
                CodCant = cant.CodCant,
                DescriCodCant = cant.DescriCodCant
            };

            lblIntestStampa.Text = "<strong>CodCant</strong>: " + mc.CodCant + " --- " +
                "<strong>DescriCodCant</strong>: " + mc.DescriCodCant + " --- " +
                "<strong>Cliente</strong>: " + mc.RagSocCli;

            Cantieri c = CantieriDAO.GetSingle(idCantiere);
            lblTotContoCliente.Text = "<strong>Tot. Conto/Preventivo</strong>: ";

            if (c.Preventivo)
                lblTotContoCliente.Text += Math.Round(c.ValorePreventivo, 2).ToString();
            else
                lblTotContoCliente.Text += Math.Round(RicalcoloConti.totRicalcoloConti, 2).ToString();

            for (int i = 0; i < matCantList.Count; i++)
            {
                valore = Convert.ToDecimal(grdStampaVerificaCant.Rows[i].Cells[4].Text) * Convert.ToDecimal(grdStampaVerificaCant.Rows[i].Cells[5].Text);
                grdStampaVerificaCant.Rows[i].Cells[6].Text = valore.ToString();
            }

            foreach (MaterialiCantieri matCant in matCantList)
            {
                decimal val = Convert.ToDecimal(matCant.Qta) * matCant.PzzoUniCantiere;
                decimal totOre = Convert.ToDecimal(matCant.Qta);
                switch (matCant.Tipologia)
                {
                    case "MATERIALE":
                        totMate += val;
                        break;
                    case "RIENTRO":
                        totRientro += val;
                        break;
                    case "MANODOPERA":
                        totManodop += val;
                        totOreManodop += totOre;
                        break;
                    case "OPERAIO":
                        totOper += val;
                        break;
                    case "ARROTONDAMENTO":
                        totArrot += val;
                        break;
                    case "SPESE":
                    case "SPESA":
                        totSpese += val;
                        break;
                    case "A CHIAMATA":
                        totChiam += val;
                        break;
                }
            }

            lblTotMate.Text = "<strong>Tot. Materiale</strong>: " + String.Format("{0:n}", totMate).ToString();
            lblTotRientro.Text = "<strong>Tot. Rientro</strong>: " + String.Format("{0:n}", totRientro).ToString();
            lblTotOper.Text = "<strong>Tot. Operaio</strong>: " + String.Format("{0:n}", totOper).ToString();
            lblTotArrot.Text = "<strong>Tot. Arrotondamento</strong>: " + String.Format("{0:n}", totArrot).ToString();
            lblTotAChiamata.Text = "<strong>Tot. A Chiamata</strong>: " + String.Format("{0:n}", totChiam).ToString();
            lblTotSpese.Text = "<strong>Tot. Spese</strong>: " + String.Format("{0:n}", totSpese).ToString();

            totManodop = Convert.ToDecimal(String.Format("{0:n}", totManodop));

            decimal sommaTotPerTipol = totMate + totRientro + totOper;
            decimal contoFinCli = Convert.ToDecimal(lblTotContoCliente.Text.Split(':')[1].Trim());
            decimal totGuadagno = Convert.ToDecimal(String.Format("{0:n}", contoFinCli - sommaTotPerTipol - totManodop + totArrot + totChiam));

            lblTotGuadagno.Text = "<strong>Totale Guadagno</strong>: " + totGuadagno;
            lblTotManodop.Text = "<strong>Tot. Manodopera</strong>: " + totManodop;

            decimal totGuadConManodop = totGuadagno + totManodop;
            lblTotGuadagnoConManodop.Text = "<strong>Tot. Guadagno Con Manodopopera</strong>: " + totGuadConManodop;

            decimal totGuadOrarioManodop = totGuadConManodop / (totOreManodop == 0 ? 1 : totOreManodop);
            lblTotGuadagnoOrarioManodop.Text = "<strong>Tot. Guadagno Orario Manodopopera</strong>: " + String.Format("{0:n}", totGuadOrarioManodop);
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
            rc.idCant = idCantiere;
            List<MaterialiCantieri> materiali = rc.GetMaterialiCantieri();
            //rc.BindGridPDF(grdStampaMateCant, grdStampaMateCantPDF);
            rc.GeneraPDFPerContoFinCli(pdfDoc, mc, pTable, materiali, totale);
            pdfDoc.Close();

            BindGrid();
            GroupGridViewCells();
        }
        #endregion

        #region Eventi Text-Changed
        /* EVENTI TEXT-CHANGED */
        protected void ddlScegliCant_TextChanged(object sender, EventArgs e)
        {
            if (ddlScegliCant.SelectedIndex != 0)
            {
                btnStampaVerificaCant.Visible = true;
            }
            else
            {
                btnStampaVerificaCant.Visible = false;
            }
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
            BindGrid();
        }
        #endregion
    }
}