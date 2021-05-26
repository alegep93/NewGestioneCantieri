using System;
using System.Web.Http;
using System.Web.Http.Description;
using Database.Models;
using Database.DAO;
using System.Collections.Generic;
using log4net;

namespace RestApi.Controllers
{
    [RoutePrefix("api/Preventivi")]
    public class PreventiviController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger("FileLog");

        [HttpGet]
        [ResponseType(typeof(List<Nota>))]
        [Route("All")]
        public IHttpActionResult GetPreventivi([FromUri] string year, [FromUri] string description)
        {
            try
            {
                List<Preventivo> items = PreventiviDAO.GetPreventivi(year, description);
                return Ok(items);
            }
            catch (Exception ex)
            {
                string messaggio = $"Errore durante la GetPreventivi in NotesController --- {ex}";
                log.Error(messaggio);
                return BadRequest(messaggio);
            }
        }
    }
}