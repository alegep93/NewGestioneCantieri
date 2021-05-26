using Database.DAO;
using Database.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace RestApi.Controllers
{
    [RoutePrefix("api/Operai")]
    public class OperaiController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger("FileLog");

        [HttpGet]
        [ResponseType(typeof(Operai))]
        [Route("All")]
        public IHttpActionResult GetAll([FromUri] string nome, [FromUri] string descrizione)
        {
            try
            {
                List<Operai> items = OperaiDAO.GetAll(nome, descrizione);
                return Ok(items);
            }
            catch (Exception ex)
            {
                string messaggio = $"Errore durante la GetAll in OperaiController --- {ex}";
                log.Error(messaggio);
                return BadRequest(messaggio);
            }
        }
    }
}