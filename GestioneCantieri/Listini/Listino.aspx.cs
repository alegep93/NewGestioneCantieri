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

                // Nuovo metodo, con txt
                //List<Mamg0ForDBF> items = ReadDataFromTextFile();
                //Mamg0DAO.InsertAll(items, tr);

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
        private List<Mamg0ForDBF> ReadDataFromTextFile()
        {
            List<Mamg0ForDBF> mamgoList = new List<Mamg0ForDBF>();
            try
            {
                string[] lines = File.ReadAllLines(filePathTxt);
                foreach (string line in lines)
                {
                    Mamg0ForDBF mamgo = new Mamg0ForDBF();
                    //int qta = line.Substring(174, 8).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(174, 8).Trim());
                    //decimal importo = line.Substring(185, 12).Trim() == "" ? 0 : Convert.ToDecimal($"{Convert.ToInt32(line.Substring(185, 12).Trim())},{Convert.ToInt32(line.Substring(197, 3).Trim())}");
                    //string sigf = line.Substring(41, 3).Trim();
                    //string codf = line.Substring(44, 16).Trim();

                    //DateTime data = GetDateFromString(line.Substring(4, 8).Trim()).Value;
                    //DateTime data2 = GetDateFromString(line.Substring(200, 8).Trim()) ?? data;
                    //DateTime data3 = GetDateFromString(line.Substring(219, 8).Trim()) ?? data;

                    mamgo.AA_IVA = line.Substring(0, 3).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(0, 3).Trim());
                    mamgo.AA_IVA = line.Substring(3, 16).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(3, 16).Trim());
                    mamgo.AA_IVA = line.Substring(19, 13).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(19, 13).Trim());
                    mamgo.AA_IVA = line.Substring(32, 43).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(32, 43).Trim());
                    mamgo.AA_IVA = line.Substring(75, 5).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(75, 5).Trim());
                    mamgo.AA_IVA = line.Substring(80, 5).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(80, 5).Trim());
                    mamgo.AA_IVA = line.Substring(85, 5).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(85, 5).Trim());
                    mamgo.AA_IVA = line.Substring(90, 6).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(90, 6).Trim());
                    mamgo.AA_IVA = line.Substring(96, 1).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(96, 1).Trim());
                    mamgo.AA_IVA = line.Substring(97, 11).Trim() == "" ? 0 : Convert.ToInt32($"{Convert.ToInt32(line.Substring(97, 9).Trim())},{Convert.ToInt32(line.Substring(106, 2).Trim())}");
                    mamgo.AA_IVA = line.Substring(108, 11).Trim() == "" ? 0 : Convert.ToInt32($"{Convert.ToInt32(line.Substring(108, 9).Trim())},{Convert.ToInt32(line.Substring(117, 2).Trim())}");
                    mamgo.AA_IVA = line.Substring(119, 6).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(119, 6).Trim());
                    mamgo.AA_IVA = line.Substring(125, 3).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(125, 3).Trim());
                    mamgo.AA_IVA = line.Substring(128, 3).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(128, 3).Trim());
                    mamgo.AA_IVA = line.Substring(131, 1).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(131, 1).Trim());
                    mamgo.AA_IVA = line.Substring(132, 1).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(132, 1).Trim());
                    mamgo.AA_IVA = line.Substring(133, 8).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(133, 8).Trim());
                    mamgo.AA_IVA = line.Substring(141, 18).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(141, 18).Trim());
                    mamgo.AA_IVA = line.Substring(159, 18).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(159, 18).Trim());
                    mamgo.AA_IVA = line.Substring(177, 21).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(177, 21).Trim());
                    mamgo.AA_IVA = line.Substring(198, 7).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(198, 7).Trim());
                    mamgo.AA_IVA = line.Substring(205, 4).Trim() == "" ? 0 : Convert.ToInt32(line.Substring(205, 4).Trim());
                    mamgo.AA_IVA = line.Substring(209, 11).Trim() == "" ? 0 : Convert.ToInt32($"{Convert.ToInt32(line.Substring(209, 8).Trim())},{Convert.ToInt32(line.Substring(217, 3).Trim())}");

                    mamgoList.Add(mamgo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il popolamento della lista dei DDT Mef da ddt.txt", ex);
            }
            return mamgoList;
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
    }
}