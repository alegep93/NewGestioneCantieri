﻿using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace GestioneCantieri
{
    public partial class Listino : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SvuotaCampi();
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

        protected void btn_ImportaListinoDaDBF_Click(object sender, EventArgs e)
        {
            string pathFile = @"C:\MEF\ORDINI\";

            // Genero una lista a partire dai dati contenuti nel nuovo file DBF
            List<Mamg0ForDBF> mamgoList = Mamg0DAO.GetListinoFromDBF(pathFile);

            if(mamgoList != null && mamgoList.Count > 0)
            {
                Mamg0DAO.EliminaListino();
            }

            foreach (Mamg0ForDBF mmg in mamgoList)
            {
                // Inserisco i nuovi DDT
                Mamg0DAO.InserisciListino(mmg);
            }

            BindGrid();
        }
    }
}