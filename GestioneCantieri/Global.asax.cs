using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace GestioneCantieri
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            LogManager.GetLogger("FileLog").Info("*** Applicazione avviata ***");
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        protected void Application_BeginRequest()
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
        }
    }
}