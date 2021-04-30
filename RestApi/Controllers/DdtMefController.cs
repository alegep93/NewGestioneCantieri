using System;
using System.Web.Http;
using System.Web.Http.Description;
using Database.Models;
using Database.DAO;
using System.Collections.Generic;
using log4net;

namespace RestApi.Controllers
{
    [RoutePrefix("api/DdtMef")]
    public class DdtMefController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger("FileLog");

        [HttpGet]
        [ResponseType(typeof(DDTMef))]
        [Route("CompleteList")]
        public IHttpActionResult GetAll()
        {
            try
            {
                List<DDTMef> items = DDTMefDAO.GetAll();
                return Ok(items);
            }
            catch (Exception ex)
            {
                string messaggio = $"Errore durante la GetAll in DdtMefController --- {ex}";
                log.Error(messaggio);
                return BadRequest(messaggio);
            }
        }
    }
}