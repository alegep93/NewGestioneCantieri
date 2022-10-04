using Database.DAO;
using Database.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
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
            //DBTransaction tr = new DBTransaction();
            //tr.Begin();
            //try
            //{
            //    Mamg0DAO.DeleteListino(tr);
            //    tr.Commit();
            //}
            //catch (Exception ex)
            //{
            //    tr.Rollback();
            //    throw new Exception("Errore durante l'eliminazione del listino MEF", ex);
            //}
            //Response.Redirect("~/Listino.aspx");
        }

        protected void btn_ImportaListino_Click(object sender, EventArgs e)
        {
            //DBTransaction tr = new DBTransaction();
            //tr.Begin();
            try
            {
                // Leggo il file .txt e aggiorno il listino con una "merge" per inserire cosa non c'è e aggiornare cosa è già presente
                DataTable dt = ToDataTable(ReadDataFromTextFile());

                using (SqlConnection cn = BaseDAO.GetConnection())
                {
                    // Svuoto e re-inserisco nella tabella Tmp
                    Mamg0DAO.DeleteListino(true, cn);
                    SqlBulkCopyMethod(dt, cn);
                    //Mamg0DAO.Insert(items, true, cn);

                    // Controllo le differenze dalla tabella finale
                    List<Mamg0> diffList = Mamg0DAO.GetDifferencesFromTmp(cn);

                    // Inserisco solo i nuovi elementi
                    Mamg0DAO.Insert(diffList, false, cn);
                }

                //tr.Commit();
                lblImportMsg.Text = "Importazione del listino avvenuta con successo";
                lblImportMsg.ForeColor = Color.Blue;
            }
            catch (Exception ex)
            {
                //tr.Rollback();
                lblImportMsg.Text = "Errore durante l'importazione del listino. <br />" + ex;
                lblImportMsg.ForeColor = Color.Red;
            }

            // Aggiorno la tabella di visualizzazione risultati
            BindGrid();
        }

        private void SqlBulkCopyMethod(DataTable dt, SqlConnection cn)
        {
            // make sure to enable triggers
            // more on triggers in next post
            SqlBulkCopy bulkCopy = new SqlBulkCopy(cn,
                SqlBulkCopyOptions.TableLock |
                SqlBulkCopyOptions.FireTriggers |
                SqlBulkCopyOptions.UseInternalTransaction,
                null
                );

            bulkCopy.ColumnMappings.Add("CodArt", "CodArt");
            bulkCopy.ColumnMappings.Add("Desc", "Desc");
            bulkCopy.ColumnMappings.Add("Pezzo", "Pezzo");
            bulkCopy.ColumnMappings.Add("PrezzoListino", "PrezzoListino");
            bulkCopy.ColumnMappings.Add("PrezzoNetto", "PrezzoNetto");
            bulkCopy.ColumnMappings.Add("CodiceFornitore", "CodiceFornitore");

            // set the destination table name
            bulkCopy.DestinationTableName = "dbo.TblListinoMefTmp";

            // write the data in the "dataTable"
            bulkCopy.WriteToServer(dt);
        }

        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        /* HELPERS */
        protected List<List<Mamg0>> ChunkBy(List<Mamg0> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

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
                List<string> lines = File.ReadAllLines(filePathTxt).ToList();

                // Prendo solamente le righe che hanno un codice esistente nella tabella dei codici da importare
                lines = lines.Where(w => codiciMefDaImportare.Any(a => a.Contains(w.Substring(0, 4)))).ToList();
                foreach (string line in lines)
                {
                    if (line.Trim().StartsWith("LISTINO"))
                    {
                        continue;
                    }

                    string codiceMef = line.Substring(0, 4); // (Sigla marchio)
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