using Database.DAO;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
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
                // Leggo il file .txt e aggiorno il listino con una "merge" per inserire cosa non c'è e aggiornare cosa è già presente
                List<Mamg0> items = ReadDataFromTextFile();
                Mamg0DAO.MergeListino(items, tr);
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
            string codArt = "";
            string desc = "";
            string prezzoNettoIntero = "";
            string prezzoNettoDecimale = "";
            string prezzoListinoIntero = "";
            string prezzoListinoDecimale = "";

            try
            {
                List<string> codiciMefDaImportare = CodiciMefDAO.GetAll();
                CultureInfo cultures = new CultureInfo("en-US");
                string[] lines = File.ReadAllLines(filePathTxt);
                foreach (string line in lines)
                {
                    if (line.Trim().StartsWith("LISTINO"))
                    {
                        continue;
                    }

                    // Verifico che il codice che sto leggendo esista nella tabella dei codici da importare, altrimenti passo alla linea successiva
                    string codiceMef = line.Substring(0, 4); // (Sigla marchio)
                    if (codiciMefDaImportare.IndexOf(codiceMef) >= 0)
                    {
                        try
                        {
                            codArt = line.Substring(0, 20).Replace(" ", "").Trim(); // AA_SIGF + AA_CODF (Sigla marchio + Codice Prodotto Produttore)
                            desc = line.Substring(33, 43).Trim(); // AA_DES (Descrizione prodotto)
                            prezzoNettoIntero = line.Substring(98, 9).Replace("-", "").Trim(); // AA_PRZ1 parte intera (Prezzo al grossista)
                            prezzoNettoDecimale = line.Substring(107, 2).Replace("-", "").Trim(); // AA_PRZ1 decimali (Prezzo al grossista)
                            prezzoListinoIntero = line.Substring(109, 9).Replace("-", "").Trim(); // AA_PRZ parte intera (Prezzo al pubblico)
                            prezzoListinoDecimale = line.Substring(118, 2).Replace("-", "").Trim(); // AA_PRZ decimali (Prezzo al pubblico)
                            decimal moltiplicatore = Convert.ToDecimal(line.Substring(120, 6).Trim());
                            Mamg0 mamgo = new Mamg0
                            {
                                CodArt = codArt == "" ? "" : codArt,
                                Desc = desc == "" ? "" : desc,
                                Pezzo = 0,
                                PrezzoListino = prezzoListinoIntero == "" ? 0 : Convert.ToDecimal($"{prezzoListinoIntero}.{(prezzoListinoDecimale == "" ? "0" : prezzoListinoDecimale)}", cultures) / moltiplicatore,
                                PrezzoNetto = prezzoNettoIntero == "" ? 0 : Convert.ToDecimal($"{prezzoNettoIntero}.{(prezzoNettoDecimale == "" ? "0" : prezzoNettoDecimale)}", cultures) / moltiplicatore,
                                CodiceFornitore = codiceMef
                            };
                            mamgoList.Add(mamgo);
                        }
                        catch { continue; }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
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