using System;
using System.Web.Http;
using System.Web.Http.Description;
using Database.Models;
using Database.DAO;
using System.Collections.Generic;
using log4net;
using System.Linq;

namespace RestApi.Controllers
{
    [RoutePrefix("api/Bollette")]
    public class BolletteController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger("FileLog");

        [HttpGet]
        [ResponseType(typeof(List<Bolletta>))]
        [Route("All")]
        public IHttpActionResult GetBollette([FromUri] string year, [FromUri] string ragSocFornitore)
        {
            try
            {
                List<Bolletta> items = BolletteDAO.GetAll(Convert.ToInt32(year), ragSocFornitore);
                return Ok(items);
            }
            catch (Exception ex)
            {
                string messaggio = $"Errore durante la GetBollette in BolletteController --- {ex}";
                log.Error(messaggio);
                return BadRequest(messaggio);
            }
        }
    }
}