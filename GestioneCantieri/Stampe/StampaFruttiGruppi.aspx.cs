using GestioneCantieri.DAO;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class StampaFruttiGruppi : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDdlScegliGruppo();
                BindGrid();
                GroupGridViewCells();
            }
        }

        /* EVENTI TEXT-CHANGED */
        protected void ddlScegliGruppo_TextChanged(object sender, EventArgs e)
        {
            BindGrid();
            GroupGridViewCells();
        }

        /* HELPERS */
        protected void BindGrid()
        {
            grdFruttiInGruppo.DataSource = CompGruppoFrutDAO.GetFruttiInGruppi(ddlScegliGruppo.SelectedItem?.Value ?? "");
            grdFruttiInGruppo.DataBind();
        }

        protected void FillDdlScegliGruppo()
        {
            ddlScegliGruppo.Items.Clear();
            ddlScegliGruppo.Items.Add(new ListItem("", "-1"));
            GruppiFruttiDAO.GetGruppi("", "", "").ForEach(f => ddlScegliGruppo.Items.Add(new ListItem(f.NomeGruppo, f.Id.ToString())));
        }

        protected void GroupGridViewCells()
        {
            GridViewHelper helper = new GridViewHelper(grdFruttiInGruppo);
            helper.RegisterGroup("NomeGruppo", true, true);
            helper.ApplyGroupSort();
        }

        /* Necessario per la creazione della GridView con intestazioni dinamiche */
        /* Definisce l'ordinamento dei dati presenti nella GridView */
        protected void grdFruttiInGruppo_Sorting(object sender, GridViewSortEventArgs e)
        {
            BindGrid();
        }
    }
}