using Database.DAO;
using Database.Models;
using GestioneCantieri.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class GestionePagamenti : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDdlScegliCant();
                pnlGestPagam.Visible = btnModPagam.Visible = false;
            }
        }

        #region Helpers
        protected void FillDdlScegliCant()
        {
            ddlScegliCant.Items.Clear();
            ddlScegliCant.Items.Add(new ListItem("", "-1"));
            DropDownListManager.FillDdlCantieri(CantieriDAO.GetCantieri(txtFiltroCantAnno.Text, txtFiltroCantCodCant.Text, txtFiltroCantDescrCodCant.Text, chkFiltroCantChiuso.Checked, chkFiltroCantRiscosso.Checked), ref ddlScegliCant);
        }
        protected void SvuotaCampi(Panel pnl)
        {
            //Svuoto tutti i TextBox
            foreach (Control c in pnl.Controls)
            {
                if (c is TextBox box)
                {
                    box.Text = "";
                }
            }

            //Acconto e Saldo FALSE
            chkSaldo.Checked = chkAcconto.Checked = false;
            txtImportoPagam.Text = "0";
            BindGrid();
        }
        protected void EnableDisableControls(bool enableControls, Panel panelName)
        {
            foreach (Control c in panelName.Controls)
            {
                if (c is TextBox box)
                {
                    box.Enabled = enableControls;
                }
                else if (c is DropDownList ddl)
                {
                    ddl.Enabled = enableControls;
                }
            }
            foreach (Control c in pnlFiltriSceltaCant.Controls)
            {
                if (c is TextBox box)
                {
                    box.Enabled = enableControls;
                }
                else if (c is DropDownList ddl)
                {
                    ddl.Enabled = enableControls;
                }
            }
        }
        protected void BindGrid()
        {
            grdPagamenti.DataSource = PagamentiDAO.GetAll(txtFiltroPagamDescri.Text).Where(w => w.IdTblCantieri == Convert.ToInt32(ddlScegliCant.SelectedItem.Value)).ToList();
            grdPagamenti.DataBind();
        }
        private void PopolaCampiPagam(int idPagam, bool enableControls)
        {
            //Rendo i textbox abilitati/disabilitati in base al parametro enableControls
            EnableDisableControls(enableControls, pnlGestPagam);

            Pagamenti p = PagamentiDAO.GetSingle(idPagam);
            ddlScegliCant.SelectedItem.Value = p.IdTblCantieri.ToString();
            txtDataDDT.Text = p.Data.ToString("yyyy-MM-dd");
            txtDataDDT.TextMode = TextBoxMode.Date;
            txtImportoPagam.Text = p.Imporo.ToString();
            txtDescrPagam.Text = p.DescriPagamenti.ToString();
            chkSaldo.Checked = p.Saldo;
            chkAcconto.Checked = p.Acconto;
        }
        #endregion

        #region Eventi click
        protected void btnFiltroCant_Click(object sender, EventArgs e)
        {
            FillDdlScegliCant();
        }
        protected void btnSvuotaIntestazione_Click(object sender, EventArgs e)
        {
            //Svuoto tutti i TextBox
            foreach (Control c in pnlFiltriSceltaCant.Controls)
            {
                if (c is TextBox box)
                {
                    box.Text = "";
                }

                if (c is DropDownList ddl)
                {
                    ddl.SelectedIndex = 0;
                }
            }

            txtDataDDT.Text = "";
            txtDataDDT.TextMode = TextBoxMode.Date;
        }
        protected void ddlScegliCant_TextChanged(object sender, EventArgs e)
        {
            if (ddlScegliCant.SelectedIndex != 0)
            {
                pnlGestPagam.Visible = true;
                btnModPagam.Visible = false;
                BindGrid();
            }
            else
            {
                pnlGestPagam.Visible = false;
            }
        }
        protected void btnInsPagam_Click(object sender, EventArgs e)
        {
            bool isInserito = false;

            if (txtDataDDT.Text != "")
            {

                if (ddlScegliCant.SelectedIndex != 0 && txtDataDDT.Text != "")
                {
                    isInserito = PagamentiDAO.InserisciPagamento(new Pagamenti
                    {
                        IdTblCantieri = Convert.ToInt32(ddlScegliCant.SelectedItem.Value),
                        Data = Convert.ToDateTime(txtDataDDT.Text),
                        DescriPagamenti = txtDescrPagam.Text,
                        Acconto = chkAcconto.Checked,
                        Saldo = chkSaldo.Checked,
                        Imporo = txtImportoPagam.Text != "" ? Convert.ToDecimal(txtImportoPagam.Text) : 0
                    });
                }

                if (isInserito)
                {
                    lblIsPagamInserito.Text = "Record inserito con successo";
                    lblIsPagamInserito.ForeColor = Color.Blue;
                }
                else
                {
                    lblIsPagamInserito.Text = "Errore durante l'inserimento del record. L'intestazione deve essere interamente compilata.";
                    lblIsPagamInserito.ForeColor = Color.Red;
                }
                SvuotaCampi(pnlGestPagam);
            }
            else
            {
                lblIsPagamInserito.Text = "Inserire un valore per la data";
                lblIsPagamInserito.ForeColor = Color.Red;
            }
        }
        protected void btnGestPagam_Click(object sender, EventArgs e)
        {
            btnModPagam.Visible = false;
            btnInsPagam.Visible = true;
            EnableDisableControls(true, pnlGestPagam);
            SvuotaCampi(pnlGestPagam);
        }
        protected void btnFiltraPagam_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
        protected void btnModPagam_Click(object sender, EventArgs e)
        {
            bool isUpdated = PagamentiDAO.UpdatePagamento(new Pagamenti
            {
                IdPagamenti = Convert.ToInt32(hidPagamenti.Value),
                IdTblCantieri = Convert.ToInt32(ddlScegliCant.SelectedItem.Value),
                Data = Convert.ToDateTime(txtDataDDT.Text),
                DescriPagamenti = txtDescrPagam.Text,
                Acconto = chkAcconto.Checked,
                Saldo = chkSaldo.Checked,
                Imporo = txtImportoPagam.Text != "" ? Convert.ToDecimal(txtImportoPagam.Text) : 0
            });

            if (isUpdated)
            {
                lblIsPagamInserito.Text = "Record modificato con successo";
                lblIsPagamInserito.ForeColor = Color.Blue;
            }
            else
            {
                lblIsPagamInserito.Text = "Errore durante la modifica del record";
                lblIsPagamInserito.ForeColor = Color.Red;
            }
            SvuotaCampi(pnlGestPagam);
        }
        #endregion

        #region Rowcommand
        protected void grdPagamenti_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idPagam = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "VisualPagam")
            {
                PopolaCampiPagam(idPagam, false);
                btnInsPagam.Visible = btnModPagam.Visible = false;
            }
            else if (e.CommandName == "ModPagam")
            {
                btnInsPagam.Visible = false;
                btnModPagam.Visible = !btnInsPagam.Visible;
                hidPagamenti.Value = idPagam.ToString();
                PopolaCampiPagam(idPagam, true);
            }
            else if (e.CommandName == "ElimPagam")
            {
                bool isDeleted = PagamentiDAO.DeletePagamento(idPagam);

                if (isDeleted)
                {
                    lblIsPagamInserito.Text = "Record eliminato con successo";
                    lblIsPagamInserito.ForeColor = Color.Blue;
                }
                else
                {
                    lblIsPagamInserito.Text = "Errore durante l'eliminazione del record";
                    lblIsPagamInserito.ForeColor = Color.Red;
                }
                SvuotaCampi(pnlGestPagam);
            }
        }
        #endregion
    }
}