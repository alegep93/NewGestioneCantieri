using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web.UI;

namespace GestioneCantieri
{
    public partial class Listino : Page
    {
        readonly string filePath = ConfigurationManager.AppSettings["Mamg0"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SvuotaCampi();
                txtCodArt1.Focus();
            }
        }

        /* EVENTI CLICK */
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridWithSearch();
        }
        protected void btnSvuotaTxt_Click(object sender, EventArgs e)
        {
            SvuotaCampi();
        }
        protected void btnEliminaListino_Click(object sender, EventArgs e)
        {
            Mamg0DAO.EliminaListino();
            Response.Redirect("~/Listino.aspx");
        }
        protected void btn_ImportaListino_Click(object sender, EventArgs e)
        {
            try
            {
                // Elimino il listino
                Mamg0DAO.EliminaListino();

                // Reinserisco tutto il listino Mamg0, recuperando i dati da un file excel nominato Mamg0.xlsx
                Mamg0DAO.GetDataFromExcelAndInsertBulkCopy(filePath);

                // Aggiorno la tabella di visualizzazione risultati
                BindGrid();

                lblImportMsg.Text = "Importazione del listino avvenuta con successo";
                lblImportMsg.ForeColor = Color.Blue;
            }
            catch (Exception ex)
            {
                lblImportMsg.Text = "Errore durante l'importazione del listino. <br />" + ex;
                lblImportMsg.ForeColor = Color.Red;
            }
        }

        /* HELPERS */
        protected void BindGrid()
        {
            List<Mamg0> listaDDT = new List<Mamg0>();
            listaDDT = Mamg0DAO.getAll();
            grdListino.DataSource = listaDDT;
            grdListino.DataBind();
        }
        protected void BindGridWithSearch()
        {
            List<Mamg0> listaDDT = new List<Mamg0>();
            listaDDT = Mamg0DAO.GetListino(txtCodArt1.Text, txtCodArt2.Text, txtCodArt3.Text, txtDescriCodArt1.Text, txtDescriCodArt2.Text, txtDescriCodArt3.Text);
            grdListino.DataSource = listaDDT;
            grdListino.DataBind();
        }
        protected void SvuotaCampi()
        {
            txtCodArt1.Text = "";
            txtCodArt2.Text = "";
            txtCodArt3.Text = "";
            txtDescriCodArt1.Text = "";
            txtDescriCodArt2.Text = "";
            txtDescriCodArt3.Text = "";
            BindGrid();
        }
    }
}