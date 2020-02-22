﻿using GestioneCantieri.DAO;
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
            int anno = txtFiltroAnno.Text != "" ? Convert.ToInt32(txtFiltroAnno.Text) : 0;

            double totaleImponibileEmesso = FattureDAO.GetTotaleImponibile(anno);
            double totaleFatturatoEmesso = FattureDAO.GetTotaleFatturato(anno);

            double totaleImponibileAcquisto = FattureAcquistoDAO.GetTotaleImponibile(anno);
            double totaleFatturatoAcquisto = FattureAcquistoDAO.GetTotaleFatturato(anno);

            decimal totaleBollette = BolletteDAO.GetTotale(anno);

            // Fatture Emesse
            lblFattureEmesseTotaleImponibile.Text =  $"Totale imponibile emesso: <strong>{totaleImponibileEmesso.ToString("N2")}</strong>";
            lblFattureEmesseTotaleFatturato.Text = $"Totale fatturato emesso: <strong>{totaleFatturatoEmesso.ToString("N2")}</strong>";

            // Fatture Acquisto
            lblFattureAcquistoTotaleImponibile.Text = $"Totale imponibile acquisto: <strong>{totaleImponibileAcquisto.ToString("N2")}</strong>";
            lblFattureAcquistoTotaleFatturato.Text = $"Totale fatturato acquisto: <strong>{totaleFatturatoAcquisto.ToString("N2")}</strong>";

            //Differenze
            lblDifferenzaTotaleImponibile.Text = $"Differenza totale imponibile: <strong>{(totaleImponibileEmesso - totaleImponibileAcquisto).ToString("N2")}</strong>";
            lblDifferenzaTotaleFatturato.Text = $"Differenza totale fatturato: <strong>{(totaleFatturatoEmesso - totaleFatturatoAcquisto).ToString("N2")}</strong>";

            // Bollette e Utile
            lblTotaleBollette.Text = $"Totale bollette: <strong>{totaleBollette.ToString("N2")}</strong>";
            lblUtile.Text = $"Utile: <strong>{(totaleImponibileEmesso - totaleImponibileAcquisto - Convert.ToDouble(totaleBollette / 2)).ToString("N2")}</strong>";
            hfUtile.Value = (totaleImponibileEmesso - totaleImponibileAcquisto - Convert.ToDouble(totaleBollette / 2)).ToString("N2");

            BindGrid(anno);
        }

        private void BindGrid(int anno)
        {
            List<(string, double, double, double)> items = new List<(string, double, double, double)>();
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
            decimal utile = Convert.ToDecimal(hfUtile.Value);
            lblUtileNettoTasse.Text = $"Utile netto tasse: <strong>{(utile * Convert.ToDecimal(txtTassePerc.Text) / 100)}</strong>";
        }

        protected void txtFiltroAnno_TextChanged(object sender, EventArgs e)
        {
            SetLabels();
        }
    }
}