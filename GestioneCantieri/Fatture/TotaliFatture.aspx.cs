using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class TotaliFatture : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetLabels();
        }

        private void SetLabels()
        {
            double totaleImponibileEmesso = 0, totaleFatturatoEmesso = 0, totaleImponibileAcquisto = 0, totaleFatturatoAcquisto = 0;
            int anno = txtFiltroAnno.Text != "" ? Convert.ToInt32(txtFiltroAnno.Text) : 0;
            List<Fattura> fatture = FattureDAO.GetAll();
            List<FatturaAcquisto> fattureAcquisto = FattureAcquistoDAO.GetAll();

            if (anno != 0)
            {
                fatture = fatture.Where(w => w.Data.Year == anno).ToList();
            }

            if (fatture.Count() > 0)
            {
                totaleImponibileEmesso = fatture.Sum(s => s.Imponibile);
                totaleFatturatoEmesso = fatture.Sum(s => s.Imponibile + (s.Imponibile * s.Iva / 100) - (s.Imponibile * s.RitenutaAcconto / 100));
            }

            if (fatture.Count() > 0)
            {
                totaleImponibileAcquisto = fattureAcquisto.Sum(s => s.Imponibile);
                totaleFatturatoAcquisto = fattureAcquisto.Sum(s => s.Imponibile + (s.Imponibile * s.Iva / 100) - (s.Imponibile * s.RitenutaAcconto / 100));
            }

            decimal totaleBollette = BolletteDAO.GetTotale(anno);

            // Fatture Emesse
            lblFattureEmesseTotaleImponibile.Text = $"Totale imponibile emesso: <strong>{totaleImponibileEmesso:N2}</strong>";
            lblFattureEmesseTotaleFatturato.Text = $"Totale fatturato emesso: <strong>{totaleFatturatoEmesso:N2}</strong>";

            // Fatture Acquisto
            lblFattureAcquistoTotaleImponibile.Text = $"Totale imponibile acquisto: <strong>{totaleImponibileAcquisto:N2}</strong>";
            lblFattureAcquistoTotaleFatturato.Text = $"Totale fatturato acquisto: <strong>{totaleFatturatoAcquisto:N2}</strong>";

            //Differenze
            lblDifferenzaTotaleImponibile.Text = $"Differenza totale imponibile: <strong>{totaleImponibileEmesso - totaleImponibileAcquisto:N2}</strong>";
            lblDifferenzaTotaleFatturato.Text = $"Differenza totale fatturato: <strong>{totaleFatturatoEmesso - totaleFatturatoAcquisto:N2}</strong>";

            // Bollette e Utile
            lblTotaleBollette.Text = $"Totale bollette: <strong>{totaleBollette:N2}</strong>";
            lblUtile.Text = $"Utile: <strong>{totaleImponibileEmesso - totaleImponibileAcquisto - Convert.ToDouble(totaleBollette / 2):N2}</strong>";
            hfUtile.Value = (totaleImponibileEmesso - totaleImponibileAcquisto - Convert.ToDouble(totaleBollette / 2)).ToString("N2");

            BindGrid(anno);
        }

        private void BindGrid(int anno)
        {
            grdTotaleIvaPerQuarter.DataSource = FattureAcquistoDAO.GetTotaliFatture(anno).Select(s => new
            {
                Trimestre = s.quarter,
                TotaleIvaAcquisto = s.totaleAcquisto,
                TotaleIvaEmesso = s.totaleEmesso,
                Saldo = s.saldo
            });
            grdTotaleIvaPerQuarter.DataBind();
        }

        protected void txtTassePerc_TextChanged(object sender, EventArgs e)
        {
            lblUtileNettoTasse.Text = $"Utile netto tasse: <strong>{Convert.ToDecimal(hfUtile.Value) * Convert.ToDecimal(txtTassePerc.Text) / 100}</strong>";
        }

        protected void txtFiltroAnno_TextChanged(object sender, EventArgs e)
        {
            SetLabels();
        }
    }
}