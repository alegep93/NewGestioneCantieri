using Dapper;
using log4net;
using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using System.Web.Http;

[assembly: OwinStartup(typeof(RestApi.Startup))]

namespace RestApi
{
    public class Startup
    {
        private static readonly ILog log = LogManager.GetLogger("FileLog");

        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            log4net.Config.XmlConfigurator.Configure();
            log.Info("*** Application start ***");
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
