using System;
using System.Web.Http;
using System.Web.Http.Description;
using Database.Models;
using Database.DAO;
using System.Collections.Generic;
using log4net;

namespace RestApi.Controllers
{
    [RoutePrefix("api/Amministratori")]
    public class AmministratoriController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger("FileLog");

        [HttpGet]
        [ResponseType(typeof(List<Amministratore>))]
        [Route("All")]
        public IHttpActionResult GetAll()
        {
            try
            {
                List<Amministratore> items = AmministratoriDAO.GetAll();
                return Ok(items);
            }
            catch (Exception ex)
            {
                string messaggio = $"Errore durante la GetAll in AmministratoriController --- {ex}";
                log.Error(messaggio);
                return BadRequest(messaggio);
            }
        }
    }
}