using Database.DAO;
using Database.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace RestApi.Controllers
{
    [RoutePrefix("api/Cantieri")]
    public class CantieriController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger("FileLog");

        [HttpGet]
        [ResponseType(typeof(Cantieri))]
        [Route("All")]
        public IHttpActionResult GetAll([FromUri] string codiceCantiere, [FromUri] string descrizione)
        {
            try
            {
                List<Cantieri> items = CantieriDAO.GetCantieri(codiceCantiere, descrizione);
                return Ok(items);
            }
            catch (Exception ex)
            {
                string messaggio = $"Errore durante la GetAll in CantieriController --- {ex}";
                log.Error(messaggio);
                return BadRequest(messaggio);
            }
        }
    }
}