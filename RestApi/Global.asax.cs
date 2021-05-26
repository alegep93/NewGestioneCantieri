using Dapper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Web.SessionState;

namespace RestApi
{
    public class Global : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger("FileLog");

        protected void Application_Start(object sender, EventArgs e)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            log4net.Config.XmlConfigurator.Configure();
            log.Info("*** Application start ***");
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            HttpConfiguration config = new HttpConfiguration();
            config.EnableCors();
            WebApiConfig.Register(config);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}