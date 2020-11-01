﻿using GestioneCantieri.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class layout : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SetAlert(string cssClass, string messaggio)
        {
            AlertManager.ShowAlert(pnlAlert, lblAlert, cssClass, messaggio);
        }
    }
}