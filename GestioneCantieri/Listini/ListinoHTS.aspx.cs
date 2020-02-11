using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class ListinoHTS : System.Web.UI.Page
    {
        readonly string filePath = ConfigurationManager.AppSettings["ListinoHts"];


        protected void Page_Load(object sender, EventArgs e)
        {
            txtPercentualeSconto.Text = txtPercentualeSconto.Text != "" ? txtPercentualeSconto.Text : 50.ToString();
            txtPercentualeSconto.Text = !txtPercentualeSconto.Text.Contains("%") ? txtPercentualeSconto.Text + "%" : txtPercentualeSconto.Text;

            if (!IsPostBack)
            {
                SvuotaCampi();
                txtCodice1.Focus();
            }
        }

        /* EVENTI CLICK */
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
        protected void btnSvuotaTxt_Click(object sender, EventArgs e)
        {
            SvuotaCampi();
        }
        protected void btnEliminaListino_Click(object sender, EventArgs e)
        {
            ListinoHtsDAO.Delete();
            Response.Redirect("~/Listino.aspx");
        }

        /* HELPERS */
        protected void BindGrid()
        {
            List<ListinoHts> listaDDT = new List<ListinoHts>();
            listaDDT = ListinoHtsDAO.GetAllFiltered(txtCodice1.Text, txtCodice2.Text, txtCodice3.Text, txtCodProd1.Text, txtCodProd2.Text, txtCodProd3.Text, txtDescriCodProd1.Text, txtDescriCodProd2.Text, txtDescriCodProd3.Text);
            grdListinoHts.DataSource = listaDDT;
            grdListinoHts.DataBind();
        }
        protected void SvuotaCampi()
        {
            txtCodice1.Text = txtCodice2.Text = txtCodice3.Text = "";
            txtCodProd1.Text = txtCodProd2.Text = txtCodProd3.Text = "";
            txtDescriCodProd1.Text = txtDescriCodProd2.Text = txtDescriCodProd3.Text = "";
            BindGrid();
        }

        protected void grdListinoHts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[5].Text = "Prezzo Scontato";
            }
            else
            {
                // Calcolo il prezzo scontato
                decimal sconto = Convert.ToDecimal(txtPercentualeSconto.Text.Replace("%", ""));
                decimal prezzo = e.Row.Cells[4].Text != "&nbsp;" ? Convert.ToDecimal(e.Row.Cells[4].Text) : 0;
                e.Row.Cells[5].Text = (prezzo - prezzo * sconto / 100).ToString();
            }
        }

        protected void txtPercentualeSconto_TextChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        // Prova estrazione dati da Excel
        protected void btnImportaListinoHts_Click(object sender, EventArgs e)
        {
            try
            {
                // Elimino il vecchio listino
                ListinoHtsDAO.Delete();

                // Popolo una lista di elementi e inserisco tutto su DB
                List<ListinoHts> items = GetDataFromExcel();
                ListinoHtsDAO.InsertAll(items);
            }
            catch (Exception ex)
            {
                lblImportMsg.Text = "Errore durante l'importazione del listino HTS" + ex.Message;
            }

            BindGrid();
        }

        private List<ListinoHts> GetDataFromExcel()
        {
            List<ListinoHts> ret = new List<ListinoHts>();
            OleDbConnection ExcelConection = null;

            try
            {
                OleDbConnectionStringBuilder OleStringBuilder = new OleDbConnectionStringBuilder(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + filePath + "; Extended Properties = 'Excel 12.0;HDR=Yes;IMEX=1';Persist Security Info=False;");
                OleStringBuilder.DataSource = filePath;
                ExcelConection = new OleDbConnection();
                ExcelConection.ConnectionString = OleStringBuilder.ConnectionString;

                using (OleDbDataAdapter adaptor = new OleDbDataAdapter("SELECT * FROM [Listino Prezzi$]", ExcelConection))
                {
                    DataSet ds = new DataSet();
                    adaptor.Fill(ds);

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string[] array = new string[7];
                        string line = "";

                        for (int i = 0; i < row.ItemArray.Length; i++)
                        {
                            line += row.ItemArray[i].ToString() + ";";
                        }

                        array = line.Split(';');

                        ListinoHts item = new ListinoHts
                        {
                            Codice = array[0],
                            CodiceProdotto = array[1],
                            Descrizione = array[2],
                            Prezzo = array[3].ToString() != "" ? Convert.ToDecimal(array[3].Replace("€", "").Replace("-", "0").Replace(".", "0").Replace("Su richiesta", "0")) : 0,
                            Cr = array[4],
                            G = array[5],
                            NoteDisponibilita = array[6],
                        };

                        if (item.Codice != "" && item.CodiceProdotto != "")
                        {
                            ret.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'importazione del listino HTS", ex);
            }
            finally
            {
                if (ExcelConection != null)
                {
                    ExcelConection.Dispose();
                }
            }

            return ret;
        }
    }
}