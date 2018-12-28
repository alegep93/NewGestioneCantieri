﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GestioneCantieri.DAO;
using GestioneCantieri.Data;

namespace GestioneCantieri
{
    public partial class Preventivi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ResetToInitial();
                BindGrid();
            }
        }

        /* HELPERS */
        protected void BindGrid()
        {
            List<Preventivo> prevList = PreventiviDAO.GetPreventivi(txtFiltroAnno.Text, txtFiltroNumero.Text, txtFiltroDescr.Text);
            grdPreventivi.DataSource = prevList;
            grdPreventivi.DataBind();
        }

        protected void FillDdlScegliOperaio()
        {
            int i = 0;
            DataTable dt = OperaiDAO.GetOperai();
            List<Operai> listOperai = dt.DataTableToList<Operai>();

            ddlScegliOperaio.Items.Clear();
            ddlScegliOperaio.Items.Add(new ListItem("", "-1"));

            foreach (Operai op in listOperai)
            {
                string show = op.NomeOp + " - " + op.DescrOp;
                ddlScegliOperaio.Items.Add(new ListItem(show, op.IdOperaio.ToString()));

                i++;
                if (op.NomeOp == "Maurizio" || op.NomeOp == "Mau" || op.NomeOp == "MAU")
                {
                    ddlScegliOperaio.SelectedIndex = i;
                }
            }
        }

        private void ResetToInitial()
        {
            txtFiltroAnno.Text = txtFiltroNumero.Text = txtFiltroDescr.Text = "";
            txtNumeroPreventivo.Text = txtData.Text = txtDescrizione.Text = txtConcatenazione.Text = lblMessaggio.Text = "";
            ddlScegliOperaio.SelectedIndex = 0;
            btnInsPreventivo.Visible = true;
            btnModPreventivo.Visible = false;
            txtData.ReadOnly = txtDescrizione.ReadOnly = false;
            txtConcatenazione.ReadOnly = true;
            ddlScegliOperaio.Enabled = true;

            long numeroPrev = PreventiviDAO.GetLastNumber();
            txtNumeroPreventivo.Text = numeroPrev.ToString().Substring(0, 2) == DateTime.Today.Year.ToString().Substring(2, 2) ? numeroPrev.ToString().Substring(2, 3) : numeroPrev.ToString();

            FillDdlScegliOperaio();
        }

        private Preventivo PopolaPreventivoObj(bool isUpdate)
        {
            Preventivo p = new Preventivo();
            p.Anno = DateTime.Now.Year;
            p.Data = Convert.ToDateTime(txtData.Text);
            p.Descrizione = txtDescrizione.Text;
            p.IdOperaio = Convert.ToInt32(ddlScegliOperaio.SelectedValue);

            if (!isUpdate)
            {
                p.Numero = Convert.ToInt64(p.Anno.ToString().Substring(2, 2) + txtNumeroPreventivo.Text);
            }

            return p;
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

        private void VisualizzaDati(int idPreventivo)
        {
            PopolaCampi(idPreventivo);

            // Nascondo entrambi i bottoni e rendo le caselle di testo ReadOnly
            btnInsPreventivo.Visible = btnModPreventivo.Visible = false;
            txtNumeroPreventivo.ReadOnly = ddlScegliOperaio.Enabled = txtData.ReadOnly = txtDescrizione.ReadOnly = txtConcatenazione.ReadOnly = true;
        }

        private void ModificaDati(int idPreventivo)
        {
            ResetToInitial();
            PopolaCampi(idPreventivo);

            txtConcatenazione.ReadOnly = true;
            btnInsPreventivo.Visible = false;
            btnModPreventivo.Visible = true;
        }

        private void Elimina(int idPreventivo)
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
            BindGrid();
        }

        protected void btnInsPreventivo_Click(object sender, EventArgs e)
        {
            Preventivo p = PopolaPreventivoObj(false);

            try
            {
                if (PreventiviDAO.Insert(p))
                {
                    lblMessaggio.ForeColor = Color.Blue;
                    lblMessaggio.Text = "Preventivo " + p.Numero + " inserito con successo";
                }
                else
                {
                    lblMessaggio.ForeColor = Color.Red;
                    lblMessaggio.Text = "Errore durante l'inserimento del preventivo " + p.Numero;
                }
            }
            catch (Exception ex)
            {
                lblMessaggio.ForeColor = Color.Red;
                lblMessaggio.Text = "Scatenata eccezione durante l'inserimento del preventivo " + p.Numero + " ===> " + ex.Message;
            }

            ResetToInitial();
            BindGrid();
        }

        protected void btnModPreventivo_Click(object sender, EventArgs e)
        {
            Preventivo p = PopolaPreventivoObj(true);
            p.Id = Convert.ToInt32(hidIdPrev.Value);

            try
            {
                if (PreventiviDAO.Update(p))
                {
                    lblMessaggio.ForeColor = Color.Blue;
                    lblMessaggio.Text = "Preventivo " + p.Numero + " aggiornato con successo";
                }
                else
                {
                    lblMessaggio.ForeColor = Color.Red;
                    lblMessaggio.Text = "Errore durante l'aggiornamento del preventivo " + p.Numero;
                }
            }
            catch (Exception ex)
            {
                lblMessaggio.ForeColor = Color.Red;
                lblMessaggio.Text = "Scatenata eccezione durante l'aggiornamento del preventivo " + p.Numero + " ===> " + ex.Message;
            }

            ResetToInitial();
            BindGrid();
        }

        protected void grdPreventivi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idPreventivo = Convert.ToInt32(e.CommandArgument.ToString());
            hidIdPrev.Value = idPreventivo.ToString();

            if (e.CommandName == "VisualPrev")
                VisualizzaDati(idPreventivo);
            else if (e.CommandName == "ModPrev")
                ModificaDati(idPreventivo);
            else if (e.CommandName == "ElimPrev")
                Elimina(idPreventivo);
        }

        protected void btnFiltraPreventivi_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnSvuotaFiltri_Click(object sender, EventArgs e)
        {
            ResetToInitial();
        }
    }
}