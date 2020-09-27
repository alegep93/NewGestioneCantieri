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
            GetBasicValuesForRecap();
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
            BindGridWithSearch();
        }

        protected void btn_GeneraDdtDaDbf_Click(object sender, EventArgs e)
        {
            //Recupero l'id del fornitore "Mef"
            int idFornitore = FornitoriDAO.GetFornitori("Mef").FirstOrDefault().IdFornitori;

            // Genero una lista a partire dai dati contenuti nel nuovo file DBF
            List<DDTMef> ddtList = DDTMefDAO.GetDdtFromDBF(filePath, txtAcquirente.Text, idFornitore);

            // Popolo la tabella temporanea
            InsertIntoDdtTemp(ddtList);

            //Prendo la lista dei DDT non presenti sulla tabella TblDDTMef
            List<DDTMef> ddtMancanti = DDTMefDAO.GetNewDDT();

            foreach (DDTMef ddt in ddtMancanti)
            {
                // Inserisco i nuovi DDT
                DDTMefDAO.InsertNewDdt(ddt);
            }

            //Aggiorno i prezzi del mese corrente
            UpdatePrezzi();

            // Aggiorno la griglia
            BindGrid();
        }
        #endregion

        #region Helpers
        protected void BindGrid()
        {
            grdListaDDTMef.DataSource = DDTMefDAO.GetAll();
            grdListaDDTMef.DataBind();
            GetBasicValuesForRecap();
        }

        private void GetBasicValuesForRecap()
        {
            List<DDTMef> listaDdt = DDTMefDAO.GetAll();
            SetFields(listaDdt);
        }

        protected void BindGridWithSearch()
        {
            DDTMefObject ddt = FillDdtObject();

            // Rigenero la griglia
            List<DDTMef> listaDdt = DDTMefDAO.GetDdt(ddt);
            grdListaDDTMef.DataSource = listaDdt;
            grdListaDDTMef.DataBind();

            // Popolo i campi con i dati filtrati
            SetFields(listaDdt);
        }

        private void SetFields(List<DDTMef> items)
        {
            txtMedia.Text = (items.Sum(s => s.PrezzoUnitario) / (items.Count() == 0 ? 1 : items.Count())).ToString("0.00") + " €";
            txtTotDDT.Text = items.Sum(s => s.Importo).ToString("N2") + " €";
            txtImponibileDDT.Text = items.Sum(s => (s.Importo * 100) / (100 + s.Iva)).ToString("N2") + " €";
            txtIvaDDT.Text = items.Sum(s => s.Importo - (100 * s.Importo / (100 + s.Iva))).ToString("N2") + " €";
        }

        protected DDTMefObject FillDdtObject()
        {
            return new DDTMefObject
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
            };
        }

        protected void FillDdlClienti()
        {
            List<Fornitori> listClienti = FornitoriDAO.GetFornitori();

            ddlFornitore.Items.Clear();
            ddlFornitore.Items.Add(new ListItem("", "-1"));

            foreach (Fornitori f in listClienti)
                ddlFornitore.Items.Add(new ListItem(f.RagSocForni, f.IdFornitori.ToString()));
        }

        protected void InsertIntoDdtTemp(List<DDTMef> ddtList)
        {
            // Svuoto la tabella DDTMefTemp
            DDTMefDAO.DeleteFromDdtTemp();

            // Per ogni elemento della lista
            foreach (DDTMef ddt in ddtList)
            {
                // Popolo la tabella temporanea con i nuovi dati
                DDTMefDAO.InsertIntoDdtTemp(ddt);
            }
        }

        private void UpdatePrezzi()
        {
            DDTMefDAO.UpdateDdt();
        }
        #endregion
    }
}