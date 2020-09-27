using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class Default : System.Web.UI.Page
    {
        readonly string filePath = ConfigurationManager.AppSettings["DdtMef"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        #region Eventi Click
        protected void btnSvuotaTxt_Click(object sender, EventArgs e)
        {
            txtAnnoInizio.Text = "";
            txtAnnoFine.Text = "";
            txtDataInizio.Text = "";
            txtDataFine.Text = "";
            txtQta.Text = "";
            txtN_DDT.Text = "";
            txtCodArt1.Text = "";
            txtCodArt2.Text = "";
            txtCodArt3.Text = "";
            txtDescriCodArt1.Text = "";
            txtDescriCodArt2.Text = "";
            txtDescriCodArt3.Text = "";
            BindGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtAnnoInizio.Text != "" || txtAnnoFine.Text != "")
            {
                txtDataInizio.Text = "";
                txtDataFine.Text = "";
            }
            else if (txtDataInizio.Text != "" || txtDataFine.Text != "")
            {
                txtAnnoInizio.Text = "";
                txtAnnoFine.Text = "";
            }

            BindGrid(new DDTMefObject
            {
                AnnoInizio = txtAnnoInizio.Text,
                AnnoFine = txtAnnoFine.Text,
                DataInizio = txtDataInizio.Text,
                DataFine = txtDataFine.Text,
                Qta = txtQta.Text,
                NDdt = txtN_DDT.Text,
                CodArt1 = txtCodArt1.Text,
                CodArt2 = txtCodArt2.Text,
                CodArt3 = txtCodArt3.Text,
                DescriCodArt1 = txtDescriCodArt1.Text,
                DescriCodArt2 = txtDescriCodArt2.Text,
                DescriCodArt3 = txtDescriCodArt3.Text
            });
        }

        protected void btn_GeneraDdtDaDbf_Click(object sender, EventArgs e)
        {
            //Recupero l'id del fornitore "Mef"
            int idFornitore = FornitoriDAO.GetFornitori("Mef").FirstOrDefault().IdFornitori;

            // Genero una lista a partire dai dati contenuti nel nuovo file DBF
            List<DDTMef> ddtList = DDTMefDAO.GetDdtFromDBF(filePath, txtAcquirente.Text, idFornitore);

            // Popolo la tabella temporanea
            // Svuoto la tabella DDTMefTemp
            DDTMefDAO.DeleteFromDdtTemp();

            // Popolo la tabella temporanea con i nuovi dati
            DDTMefDAO.InsertIntoDdtTemp(ddtList);

            //Prendo la lista dei DDT non presenti sulla tabella TblDDTMef e li inserisco in TblDdtMef
            DDTMefDAO.InsertNewDdt();

            //Aggiorno i prezzi del mese corrente
            DDTMefDAO.UpdateDdt();

            // Aggiorno la griglia
            BindGrid();
        }
        #endregion

        #region Helpers
        protected void BindGrid(DDTMefObject ddt = null)
        {
            List<DDTMef> items = ddt == null ? DDTMefDAO.GetAll() : DDTMefDAO.GetDdt(ddt);
            grdListaDDTMef.DataSource = items;
            grdListaDDTMef.DataBind();

            // Imposto i campi di riepilogo
            txtMedia.Text = (items.Sum(s => s.PrezzoUnitario) / (items.Count() == 0 ? 1 : items.Count())).ToString("0.00") + " €";
            txtTotDDT.Text = items.Sum(s => s.Importo).ToString("N2") + " €";
            txtImponibileDDT.Text = items.Sum(s => (s.Importo * 100) / (100 + s.Iva)).ToString("N2") + " €";
            txtIvaDDT.Text = items.Sum(s => s.Importo - (100 * s.Importo / (100 + s.Iva))).ToString("N2") + " €";
        }
        #endregion
    }
}