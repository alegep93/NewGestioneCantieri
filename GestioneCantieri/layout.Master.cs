using GestioneCantieri.Utils;
using System;
using System.Web.UI;

namespace GestioneCantieri
{
    public partial class layout : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SetAlert(string cssClass, string messaggio)
        {
            AlertManager.ShowAlert(pnlAlert, lblAlert, cssClass, messaggio);
        }

        public void SetModal(string messaggio)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", $"openModal('{messaggio}');", true);
        }
    }
}