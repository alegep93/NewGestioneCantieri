using System;
using System.Web.UI.WebControls;

namespace GestioneCantieri.Utils
{
    public class AlertManager
    {
        public static void ShowAlert(Panel pnlAlert, Label lblMsg, string cssClass, string message)
        {
            pnlAlert.Visible = true;
            pnlAlert.CssClass = $"row mt-3 d-flex justify-content-center align-items-center alert {cssClass} text-center";
            lblMsg.Text = message;
        }

        public static void HideAlert(Panel pnlAlert)
        {
            pnlAlert.Visible = false;
        }
    }
}