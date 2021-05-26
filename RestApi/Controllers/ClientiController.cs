using Database.DAO;
using Database.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace RestApi.Controllers
{
    [RoutePrefix("api/Clienti")]
    public class ClientiController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger("FileLog");

        [HttpGet]
        [ResponseType(typeof(Clienti))]
        [Route("All")]
        public IHttpActionResult GetAll([FromUri] string ragioneSociale)
        {
            try
            {
                List<Clienti> items = ClientiDAO.GetClienti(ragioneSociale);
                return Ok(items);
            }
            catch (Exception ex)
            {
                string messaggio = $"Errore durante la GetAll in ClientiController --- {ex}";
                log.Error(messaggio);
                return BadRequest(messaggio);
            }
        }
    }
}