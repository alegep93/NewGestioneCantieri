using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class Default : System.Web.UI.Page
    {
        readonly string filePath = ConfigurationManager.AppSettings["DdtMef"];
        readonly string filePathTxt = ConfigurationManager.AppSettings["DdtMefTxt"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        /*** INIZIO EVENTI CLICK ***/
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
            int idFornitore = FornitoriDAO.GetIdFornitore("Mef");

            //spinnerImg.Visible = true;

            // Genero una lista a partire dai dati contenuti nel nuovo file DBF
            List<DDTMef> ddtList = DDTMefDAO.GetDdtFromDBF(filePath, txtAcquirente.Text, idFornitore);
            //List<DDTMef> ddtList = ReadDataFromTextFile(idFornitore);

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

            BindGrid();
        }

        private List<DDTMef> ReadDataFromTextFile(int idFornitore)
        {
            List<DDTMef> ddts = new List<DDTMef>();
            string[] lines = File.ReadAllLines(filePathTxt);
            foreach (string line in lines)
            {
                DDTMef ddt = new DDTMef();
                int qta = line.Substring(174, 8).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(174, 8).Trim());
                decimal importo = line.Substring(185, 12).Trim() == "" ? 0 : Convert.ToDecimal($"{Convert.ToInt32(line.Substring(185, 12).Trim())},{Convert.ToInt32(line.Substring(196, 3).Trim())}");
                ddt.Anno = line.Substring(0, 4).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(0, 4).Trim());
                ddt.Data = line.Substring(4, 8).Trim() == "" ? new DateTime() : Convert.ToDateTime(GetDateFromString(line.Substring(4, 8).Trim()));
                ddt.N_ddt = line.Substring(12, 6).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(12, 6).Trim());
                ddt.FTVRF0 = line.Substring(18, 15).Trim();
                ddt.FTDT30 = line.Substring(33, 41).Trim();
                ddt.CodArt = line.Substring(41, 19).Trim();
                ddt.FTAIN = line.Substring(60, 17).Trim();
                ddt.DescriCodArt = line.Substring(77, 40).Trim();
                ddt.DescrizioneArticolo2 = line.Substring(117, 40).Trim();
                ddt.Iva = line.Substring(157, 2).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(157, 2).Trim());
                ddt.PrezzoListino = line.Substring(159, 12).Trim() == "" ? 0 : Convert.ToDecimal($"{Convert.ToInt32(line.Substring(159, 12).Trim())},{Convert.ToInt32(line.Substring(170, 3).Trim())}");
                ddt.Qta = qta;
                ddt.Importo = importo;
                ddt.Data2 = line.Substring(200, 8).Trim() == "" ? new DateTime() : Convert.ToDateTime(GetDateFromString(line.Substring(200, 8).Trim()));
                ddt.Valuta = line.Substring(208, 3).Trim();
                ddt.FTFOM = line.Substring(211, 1).Trim();
                ddt.FTCMA = line.Substring(212, 2).Trim();
                ddt.FTCDO = line.Substring(214, 2).Trim();
                ddt.FLFLAG = line.Substring(216, 1).Trim();
                ddt.FLFLQU = line.Substring(217, 2).Trim();
                ddt.Data3 = line.Substring(219, 8).Trim() == "" ? new DateTime() : Convert.ToDateTime(GetDateFromString(line.Substring(219, 8).Trim()));
                ddt.FTORAG = line.Substring(227, 6).Trim();
                ddt.FTMLT0 = line.Substring(233, 5).Trim() == "" ? "" : $"{Convert.ToInt32(line.Substring(233, 5).Trim())},{Convert.ToInt32(line.Substring(237, 2).Trim())}";
                ddt.Importo2 = line.Substring(240, 12).Trim() == "" ? 0 : Convert.ToDecimal($"{Convert.ToInt32(line.Substring(240, 12).Trim())},{Convert.ToInt32(line.Substring(251, 3).Trim())}");
                ddt.FTIMRA = line.Substring(255, 12).Trim() == "" ? "" : $"{Convert.ToInt32(line.Substring(255, 12).Trim())},{Convert.ToInt32(line.Substring(266, 3).Trim())}";
                ddt.AnnoN_ddt = (line.Substring(0, 4).Trim() == "" || line.Substring(12, 6).Trim() == "") ? 0 : Convert.ToInt32($"{Convert.ToInt32(line.Substring(0, 4).Trim())}{Convert.ToInt32(line.Substring(12, 6).Trim())}");
                ddt.IdFornitore = idFornitore;
                ddt.Acquirente = txtAcquirente.Text;
                ddt.PrezzoUnitario = importo / (qta == 0 ? 1 : qta);
                ddts.Add(ddt);
            }
            return ddts;
        }

        public static DateTime GetDateFromString(string dataString)
        {
            //Ottiene un DateTime da una stringa con la data formattata in base alla nostra scelta (in questo caso Italiana "dd/MM/yyyy")
            DateTime data = new DateTime(Convert.ToInt32(dataString.Substring(0, 4)), Convert.ToInt32(dataString.Substring(4, 2)), Convert.ToInt32(dataString.Substring(6, 2)));
            return data;
        }

        /*** INIZIO EVENTI GRIDVIEW ***/
        protected void grdListaDDTMef_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void grdListaDDTMef_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void grdListaDDTMef_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdListaDDTMef.PageIndex = e.NewPageIndex;
            if (txtAnnoInizio.Text == "" && txtAnnoFine.Text == "" && txtDataInizio.Text == "" && txtDataFine.Text == "" &&
                txtCodArt1.Text == "" && txtCodArt2.Text == "" && txtCodArt3.Text == "" &&
                txtDescriCodArt1.Text == "" && txtDescriCodArt3.Text == "" && txtDescriCodArt3.Text == "")
                BindGrid();
            else
                BindGridWithSearch();
        }

        /*** INIZIO METODI PER AGGIORNAMENTO GRIDVIEW ***/
        protected void BindGrid()
        {
            List<DDTMef> listaDDT = new List<DDTMef>();
            listaDDT = DDTMefDAO.getDDTList();
            grdListaDDTMef.DataSource = listaDDT;
            grdListaDDTMef.DataBind();
            GetBasicValuesForRecap();
        }

        private void GetBasicValuesForRecap()
        {
            txtMedia.Text = DDTMefDAO.calcolaMediaPrezzoUnitario().ToString("0.00") + " €";
            txtTotDDT.Text = DDTMefDAO.GetTotalDDT().ToString("N2") + " €";
            txtImponibileDDT.Text = DDTMefDAO.GetImponibileDDT().ToString("N2") + " €";
            txtIvaDDT.Text = DDTMefDAO.GetIvaDDT().ToString("N2") + " €";
        }

        protected void BindGridWithSearch()
        {
            List<DDTMef> listaDDT = new List<DDTMef>();
            DDTMefObject ddt = FillDdtObject();

            // Rigenero la griglia
            listaDDT = DDTMefDAO.searchFilter(ddt);
            grdListaDDTMef.DataSource = listaDDT;
            grdListaDDTMef.DataBind();

            //Rigenero il valore della media dei prezzi unitari
            ddt = FillDdtObject();
            txtMedia.Text = DDTMefDAO.calcolaMediaPrezzoUnitarioWithSearch(ddt).ToString("0.00") + " €";
            ddt = FillDdtObject();
            txtTotDDT.Text = DDTMefDAO.GetTotalDDT(ddt).ToString("N2") + " €";
            ddt = FillDdtObject();
            txtImponibileDDT.Text = DDTMefDAO.GetImponibileDDT(ddt).ToString("N2") + " €";
            ddt = FillDdtObject();
            txtIvaDDT.Text = DDTMefDAO.GetIvaDDT(ddt).ToString("N2") + " €";
        }

        /*** HELPERS ***/
        protected DDTMefObject FillDdtObject()
        {
            DDTMefObject ddt = new DDTMefObject();
            ddt.AnnoInizio = txtAnnoInizio.Text;
            ddt.AnnoFine = txtAnnoFine.Text;
            ddt.DataInizio = txtDataInizio.Text;
            ddt.DataFine = txtDataFine.Text;
            ddt.Qta = txtQta.Text;
            ddt.NDdt = txtN_DDT.Text;
            ddt.CodArt1 = txtCodArt1.Text;
            ddt.CodArt2 = txtCodArt2.Text;
            ddt.CodArt3 = txtCodArt3.Text;
            ddt.DescriCodArt1 = txtDescriCodArt1.Text;
            ddt.DescriCodArt2 = txtDescriCodArt2.Text;
            ddt.DescriCodArt3 = txtDescriCodArt3.Text;
            return ddt;
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
    }
}