using Database.DAO;
using Database.Models;
using GestioneCantieri.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class StampaExcell : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnPrint.Visible = false;
                ddlScegliCantiere.SelectedIndex = 0;
                FillDdlScegliCantiere();
            }
        }

        #region Eventi Click
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            CreateExcel();
        }
        #endregion

        #region Eventi Text-Changed
        protected void ddlScegliCantiere_TextChanged(object sender, EventArgs e)
        {
            BindRepeater();
            BindGrid();
            btnPrint.Visible = ddlScegliCantiere.SelectedIndex != 0;
        }

        #endregion

        #region Helpers
        private void BindRepeater()
        {
            try
            {
                List<MatOrdFrut> items = OrdineFruttiDAO.GetByIdCantiere(Convert.ToInt32(ddlScegliCantiere.SelectedValue));
                Session["locali"] = items.Select(s => s.IdLocale).ToList();
                rptLocali.DataSource = items;
                rptLocali.DataBind();
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante il GeneraCodiceHtmlLocali in StampaOrdFrutCantExcel ===> {ex.Message}");
            }
        }

        protected void BindGrid()
        {
            List<int> idLocali = GetIdLocali();
            grdFruttiInLocale.DataSource = OrdineFruttiDAO.GetFruttiPerStampaExcel(ddlScegliCantiere.SelectedValue, GetStringFromListForQuery(idLocali));
            grdFruttiInLocale.DataBind();
        }

        public List<int> GetIdLocali()
        {
            return (Session["locali"] != null ? (List<int>)Session["locali"] : new List<int>());
        }

        public static string GetStringFromListForQuery(List<int> list)
        {
            // Converte una lista di int in una stringa formattata per fare una query
            string ret = "";
            foreach (int id in list)
            {
                ret += ret == "" ? $"{id}" : $", {id}";
            }
            return ret;
        }

        protected void CreateExcel()
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=OrdFrut-" + ddlScegliCantiere.SelectedItem.Text + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            htmlWrite.WriteLine("<strong><font size='4'>" + ddlScegliCantiere.SelectedItem.Text + "</font></strong>");
            // viene reindirizzato il rendering verso la stringa in uscita
            grdFruttiInLocale.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }

        protected void FillDdlScegliCantiere()
        {
            ddlScegliCantiere.Items.Clear();
            ddlScegliCantiere.Items.Add(new ListItem("", "-1"));
            DropDownListManager.FillDdlCantieri(CantieriDAO.GetAll().Where(w => !w.Chiuso).ToList(), ref ddlScegliCantiere);
        }
        #endregion

        /* Override per il corretto funzionamento della creazione del foglio Excel */
        public override void VerifyRenderingInServerForm(Control control)
        {
            //Do nothing
        }

        //protected void btnFiltraStampa_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // Vado a cercare tutti i checkbox dentro al pannello "pnlLocaliWrapper"
        //        Panel wrapper = pnlLocaliWrapper.FindControl("pnlLocaliWrapper") as Panel;
        //        foreach (Control panel in pnlLocaliWrapper.Controls)
        //        {
        //            if (panel is Panel)
        //            {
        //                foreach (Control checkbox in panel.Controls)
        //                {
        //                    if (checkbox is CheckBox)
        //                    {
        //                        // Se il checkBox NON è spuntato, rimuovo il suo id dalla lista
        //                        if (!(checkbox as CheckBox).Checked)
        //                        {
        //                            idLocali.Remove(Convert.ToInt32(checkbox.ID.Split('_').Last()));
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        BindGrid();
        //    }
        //    catch (Exception ex)
        //    {
        //        (Master as layout).SetAlert("alert-danger", $"Errore durante il btnFiltraStampa_Click in StampaOrdFrutCanExcel ===> {ex.Message}");
        //    }
        //}

        protected void chkLocale_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = (sender as CheckBox);
                int rowId = Convert.ToInt32(checkBox.ClientID.Split('_').Last());
                int idLocale = Convert.ToInt32((rptLocali.Items[rowId].FindControl("hfIdLocale") as HiddenField).Value);
                List<int> idLocali = GetIdLocali();

                if (checkBox.Checked)
                {
                    if (!idLocali.Contains(idLocale))
                    {
                        idLocali.Add(idLocale);
                    }
                }
                else
                {
                    idLocali.Remove(idLocale);
                }
                BindGrid();
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante il chkLocale_CheckedChanged in StampaOrdFrutCanExcel ===> {ex.Message}");
            }
        }
    }
}