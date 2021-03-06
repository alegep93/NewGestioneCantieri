﻿using Database.DAO;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class ControlloGruppi : Page
    {
        public List<CompGruppoFrut> componentiGruppo = new List<CompGruppoFrut>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
                MostraNumeroGruppiNonControllati();
            }

            Page.MaintainScrollPositionOnPostBack = true;
        }

        private void MostraNumeroGruppiNonControllati()
        {
            lblNumGruppiNonControllati.Text = "Numero di gruppi da controllare: " + GruppiFruttiDAO.GetAll().Where(w => !w.Controllato).Count().ToString();
        }

        private void BindGrid()
        {
            List<GruppiFrutti> gruppiNonControllatiList = GruppiFruttiDAO.GetAll().Where(w => !w.Controllato).ToList();
            grdFruttiNonControllati.DataSource = gruppiNonControllatiList;
            grdFruttiNonControllati.DataBind();
        }

        protected void MostraComponentiGruppo(int idGruppo)
        {
            lblPanelTitleGroupName.Text = GruppiFruttiDAO.GetSingle(idGruppo).NomeGruppo;
            componentiGruppo = CompGruppoFrutDAO.GetCompGruppo(idGruppo);
        }

        protected void grdFruttiNonControllati_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument.ToString());

            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            int rowIndex = row.RowIndex;

            if (e.CommandName == "MostraCompGruppo")
            {
                MostraComponentiGruppo(id);
                SelezionaRigaCorrente(rowIndex);
            }
            else if (e.CommandName == "ControllaGruppo")
            {
                GruppiFruttiDAO.UpdateFlagControllato(id);
                BindGrid();
                MostraNumeroGruppiNonControllati();
            }
        }

        private void SelezionaRigaCorrente(int rowIndex)
        {
            //for (int i = 0; i < grdFruttiNonControllati.Rows.Count; i++)
            //{
            //    grdFruttiNonControllati.Rows[i].BackColor = Color.Transparent;
            //}
            //for (int i = 1; i < grdFruttiNonControllati.Rows.Count; i = i + 2)
            //{
            //    grdFruttiNonControllati.Rows[i].BackColor = Color.FromArgb(249, 249, 249);
            //}

            for (int i = 0; i < grdFruttiNonControllati.Rows.Count; i++)
            {
                grdFruttiNonControllati.Rows[i].CssClass = "";
            }
            grdFruttiNonControllati.Rows[rowIndex].CssClass = "yellow-background";
        }

        protected void grdFruttiNonControllati_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[2].Text = "Completato";
                e.Row.Cells[3].Text = "Controllato";
            }
        }
    }
}