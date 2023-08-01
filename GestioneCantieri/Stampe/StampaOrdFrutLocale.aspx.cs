using Database.DAO;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Linq;
using GestioneCantieri.Utils;

namespace GestioneCantieri
{
    public partial class StampaOrdFrutLocale : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlScegliCantiere.SelectedIndex = 0;
                FillDdlScegliCantiere();
            }
        }

        #region Eventi Text-Changed
        protected void ddlScegliCantiere_TextChanged(object sender, EventArgs e)
        {
            BindGrid();
            GroupGridViewCells();
        }
        #endregion

        #region Helpers
        protected void BindGrid()
        {
            string idCant = ddlScegliCantiere.SelectedItem.Value;
            List<int> indiciFruttiDaInserire = new List<int>();

            List<StampaOrdFrutCantLoc> listGruppi = OrdineFruttiDAO.GetAllGruppiInLocale(idCant);
            List<StampaOrdFrutCantLoc> listFrutti = OrdineFruttiDAO.GetAllFruttiInLocale(idCant);
            List<StampaOrdFrutCantLoc> listFruttiNonInGruppo = OrdineFruttiDAO.GetAllFruttiNonInGruppo(idCant);

            // Per ogni frutto non appartenente ad un gruppo
            foreach (StampaOrdFrutCantLoc fruttoNonInGruppo in listFruttiNonInGruppo)
            {
                StampaOrdFrutCantLoc frutto = listFrutti.Where(w => w.Nome == fruttoNonInGruppo.Nome).FirstOrDefault();

                // Verifico che esista nella lista dei Frutti
                if (frutto != null)
                {
                    // Elimino il frutto dalla lista
                    listFrutti.Remove(frutto);

                    // Sommo la quantità del frutto iniziale con quella del frutto non ancora in gruppo
                    frutto.Qta += fruttoNonInGruppo.Qta;

                    // Re-inserisco il frutto in lista
                    listFrutti.Add(frutto);
                }
                else
                {
                    // Altrimenti aggiungo l'elemento alla lista
                    listFrutti.Add(fruttoNonInGruppo);
                }
            }

            // Popolo tutte le griglie
            grdGruppiInLocale.DataSource = listGruppi;
            grdGruppiInLocale.DataBind();

            grdFruttiInLocale.DataSource = listFrutti;
            grdFruttiInLocale.DataBind();
        }

        protected void FillDdlScegliCantiere()
        {
            ddlScegliCantiere.Items.Clear();
            ddlScegliCantiere.Items.Add(new ListItem("", "-1"));
            DropDownListManager.FillDdlCantieri(CantieriDAO.GetAll().Where(w => !w.Chiuso).ToList(),ref ddlScegliCantiere);
        }

        protected void GroupGridViewCells()
        {
            GridViewHelper helper = new GridViewHelper(grdGruppiInLocale);
            helper.RegisterGroup("NomeLocale", true, true);
            helper.ApplyGroupSort();
        }
        #endregion

        /* Override per il corretto funzionamento di tutte le funzionalità della pagina */
        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
            //Do nothing
        }

        /* Necessario per la creazione della GridView con intestazioni dinamiche */
        /* Definisce l'ordinamento dei dati presenti nella GridView */
        protected void grdGruppiInLocale_Sorting(object sender, GridViewSortEventArgs e)
        {
            BindGrid();
        }
    }
}