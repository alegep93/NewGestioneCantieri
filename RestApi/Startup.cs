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
        }
    }
}
