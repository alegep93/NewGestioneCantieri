﻿using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class ResocontoOperaio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDdlScegliAcquirente();
                txtDataDa.Text = DateTime.Now.Year + "-01-01";
                txtDataA.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtDataDa.TextMode = txtDataA.TextMode = TextBoxMode.Date;

                btnPagaOperaio.Visible = false;
            }
        }

        /* HELPERS */
        protected void FillDdlScegliAcquirente()
        {
            DataTable dt = OperaiDAO.GetOperai();
            List<Operai> listOperai = dt.DataTableToList<Operai>();

            ddlScegliOperaio.Items.Clear();
            ddlScegliOperaio.Items.Add(new ListItem("", "-1"));

            foreach (Operai op in listOperai)
            {
                string show = op.Operaio + " - " + op.NomeOp + " - " + op.DescrOp;
                ddlScegliOperaio.Items.Add(new ListItem(show, op.IdOperaio.ToString()));
            }
        }
        protected void BindGrid()
        {
            decimal totValore = 0m;
            decimal totOre = 0;
            List<MaterialiCantieri> matCantList;
            matCantList = MaterialiCantieriDAO.GetMatCantPerResocontoOperaio(txtDataDa.Text, txtDataA.Text, ddlScegliOperaio.SelectedItem.Value, txtFiltroCantiere.Text, Convert.ToInt32(rblChooseView.SelectedValue));
            grdResocontoOperaio.DataSource = matCantList;
            grdResocontoOperaio.DataBind();

            //Imposto la colonna del valore
            for (int i = 0; i < grdResocontoOperaio.Rows.Count; i++)
            {
                decimal valore = Convert.ToDecimal(grdResocontoOperaio.Rows[i].Cells[4].Text) * Convert.ToDecimal(grdResocontoOperaio.Rows[i].Cells[5].Text);
                grdResocontoOperaio.Rows[i].Cells[6].Text = Math.Round(valore, 2).ToString();
                totOre += Convert.ToDecimal(Convert.ToDecimal(grdResocontoOperaio.Rows[i].Cells[4].Text.Replace(".", ",")));
                totValore += Convert.ToDecimal(grdResocontoOperaio.Rows[i].Cells[6].Text);
            }

            lblTotali.Text = "Totale Ore: " + totOre.ToString() + " ||" + "Totale Valore: " + totValore.ToString();
        }

        protected void btnStampaResoconto_Click(object sender, EventArgs e)
        {
            BindGrid();
            btnPagaOperaio.Visible = true;
            CheckToEnablePagaOperaio();
        }
        protected void btnPagaOperaio_Click(object sender, EventArgs e)
        {
            bool isUpdated = MaterialiCantieriDAO.UpdateOperaioPagato(txtDataDa.Text, txtDataA.Text, ddlScegliOperaio.SelectedItem.Value);

            if (isUpdated)
            {
                lblIsOperaioPagato.Text = "Campo \"OperaioPagato\" aggiornato con successo";
                lblIsOperaioPagato.ForeColor = Color.Blue;
            }
            else
            {
                lblIsOperaioPagato.Text = "Impossibile aggiornare il campo \"OperaioPagato\"";
                lblIsOperaioPagato.ForeColor = Color.Red;
            }

            BindGrid();
        }

        protected void btnFiltra_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void ddlScegliOperaio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckToEnablePagaOperaio();
        }

        private void CheckToEnablePagaOperaio()
        {
            if (ddlScegliOperaio.SelectedIndex == 0)
            {
                btnPagaOperaio.Enabled = false;
            }
            else
            {
                btnPagaOperaio.Enabled = true;
            }
        }
    }
}