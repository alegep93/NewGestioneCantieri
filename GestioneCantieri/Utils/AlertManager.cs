using System.Web.UI.WebControls;

namespace GestioneCantieri.Utils
{
    public class AlertManager
    {
        public static void ShowAlert(Panel pnlMsg, Label lblMsg, string cssClass, string message)
        {
            pnlMsg.Visible = true;
            pnlMsg.CssClass = $"row mt-3 d-flex justify-content-center align-items-center alert {cssClass} text-center";
            lblMsg.Text = message;
        }
    }
}