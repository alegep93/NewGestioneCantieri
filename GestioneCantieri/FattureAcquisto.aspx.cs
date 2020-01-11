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
    public partial class FattureAcquisto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ResetToInitial();
                BindGrid();
            }
        }

        private void ResetToInitial()
        {
            txtFiltroGrdAnno.Text = txtFiltroGrdFornitore.Text = "";
            txtNumeroFattura.Text = txtData.Text;
            txtIva.Text = "22";
            ddlScegliFornitore.SelectedIndex = 0;
            btnInsFattura.Visible = true;
            btnModFattura.Visible = false;
            txtData.ReadOnly = false;
            SetNumeroFattura();
            FillDdlScegliFornitore();
        }

        protected void BindGrid()
        {
            int numeroFattura = txtFiltroGrdNumeroFattura.Text != "" ? Convert.ToInt32(txtFiltroGrdNumeroFattura.Text) : 0;
            List<FatturaAcquisto> fatture = FattureAcquistoDAO.GetFattureAcquisto(txtFiltroGrdAnno.Text, txtFiltroGrdDataDa.Text, txtFiltroGrdDataA.Text, txtFiltroGrdFornitore.Text, numeroFattura);
            grdFatture.DataSource = fatture;
            grdFatture.DataBind();

            int anno = txtFiltroGrdAnno.Text == "" ? DateTime.Now.Year : Convert.ToInt32(txtFiltroGrdAnno.Text);

            grdTotaleIvaPerQuarter.DataSource = FattureDAO.GetTotaliIvaPerQuarter(anno).Select(s => new
            {
                Trimestre = s.quarter,
                TotaleIva = s.totaleIva.ToString("N2")
            });
            grdTotaleIvaPerQuarter.DataBind();

            grdTotaleImponibilePerQuarter.DataSource = FattureDAO.GetTotaliImponibilePerQuarter(anno).Select(s => new
            {
                Trimestre = s.quarter,
                TotaleIva = s.totaleIva.ToString("N2")
            });
            grdTotaleImponibilePerQuarter.DataBind();

            grdTotaleImportoPerQuarter.DataSource = FattureDAO.GetTotaliImportoPerQuarter(anno).Select(s => new
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

        private void SetNumeroFattura()
        {
            long numeroFattura = FattureDAO.GetLastNumber(DateTime.Now.Year);
            if (numeroFattura == 0)
            {
                txtNumeroFattura.Text = "001";
            }
            else
            {
                txtNumeroFattura.Text = numeroFattura.ToString();
            }
        }

        protected void FillDdlScegliFornitore(string ragSocFornitore = "")
        {
            List<Fornitori> fornitori = FornitoriDAO.GetFornitori();

            ddlScegliFornitore.Items.Clear();
            foreach (Fornitori fornitore in fornitori)
            {
                ddlScegliFornitore.Items.Add(new ListItem(fornitore.RagSocForni, fornitore.IdFornitori.ToString()));
            }
        }
    }
}