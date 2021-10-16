using Database.DAO;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web.UI;
using Utils;

namespace GestioneCantieri
{
    public partial class Listino : Page
    {
        readonly string filePath = ConfigurationManager.AppSettings["Mamg0"];
        readonly string filePathTxt = ConfigurationManager.AppSettings["Mamg0Txt"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SvuotaCampi();
                txtCodArt1.Focus();
                _isPostBack.Value = "1";
            }
            else
            {
                _isPostBack.Value = "0";
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
            DBTransaction tr = new DBTransaction();
            tr.Begin();
            try
            {
                Mamg0DAO.EliminaListino(tr);
                tr.Commit();
            }
            catch (Exception ex)
            {
                tr.Rollback();
                throw new Exception("Errore durante l'eliminazione del listino MEF", ex);
            }
            Response.Redirect("~/Listino.aspx");
        }
        protected void btn_ImportaListino_Click(object sender, EventArgs e)
        {
            DBTransaction tr = new DBTransaction();
            tr.Begin();
            try
            {
                // Elimino il listino
                Mamg0DAO.EliminaListino(tr);

                // Reinserisco tutto il listino Mamg0, recuperando i dati da un file excel nominato Mamg0.xlsx
                Mamg0DAO.GetDataFromExcelAndInsertBulkCopy(filePath, tr);

                tr.Commit();
                lblImportMsg.Text = "Importazione del listino avvenuta con successo";
                lblImportMsg.ForeColor = Color.Blue;
            }
            catch (Exception ex)
            {
                tr.Rollback();
                lblImportMsg.Text = "Errore durante l'importazione del listino. <br />" + ex;
                lblImportMsg.ForeColor = Color.Red;
            }

            // Aggiorno la tabella di visualizzazione risultati
            BindGrid();
        }

        /* HELPERS */
        protected void BindGrid()
        {
            List<Mamg0> listaDDT = new List<Mamg0>();
            listaDDT = Mamg0DAO.GetAll();
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
        //private List<Mamg0> ReadDataFromTextFile()
        //{
        //    List<Mamg0> ddts = new List<Mamg0>();
        //    try
        //    {
        //        string[] lines = File.ReadAllLines(filePathTxt);
        //        foreach (string line in lines)
        //        {
        //            Mamg0 mamgo = new Mamg0();
        //            //int qta = line.Substring(174, 8).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(174, 8).Trim());
        //            //decimal importo = line.Substring(185, 12).Trim() == "" ? 0 : Convert.ToDecimal($"{Convert.ToInt32(line.Substring(185, 12).Trim())},{Convert.ToInt32(line.Substring(197, 3).Trim())}");
        //            //string sigf = line.Substring(41, 3).Trim();
        //            //string codf = line.Substring(44, 16).Trim();

        //            //DateTime data = GetDateFromString(line.Substring(4, 8).Trim()).Value;
        //            //DateTime data2 = GetDateFromString(line.Substring(200, 8).Trim()) ?? data;
        //            //DateTime data3 = GetDateFromString(line.Substring(219, 8).Trim()) ?? data;

        //            mamgo.Anno = line.Substring(0, 4).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(0, 4).Trim());
        //            mamgo.Data = line.Substring(4, 8).Trim() == "" ? new DateTime() : data;
        //            mamgo.N_DDT = line.Substring(12, 6).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(12, 6).Trim());
        //            mamgo.FTVRF0 = line.Substring(18, 15).Trim();
        //            mamgo.FTDT30 = line.Substring(33, 41).Trim();
        //            mamgo.CodArt = line.Substring(41, 19).Trim();
        //            mamgo.FTAIN = line.Substring(60, 17).Trim();
        //            mamgo.DescriCodArt = line.Substring(77, 40).Trim();
        //            mamgo.DescrizioneArticolo2 = line.Substring(117, 40).Trim();
        //            mamgo.Iva = line.Substring(157, 2).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(157, 2).Trim());
        //            //ddt.PrezzoListino = line.Substring(159, 12).Trim() == "" ? 0 : Convert.ToDecimal($"{Convert.ToInt32(line.Substring(159, 12).Trim())},{Convert.ToInt32(line.Substring(170, 3).Trim())}");
        //            mamgo.PrezzoListino = Convert.ToDecimal(Mamg0DAO.GetPrezzoDiListino(sigf, codf));
        //            mamgo.Qta = qta;
        //            mamgo.Importo = importo;
        //            mamgo.Data2 = line.Substring(200, 8).Trim() == "" ? new DateTime() : data2;
        //            mamgo.Valuta = line.Substring(208, 3).Trim();
        //            mamgo.FTFOM = line.Substring(211, 1).Trim();
        //            mamgo.FTCMA = line.Substring(212, 2).Trim();
        //            mamgo.FTCDO = line.Substring(214, 2).Trim();
        //            mamgo.FLFLAG = line.Substring(216, 1).Trim();
        //            mamgo.FLFLQU = line.Substring(217, 2).Trim();
        //            mamgo.Data3 = line.Substring(219, 8).Trim() == "" ? new DateTime() : data3;
        //            mamgo.FTORAG = line.Substring(227, 6).Trim();
        //            mamgo.FTMLT0 = line.Substring(233, 5).Trim() == "" ? "" : $"{Convert.ToInt32(line.Substring(233, 5).Trim())},{Convert.ToInt32(line.Substring(238, 2).Trim())}";
        //            mamgo.Importo2 = line.Substring(240, 12).Trim() == "" ? 0 : Convert.ToDecimal($"{Convert.ToInt32(line.Substring(240, 12).Trim())},{Convert.ToInt32(line.Substring(252, 3).Trim())}");
        //            mamgo.FTIMRA = line.Substring(255, 12).Trim() == "" ? "" : $"{Convert.ToInt32(line.Substring(255, 12).Trim())},{Convert.ToInt32(line.Substring(267, 3).Trim())}";
        //            mamgo.AnnoN_ddt = (line.Substring(0, 4).Trim() == "" || line.Substring(12, 6).Trim() == "") ? 0 : Convert.ToInt32($"{Convert.ToInt32(line.Substring(0, 4).Trim())}{Convert.ToInt32(line.Substring(12, 6).Trim())}");
        //            mamgo.IdFornitore = idFornitore;
        //            mamgo.Acquirente = txtAcquirente.Text;
        //            mamgo.PrezzoUnitario = importo / (qta == 0 ? 1 : qta);
        //            ddts.Add(mamgo);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Errore durante il popolamento della lista dei DDT Mef da ddt.txt", ex);
        //    }
        //    return ddts;
        //}
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
    }
}