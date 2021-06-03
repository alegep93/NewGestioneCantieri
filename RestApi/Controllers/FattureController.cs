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
    [RoutePrefix("api/Fatture")]
    public class FattureController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger("FileLog");

        [HttpGet]
        [ResponseType(typeof(List<Fattura>))]
        [Route("All/Emesse")]
        public IHttpActionResult GetFattureEmesse([FromUri] string year, [FromUri] string numFattura)
        {
            try
            {
                List<Fattura> items = FattureDAO.GetByAnnoNumero(Convert.ToInt32(year), Convert.ToInt32(numFattura));
                return Ok(items);
            }
            catch (Exception ex)
            {
                string messaggio = $"Errore durante la GetFattureEmesse in NotesController --- {ex}";
                log.Error(messaggio);
                return BadRequest(messaggio);
            }
        }

        [HttpGet]
        [ResponseType(typeof(List<Fattura>))]
        [Route("All/Acquisto")]
        public IHttpActionResult GetFattureAcquisto([FromUri] string year, [FromUri] string numFattura)
        {
            try
            {
                List<FatturaAcquisto> items = FattureAcquistoDAO.GetByAnnoNumero(Convert.ToInt32(year), Convert.ToInt32(numFattura));
                return Ok(items);
            }
            catch (Exception ex)
            {
                string messaggio = $"Errore durante la GetFattureEmesse in NotesController --- {ex}";
                log.Error(messaggio);
                return BadRequest(messaggio);
            }
        }
    }
}