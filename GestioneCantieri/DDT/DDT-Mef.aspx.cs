using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            List<DDTMef> items = ddt == null ? DDTMefDAO.GetAll() : DDTMefDAO.GetDdt(ddt);
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
                    int qta = line.Substring(174, 8).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(174, 8).Trim());
                    decimal importo = line.Substring(185, 12).Trim() == "" ? 0 : Convert.ToDecimal($"{Convert.ToInt32(line.Substring(185, 12).Trim())},{Convert.ToInt32(line.Substring(197, 3).Trim())}");
                    string sigf = line.Substring(41, 3).Trim();
                    string codf = line.Substring(44, 16).Trim();

                    DateTime data = GetDateFromString(line.Substring(4, 8).Trim()).Value;
                    DateTime data2 = GetDateFromString(line.Substring(200, 8).Trim()) ?? data;
                    DateTime data3 = GetDateFromString(line.Substring(219, 8).Trim()) ?? data;

                    ddt.Anno = line.Substring(0, 4).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(0, 4).Trim());
                    ddt.Data = line.Substring(4, 8).Trim() == "" ? new DateTime() : data;
                    ddt.N_DDT = line.Substring(12, 6).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(12, 6).Trim());
                    ddt.FTVRF0 = line.Substring(18, 15).Trim();
                    ddt.FTDT30 = line.Substring(33, 41).Trim();
                    ddt.CodArt = line.Substring(41, 19).Trim();
                    ddt.FTAIN = line.Substring(60, 17).Trim();
                    ddt.DescriCodArt = line.Substring(77, 40).Trim();
                    ddt.DescrizioneArticolo2 = line.Substring(117, 40).Trim();
                    ddt.Iva = line.Substring(157, 2).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(157, 2).Trim());
                    //ddt.PrezzoListino = line.Substring(159, 12).Trim() == "" ? 0 : Convert.ToDecimal($"{Convert.ToInt32(line.Substring(159, 12).Trim())},{Convert.ToInt32(line.Substring(170, 3).Trim())}");
                    ddt.PrezzoListino = Convert.ToDecimal(Mamg0DAO.GetPrezzoDiListino(sigf, codf));
                    ddt.Qta = qta;
                    ddt.Importo = importo;
                    ddt.Data2 = line.Substring(200, 8).Trim() == "" ? new DateTime() : data2;
                    ddt.Valuta = line.Substring(208, 3).Trim();
                    ddt.FTFOM = line.Substring(211, 1).Trim();
                    ddt.FTCMA = line.Substring(212, 2).Trim();
                    ddt.FTCDO = line.Substring(214, 2).Trim();
                    ddt.FLFLAG = line.Substring(216, 1).Trim();
                    ddt.FLFLQU = line.Substring(217, 2).Trim();
                    ddt.Data3 = line.Substring(219, 8).Trim() == "" ? new DateTime() : data3;
                    ddt.FTORAG = line.Substring(227, 6).Trim();
                    ddt.FTMLT0 = line.Substring(233, 5).Trim() == "" ? "" : $"{Convert.ToInt32(line.Substring(233, 5).Trim())},{Convert.ToInt32(line.Substring(238, 2).Trim())}";
                    ddt.Importo2 = line.Substring(240, 12).Trim() == "" ? 0 : Convert.ToDecimal($"{Convert.ToInt32(line.Substring(240, 12).Trim())},{Convert.ToInt32(line.Substring(252, 3).Trim())}");
                    ddt.FTIMRA = line.Substring(255, 12).Trim() == "" ? "" : $"{Convert.ToInt32(line.Substring(255, 12).Trim())},{Convert.ToInt32(line.Substring(267, 3).Trim())}";
                    ddt.AnnoN_ddt = (line.Substring(0, 4).Trim() == "" || line.Substring(12, 6).Trim() == "") ? 0 : Convert.ToInt32($"{Convert.ToInt32(line.Substring(0, 4).Trim())}{Convert.ToInt32(line.Substring(12, 6).Trim())}");
                    ddt.IdFornitore = idFornitore;
                    ddt.Acquirente = txtAcquirente.Text;
                    ddt.PrezzoUnitario = importo / (qta == 0 ? 1 : qta);
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