using Database.DAO;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class Default : System.Web.UI.Page
    {
        //readonly string filePath = ConfigurationManager.AppSettings["DdtMef"];
        readonly string filePathTxt = ConfigurationManager.AppSettings["DdtMefTxt"];

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
            //List<DDTMef> ddtList = DDTMefDAO.GetDdtFromDBF(filePath, txtAcquirente.Text, idFornitore);
            List<DDTMef> ddtList = ReadDataFromTextFile(idFornitore);

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
            List<DDTMef> items = ddt == null ? DDTMefDAO.GetAll(500) : DDTMefDAO.GetDdt(ddt);
            grdListaDDTMef.DataSource = items;
            grdListaDDTMef.DataBind();

            // Imposto i campi di riepilogo
            txtMedia.Text = (items.Sum(s => s.PrezzoUnitario) / (items.Count() == 0 ? 1 : items.Count())).ToString("0.00") + " €";
            txtTotDDT.Text = items.Sum(s => s.Importo).ToString("N2") + " €";
            txtImponibileDDT.Text = items.Sum(s => (s.Importo * 100) / (100 + s.Iva)).ToString("N2") + " €";
            txtIvaDDT.Text = items.Sum(s => s.Importo - (100 * s.Importo / (100 + s.Iva))).ToString("N2") + " €";
        }

        private List<DDTMef> ReadDataFromTextFile(int idFornitore)
        {
            List<DDTMef> ddts = new List<DDTMef>();
            try
            {
                string[] lines = File.ReadAllLines(filePathTxt);
                foreach (string line in lines)
                {
                    DDTMef ddt = new DDTMef();
                    CultureInfo cultures = new CultureInfo("en-US");
                    int qta = line.Substring(215, 8).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(215, 8).Trim());
                    decimal unitarioNetto = line.Substring(226, 13).Trim() == "" ? 0 : Convert.ToDecimal($"{line.Substring(226, 13).Trim()}.{line.Substring(239, 2).Trim()}", cultures);

                    ddt.Anno = line.Substring(0, 4).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(0, 4).Trim());
                    ddt.Data = line.Substring(4, 8).Trim() == "" ? new DateTime() : GetDateFromString(line.Substring(4, 8).Trim()).Value;
                    ddt.N_DDT = line.Substring(12, 10).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(12, 10).Trim());
                    ddt.FTVRF0 = line.Substring(22, 35).Trim();
                    ddt.FTDT30 = line.Substring(57, 8).Trim();
                    ddt.CodArt = line.Substring(65, 35).Trim();
                    ddt.DescriCodArt = line.Substring(118, 40).Trim();
                    ddt.DescrizioneArticolo2 = line.Substring(158, 40).Trim();
                    ddt.Iva = line.Substring(198, 2).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(198, 2).Trim());
                    ddt.PrezzoListino = line.Substring(200, 12).Trim() == "" ? 0 : Convert.ToDecimal($"{line.Substring(200, 12).Trim()}.{line.Substring(212, 3).Trim()}", cultures); // Prezzo Lordo
                    ddt.Qta = qta;
                    ddt.Importo = unitarioNetto * (qta == 0 ? 1 : qta);
                    ddt.IdFornitore = idFornitore;
                    ddt.Acquirente = txtAcquirente.Text;
                    ddt.PrezzoUnitario = unitarioNetto;
                    ddts.Add(ddt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il popolamento della lista dei DDT Mef da ddt.txt", ex);
            }
            return ddts;
        }

        public static DateTime? GetDateFromString(string dataString)
        {
            //Ottiene un DateTime da una stringa con la data formattata in base alla nostra scelta (in questo caso Italiana "dd/MM/yyyy")
            DateTime? data = null;
            try
            {
                data = new DateTime(Convert.ToInt32(dataString.Substring(0, 4)), Convert.ToInt32(dataString.Substring(4, 2)), Convert.ToInt32(dataString.Substring(6, 2)));
            }
            catch { }
            return data;
        }
        #endregion
    }
}