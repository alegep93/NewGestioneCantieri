using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using GestioneCantieri.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class Preventivi : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ResetToInitial();
            }
        }

        /* HELPERS */
        protected void BindGrid()
        {
            grdPreventivi.DataSource = PreventiviDAO.GetPreventivi(txtFiltroAnno.Text, txtFiltroNumero.Text, txtFiltroDescr.Text);
            grdPreventivi.DataBind();
        }

        protected void FillDdlScegliOperaio()
        {
            ddlScegliOperaio.Items.Clear();
            ddlScegliOperaio.Items.Add(new ListItem("", "-1"));
            OperaioManager.FillDdlOperaio(OperaiDAO.GetAll(), ref ddlScegliOperaio);
        }

        private void ResetToInitial()
        {
            txtFiltroAnno.Text = txtFiltroNumero.Text = txtFiltroDescr.Text = "";
            txtNumeroPreventivo.Text = txtData.Text = txtDescrizione.Text = txtConcatenazione.Text = lblMessaggio.Text = "";
            txtAnno.Text = DateTime.Now.Year.ToString();
            ddlScegliOperaio.SelectedIndex = 0;
            btnInsPreventivo.Visible = true;
            btnModPreventivo.Visible = false;
            txtData.ReadOnly = txtDescrizione.ReadOnly = false;
            txtConcatenazione.ReadOnly = true;
            ddlScegliOperaio.Enabled = true;

            SetNumPrev();
            FillDdlScegliOperaio();
            BindGrid();
        }

        private void SetNumPrev()
        {
            int anno = txtAnno.Text != "" ? Convert.ToInt32(txtAnno.Text) : DateTime.Now.Year;
            List<Preventivo> items = PreventiviDAO.GetAll().Where(w => w.Anno == anno).ToList();

            long numeroPrev = items.Select(s => s.Numero).Max() + 1;
            string numeroPreventivo = "";
            if (items.Count() == 0)
            {
                numeroPreventivo = "001";
            }
            else
            {
                numeroPreventivo = numeroPrev.ToString().Substring(0, 2) == txtAnno.Text.Substring(2, 2) ? numeroPrev.ToString().Substring(2, 3) : numeroPrev.ToString();
            }

            txtNumeroPreventivo.Text = numeroPreventivo;
        }

        private void PopolaCampi(int idPreventivo)
        {
            Preventivo p = PreventiviDAO.GetSingle(idPreventivo);
            txtNumeroPreventivo.Text = p.Numero.ToString();
            ddlScegliOperaio.SelectedValue = p.IdOperaio.ToString();
            txtData.Text = p.Data.ToString("yyyy-MM-dd");
            txtData.TextMode = TextBoxMode.Date;
            txtDescrizione.Text = p.Descrizione;
            txtConcatenazione.Text = p.Numero + p.NomeOp.Substring(0, 2) + "-" + p.Descrizione;
        }

        protected void btnInsPreventivo_Click(object sender, EventArgs e)
        {
            try
            {
                bool isInserito = PreventiviDAO.Insert(new Preventivo
                {
                    Anno = Convert.ToInt32(txtAnno.Text),
                    Data = Convert.ToDateTime(txtData.Text),
                    Descrizione = txtDescrizione.Text,
                    IdOperaio = Convert.ToInt32(ddlScegliOperaio.SelectedValue),
                    Numero = Convert.ToInt64(txtAnno.Text.Substring(2, 2) + txtNumeroPreventivo.Text),
                });

                if (isInserito)
                {
                    lblMessaggio.ForeColor = Color.Blue;
                    lblMessaggio.Text = "Preventivo inserito con successo";
                }
                else
                {
                    lblMessaggio.ForeColor = Color.Red;
                    lblMessaggio.Text = "Errore durante l'inserimento del preventivo";
                }
            }
            catch (Exception ex)
            {
                lblMessaggio.ForeColor = Color.Red;
                lblMessaggio.Text = "Scatenata eccezione durante l'inserimento del preventivo ===> " + ex.Message;
            }

            ResetToInitial();
        }

        protected void btnModPreventivo_Click(object sender, EventArgs e)
        {
            try
            {
                bool isInserito = PreventiviDAO.Update(new Preventivo
                {
                    Id = Convert.ToInt32(hidIdPrev.Value),
                    Anno = Convert.ToInt32(txtAnno.Text),
                    Data = Convert.ToDateTime(txtData.Text),
                    Descrizione = txtDescrizione.Text,
                    IdOperaio = Convert.ToInt32(ddlScegliOperaio.SelectedValue),
                });

                if (isInserito)
                {
                    lblMessaggio.ForeColor = Color.Blue;
                    lblMessaggio.Text = "Preventivo aggiornato con successo";
                }
                else
                {
                    lblMessaggio.ForeColor = Color.Red;
                    lblMessaggio.Text = "Errore durante l'aggiornamento del preventivo";
                }
            }
            catch (Exception ex)
            {
                lblMessaggio.ForeColor = Color.Red;
                lblMessaggio.Text = "Scatenata eccezione durante l'aggiornamento del preventivo ===> " + ex.Message;
            }

            ResetToInitial();
            BindGrid();
        }

        protected void btnFiltraPreventivi_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnSvuotaFiltri_Click(object sender, EventArgs e)
        {
            ResetToInitial();
        }

        protected void grdPreventivi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idPreventivo = Convert.ToInt32(e.CommandArgument.ToString());
            hidIdPrev.Value = idPreventivo.ToString();

            if (e.CommandName == "VisualPrev")
            {
                PopolaCampi(idPreventivo);

                // Nascondo entrambi i bottoni e rendo le caselle di testo ReadOnly
                btnInsPreventivo.Visible = btnModPreventivo.Visible = false;
                txtNumeroPreventivo.ReadOnly = ddlScegliOperaio.Enabled = txtData.ReadOnly = txtDescrizione.ReadOnly = txtConcatenazione.ReadOnly = true;
            }
            else if (e.CommandName == "ModPrev")
            {
                ResetToInitial();
                PopolaCampi(idPreventivo);
                txtConcatenazione.ReadOnly = true;
                btnInsPreventivo.Visible = false;
                btnModPreventivo.Visible = true;
            }
            else if (e.CommandName == "ElimPrev")
            {
                bool isDeleted = PreventiviDAO.Delete(idPreventivo);

                if (isDeleted)
                {
                    lblMessaggio.ForeColor = Color.Blue;
                    lblMessaggio.Text = "Preventivo eliminato con successo";
                }
                else
                {
                    lblMessaggio.ForeColor = Color.Red;
                    lblMessaggio.Text = "Errore durante l'eliminazione del preventivo";
                }

                ResetToInitial();
            }
        }

        protected void txtAnno_TextChanged(object sender, EventArgs e)
        {
            SetNumPrev();
        }
    }
}