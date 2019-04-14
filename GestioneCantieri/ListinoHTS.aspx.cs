﻿using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class ListinoHTS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtPercentualeSconto.Text = txtPercentualeSconto.Text != "" ? txtPercentualeSconto.Text : 50.ToString();
            txtPercentualeSconto.Text = !txtPercentualeSconto.Text.Contains("%") ? txtPercentualeSconto.Text + "%" : txtPercentualeSconto.Text;

            if (!IsPostBack)
            {
                SvuotaCampi();
                txtCodice1.Focus();
            }
        }

        /* EVENTI CLICK */
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
        protected void btnSvuotaTxt_Click(object sender, EventArgs e)
        {
            SvuotaCampi();
        }
        protected void btnEliminaListino_Click(object sender, EventArgs e)
        {
            ListinoHtsDAO.Delete();
            Response.Redirect("~/Listino.aspx");
        }

        /* HELPERS */
        protected void BindGrid()
        {
            List<ListinoHts> listaDDT = new List<ListinoHts>();
            listaDDT = ListinoHtsDAO.GetAllFiltered(txtCodice1.Text, txtCodice2.Text, txtCodice3.Text, txtCodProd1.Text, txtCodProd2.Text, txtCodProd3.Text, txtDescriCodProd1.Text, txtDescriCodProd2.Text, txtDescriCodProd3.Text);
            grdListinoHts.DataSource = listaDDT;
            grdListinoHts.DataBind();
        }
        protected void SvuotaCampi()
        {
            txtCodice1.Text = txtCodice2.Text = txtCodice3.Text = "";
            txtCodProd1.Text = txtCodProd2.Text = txtCodProd3.Text = "";
            txtDescriCodProd1.Text = txtDescriCodProd2.Text = txtDescriCodProd3.Text = "";
            BindGrid();
        }

        protected void grdListinoHts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[5].Text = "Prezzo Scontato";
            }
            else
            {
                // Calcolo il prezzo scontato
                decimal sconto = Convert.ToDecimal(txtPercentualeSconto.Text.Replace("%",""));
                decimal prezzo = e.Row.Cells[4].Text != "&nbsp;" ? Convert.ToDecimal(e.Row.Cells[4].Text) : 0;
                e.Row.Cells[5].Text = (prezzo - prezzo * sconto / 100).ToString();
            }
        }

        protected void txtPercentualeSconto_TextChanged(object sender, EventArgs e)
        {
            BindGrid();
        }
    }
}