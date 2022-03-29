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
                Mamg0DAO.DeleteListino(tr);
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
                //// Elimino il listino
                //Mamg0DAO.EliminaListino(tr);

                //// Reinserisco tutto il listino Mamg0, recuperando i dati da un file excel nominato Mamg0.xlsx
                //Mamg0DAO.GetDataFromExcelAndInsertBulkCopy(filePath, tr);

                // Nuovo metodo, con txt
                // Elimino il listino
                List<Mamg0> items = ReadDataFromTextFile();
                Mamg0DAO.DeleteListino(tr);
                Mamg0DAO.InsertListino(items, tr);

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
            listaDDT = Mamg0DAO.GetListino("", "", "", "", "", "", true);
            grdListino.DataSource = listaDDT;
            grdListino.DataBind();
        }
        protected void BindGridWithSearch()
        {
            List<Mamg0> listaDDT = new List<Mamg0>();
            listaDDT = Mamg0DAO.GetListino(txtCodArt1.Text, txtCodArt2.Text, txtCodArt3.Text, txtDescriCodArt1.Text, txtDescriCodArt2.Text, txtDescriCodArt3.Text, false);
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
        private List<Mamg0> ReadDataFromTextFile()
        {
            List<Mamg0> mamgoList = new List<Mamg0>();
            //Mamg0 mamgo = new Mamg0();
            //string lineCheck = "";
            try
            {
                string[] lines = File.ReadAllLines(filePathTxt);
                foreach (string line in lines)
                {
                    if (line.Trim().StartsWith("LISTINO"))
                    {
                        continue;
                    }

                    string codArt = line.Substring(0, 20).Trim();  // AA_COD
                    string desc = line.Substring(40, 40).Trim();  // AA_DES
                    string pezzo = line.Substring(83, 5).Trim(); // AA_PZ
                    string prezzoListinoIntero = line.Substring(104, 8).Replace("-", "").Trim(); // AA_PRZ parte intera
                    string prezzoListinoDecimale = line.Substring(112, 5).Replace("-", "").Trim(); // AA_PRZ decimali
                    string prezzoNettoIntero = line.Substring(185, 8).Replace("-", "").Trim(); // AA_PRZ1 parte intera
                    string prezzoNettoDecimale = line.Substring(193, 5).Replace("-", "").Trim(); // AA_PRZ1 decimali

                    //lineCheck = line;
                    Mamg0 mamgo = new Mamg0
                    {
                        CodArt = codArt == "" ? "" : line.Substring(0, 20).Trim(),
                        Desc = desc == "" ? "" : line.Substring(39, 40).Trim(),
                        Pezzo = pezzo == "" ? 0 : Convert.ToInt32(pezzo),
                        PrezzoListino = prezzoListinoIntero == "" ? 0 : Convert.ToDecimal($"{Convert.ToInt32(prezzoListinoIntero)},{Convert.ToInt32(prezzoListinoDecimale)}"),
                        PrezzoNetto = prezzoNettoIntero == "" ? 0 : Convert.ToDecimal($"{Convert.ToInt32(prezzoNettoIntero)},{Convert.ToInt32(prezzoNettoDecimale)}")
                    };
                    mamgoList.Add(mamgo);
                }
            }
            catch (Exception ex)
            {
                //var x = mamgo;
                //var y = lineCheck;
                throw new Exception("Errore durante il popolamento dei listini Mef da ddt.txt", ex);
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