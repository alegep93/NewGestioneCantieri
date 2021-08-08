using Database.DAO;
using GestioneCantieri.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri.GestioneOrdineFrutti
{
    public partial class OrdineDaDefault : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDdlScegliCantiere();
                FillDdlScegliLocale();
            }
        }

        protected void FillDdlScegliCantiere()
        {
            ddlScegliCantiere.Items.Clear();
            ddlScegliCantiere.Items.Add(new ListItem("", "-1"));
            DropDownListManager.FillDdlCantieri(CantieriDAO.GetAll().Where(w => !w.Chiuso).ToList(), ref ddlScegliCantiere);
        }

        protected void FillDdlScegliLocale()
        {
            ddlScegliLocaleDefault.Items.Clear();
            ddlScegliLocale.Items.Clear();
            ddlScegliLocaleDefault.Items.Add(new ListItem("", "-1"));
            ddlScegliLocale.Items.Add(new ListItem("", "-1"));
            DropDownListManager.FillDdlLocali(LocaliDAO.GetAll(), ref ddlScegliLocale);
            DropDownListManager.FillDdlLocali(LocaliDAO.GetOnlyFirstTypeOfLocale(), ref ddlScegliLocaleDefault);
        }

        protected void btnInserisciDaDefault_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlScegliCantiere.SelectedValue != "-1")
                {
                    if (ddlScegliLocale.SelectedValue != "-1" && ddlScegliLocaleDefault.SelectedValue != "-1")
                    {
                        OrdineFruttiDAO.InserisciDaDefault(Convert.ToInt32(ddlScegliCantiere.SelectedValue), Convert.ToInt32(ddlScegliLocale.SelectedValue), Convert.ToInt32(ddlScegliLocaleDefault.SelectedValue));
                        (Master as layout).SetAlert("alert-success", $"Dati di default correttamente inseriti da {ddlScegliLocaleDefault.SelectedItem.Text} a {ddlScegliLocale.SelectedItem.Text}");
                    }
                    else
                    {
                        (Master as layout).SetAlert("alert-warning", $"È necessario impostare entrambi i locali (sorgente e destinazione)");
                    }
                }
                else
                {
                    (Master as layout).SetAlert("alert-warning", $"È necessario selezionare un cantiere prima di inserire un locale di default");
                }
            }
            catch (Exception ex)
            {
                (Master as layout).SetAlert("alert-danger", $"Errore durante l'inserimento da Default ===> {ex.Message}");
            }
        }
    }
}