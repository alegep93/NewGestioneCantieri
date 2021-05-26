using Database.DAO;
using Database.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace RestApi.Controllers
{
    [RoutePrefix("api/Spese")]
    public class SpeseController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger("FileLog");

        [HttpGet]
        [ResponseType(typeof(Spese))]
        [Route("All")]
        public IHttpActionResult GetAll([FromUri] string descrizione)
        {
            try
            {
                List<Spese> items = SpeseDAO.GetByDescription(descrizione);
                return Ok(items);
            }
            catch (Exception ex)
            {
                string messaggio = $"Errore durante la GetAll in SpeseController --- {ex}";
                log.Error(messaggio);
                return BadRequest(messaggio);
            }
        }
    }
}