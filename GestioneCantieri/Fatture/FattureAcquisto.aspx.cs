using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using GestioneCantieri.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class FattureAcquisto : System.Web.UI.Page
    {
        public double totaleImporto = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ResetToInitial(false);
            }
        }

        private void ResetToInitial(bool needToUpdateGrid = true)
        {
            txtFiltroGrdAnno.Text = DateTime.Now.Year.ToString();
            txtFiltroGrdFornitore.Text = txtFiltroGrdDataDa.Text = txtFiltroGrdDataA.Text = txtFiltroGrdNumeroFattura.Text = "";
            txtFiltroFornitore.Text = "";
            ddlScegliFornitore.SelectedValue = "-1";
            txtNumeroFattura.Text = txtData.Text = "";
            txtImponibile.Text = txtRitenutaAcconto.Text = txtConcatenazione.Text = "";
            chkNotaCredito.Checked = chkReverseCharge.Checked = false;
            txtIva.Text = "22";
            btnInsFattura.Visible = true;
            btnModFattura.Visible = false;
            txtData.ReadOnly = false;
            hfIdFattura.Value = "";
            SetNumeroFattura();
            FillDdlScegliFornitore();
            BindGrid(needToUpdateGrid);
        }

        protected void BindGrid(bool needToUpdateGrid)
        {
            if (needToUpdateGrid)
            {
                int numeroFattura = txtFiltroGrdNumeroFattura.Text != "" ? Convert.ToInt32(txtFiltroGrdNumeroFattura.Text) : 0;
                int anno = txtFiltroGrdAnno.Text == "" ? DateTime.Now.Year : Convert.ToInt32(txtFiltroGrdAnno.Text);

                List<FatturaAcquisto> fatture = FattureAcquistoDAO.GetFattureAcquisto(txtFiltroGrdAnno.Text, txtFiltroGrdDataDa.Text, txtFiltroGrdDataA.Text, txtFiltroGrdFornitore.Text, numeroFattura);
                grdFatture.DataSource = fatture;
                grdFatture.DataBind();

                grdTotaleIvaPerQuarter.DataSource = FattureAcquistoDAO.GetTotaliIvaPerQuarter(anno).Select(s => new
                {
                    Trimestre = s.quarter,
                    TotaleIva = s.totaleIva.ToString("N2")
                });
                grdTotaleIvaPerQuarter.DataBind();

                grdTotaleImponibilePerQuarter.DataSource = FattureAcquistoDAO.GetTotaliImponibilePerQuarter(anno).Select(s => new
                {
                    Trimestre = s.quarter,
                    TotaleIva = s.totaleIva.ToString("N2")
                });
                grdTotaleImponibilePerQuarter.DataBind();

                grdTotaleImportoPerQuarter.DataSource = FattureAcquistoDAO.GetTotaliImportoPerQuarter(anno).Select(s => new
                {
                    Trimestre = s.quarter,
                    TotaleIva = s.totaleIva.ToString("N2")
                });
                grdTotaleImportoPerQuarter.DataBind();

                grdTotali.DataSource = FattureAcquistoDAO.GetTotaliFatture(txtFiltroGrdFornitore.Text, txtFiltroGrdAnno.Text, numeroFattura, txtFiltroGrdDataDa.Text, txtFiltroGrdDataA.Text).Select(s => new
                {
                    Titolo = s.titolo,
                    Valore = s.valore.ToString("N2")
                });
                grdTotali.DataBind();
            }
        }

        private void SetNumeroFattura(int year = 0)
        {
            string numeroFattura = "";
            string nuovoNumeroFattura = ""; 
            List<FatturaAcquisto> fatture = FattureAcquistoDAO.GetAll().Where(w => w.Data.Year == (year > 0 ? year : DateTime.Now.Year)).ToList();
            if (fatture.Count() == 0)
            {
                numeroFattura = "001";
            }
            else
            {
                nuovoNumeroFattura = (fatture.Select(s => s.Numero).Max() + 1).ToString();
                if (nuovoNumeroFattura.Length == 1)
                {
                    numeroFattura = "00";
                }
                if (nuovoNumeroFattura.Length == 2)
                {
                    numeroFattura = "0";
                }
            }
            txtNumeroFattura.Text = $"{numeroFattura} {nuovoNumeroFattura}";
        }

        protected void FillDdlScegliFornitore(string ragSocFornitore = "")
        {
            ddlScegliFornitore.Items.Clear();
            ddlScegliFornitore.Items.Add(new ListItem("", "-1"));
            DropDownListManager.FillDdlFornitore(FornitoriDAO.GetFornitori(ragSocFornitore), ref ddlScegliFornitore);
        }

        private FatturaAcquisto PopolaFatturaObj()
        {
            return new FatturaAcquisto
            {
                IdFattureAcquisto = hfIdFattura.Value != "" ? Convert.ToInt32(hfIdFattura.Value) : 0,
                IdFornitore = Convert.ToInt32(ddlScegliFornitore.SelectedValue),
                Numero = txtNumeroFattura.Text != "" ? Convert.ToInt32(txtNumeroFattura.Text) : 0,
                Data = Convert.ToDateTime(txtData.Text),
                Imponibile = (chkNotaCredito.Checked ? -1 : 1) * (txtImponibile.Text != "" ? Convert.ToDouble(txtImponibile.Text.Replace(".", ",")) : 0),
                Iva = txtIva.Text != "" ? Convert.ToInt32(txtIva.Text) : 0,
                RitenutaAcconto = txtRitenutaAcconto.Text != "" ? Convert.ToInt32(txtRitenutaAcconto.Text) : 0,
                ReverseCharge = chkReverseCharge.Checked,
                IsNotaDiCredito = chkNotaCredito.Checked
                //FilePath = Server.MapPath(fuUploadFattura.FileName)
            };
        }

        private void PopolaCampi(int idFatturaAcquisto, bool isModifica)
        {
            FatturaAcquisto fatt = FattureAcquistoDAO.GetSingle(idFatturaAcquisto);

            txtNumeroFattura.Text = fatt.Numero.ToString();
            ddlScegliFornitore.SelectedValue = fatt.IdFornitore.ToString();
            txtData.Text = fatt.Data.ToString("yyyy-MM-dd");
            txtData.TextMode = TextBoxMode.Date;
            txtImponibile.Text = fatt.Imponibile.ToString();
            txtRitenutaAcconto.Text = fatt.RitenutaAcconto.ToString();
            txtIva.Text = fatt.Iva.ToString();
            chkNotaCredito.Checked = fatt.IsNotaDiCredito;
            chkReverseCharge.Checked = fatt.ReverseCharge;
            txtConcatenazione.Text = $"Fat. {fatt.Numero.ToString()} del {fatt.Data.ToString("dd/MM/yyyy")}";

            // Accessibilità campi
            txtNumeroFattura.ReadOnly = txtData.ReadOnly = !isModifica;
            txtImponibile.ReadOnly = txtRitenutaAcconto.ReadOnly = txtIva.ReadOnly = !isModifica;
            txtFiltroFornitore.ReadOnly = !isModifica;
            chkNotaCredito.Enabled = chkReverseCharge.Enabled = isModifica;
            ddlScegliFornitore.Enabled = isModifica;

            // Visibilità pannelli
            pnlInsFatture.Visible = true;
            pnlRicercaFatture.Visible = !pnlInsFatture.Visible;
        }

        protected void btnInsFattura_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlScegliFornitore.SelectedIndex > 0 && txtImponibile.Text != "" && txtData.Text != "")
                {
                    // Inserisco la fattura
                    FattureAcquistoDAO.Insert(PopolaFatturaObj());
                    lblMessaggio.ForeColor = Color.Blue;
                    lblMessaggio.Text = "Fattura Acquisto inserita con successo";

                    ResetToInitial();
                }
                else
                {
                    lblMessaggio.ForeColor = Color.Red;
                    lblMessaggio.Text = "I campi Fornitore e Imponibile devono essere compilati";
                }
            }
            catch (Exception ex)
            {
                lblMessaggio.ForeColor = Color.Red;
                lblMessaggio.Text = $"Errore durante l'inserimento della Fattura Acquisto ===> {ex.Message}";
            }
        }

        protected void btnModFattura_Click(object sender, EventArgs e)
        {
            try
            {
                if (FattureAcquistoDAO.Update(PopolaFatturaObj()))
                {
                    lblMessaggio.ForeColor = Color.Blue;
                    lblMessaggio.Text = "Fattura Acquisto aggiornata con successo";
                }
                else
                {
                    lblMessaggio.ForeColor = Color.Red;
                    lblMessaggio.Text = "Errore durante l'aggiornamento della Fattura";
                }
            }
            catch (Exception ex)
            {
                lblMessaggio.ForeColor = Color.Red;
                lblMessaggio.Text = $"Errore durante l'aggiornamento del Fattura Acquisto ===> {ex.Message}";
            }

            ResetToInitial();
        }

        protected void grdFatture_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idFattura = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "Visualizza")
            {
                ResetToInitial();
                PopolaCampi(idFattura, false);
                btnInsFattura.Visible = btnModFattura.Visible = false;
            }
            else if (e.CommandName == "Modifica")
            {
                ResetToInitial();
                PopolaCampi(idFattura, true);
                btnInsFattura.Visible = false;
                btnModFattura.Visible = !btnInsFattura.Visible;
                hfIdFattura.Value = idFattura.ToString();
            }
            else if (e.CommandName == "Elimina")
            {
                bool isDeleted = false;
                try
                {
                    FattureAcquistoDAO.Delete(idFattura);
                    isDeleted = true;
                }
                catch (Exception)
                {
                    lblMessaggio.Text = "Errore durante l'eliminazione di una fattura acquisto";
                    lblMessaggio.ForeColor = Color.Red;
                }

                if (isDeleted)
                {
                    lblMessaggio.ForeColor = Color.Blue;
                    lblMessaggio.Text = "Fattura eliminato con successo";
                }
                else
                {
                    lblMessaggio.ForeColor = Color.Red;
                    lblMessaggio.Text = "Errore durante l'eliminazione della Fattura acquisto";
                }

                ResetToInitial();
            }
        }

        protected void btnFiltraFatture_Click(object sender, EventArgs e)
        {
            BindGrid(true);
        }

        protected void btnSvuotaFiltri_Click(object sender, EventArgs e)
        {
            ResetToInitial();
        }

        protected void btnInserisciFatture_Click(object sender, EventArgs e)
        {
            pnlInsFatture.Visible = true;
            pnlRicercaFatture.Visible = !pnlInsFatture.Visible;
        }

        protected void btnRicercaFatture_Click(object sender, EventArgs e)
        {
            pnlInsFatture.Visible = false;
            pnlRicercaFatture.Visible = !pnlInsFatture.Visible;
            txtFiltroGrdAnno.Text = DateTime.Now.Year.ToString();
            BindGrid(true);
        }

        protected void grdFatture_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[3].Text = "Imponibile";
                e.Row.Cells[6].Text = "Valore Iva";
                e.Row.Cells[7].Text = "Valore Rit. Acc.";
                e.Row.Cells[8].Text = "Imp. Fattura";
                e.Row.Cells[9].Text = "Reverse Charge";
                e.Row.Cells[10].Text = "Nota Credito";
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                long rowIdFattura = Convert.ToInt64((e.Row.FindControl("hfRowIdFattura") as HiddenField).Value);
                FatturaAcquisto fattura = FattureAcquistoDAO.GetSingle(rowIdFattura);

                double imponibile = fattura.Imponibile;
                double valoreIva = imponibile * fattura.Iva / 100;
                double valoreRitenutaAcconto = imponibile * fattura.RitenutaAcconto / 100;

                (e.Row.FindControl("lblValoreIva") as Label).Text = valoreIva.ToString("N2");
                (e.Row.FindControl("lblValoreRitenutaAcconto") as Label).Text = valoreRitenutaAcconto.ToString("N2");

                double importoFattura = imponibile + valoreIva - valoreRitenutaAcconto;
                (e.Row.FindControl("lblImportoFattura") as Label).Text = importoFattura.ToString("N2");

                // Scrivo i totali generali
                totaleImporto += importoFattura;
            }
        }

        protected void txtData_TextChanged(object sender, EventArgs e)
        {
            SetNumeroFattura(Convert.ToDateTime(txtData.Text).Year);
        }

        protected void txtFiltroFornitore_TextChanged(object sender, EventArgs e)
        {
            FillDdlScegliFornitore(txtFiltroFornitore.Text);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }
    }
}