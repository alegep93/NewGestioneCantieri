using Database.DAO;
using Database.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

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

        [HttpGet]
        [ResponseType(typeof(List<DDTMef>))]
        [Route("Codes")]
        public IHttpActionResult GetDdtMefCodes([FromUri] string annoInizio, [FromUri] string annoFine)
        {
            try
            {
                List<DDTMef> items = DDTMefDAO.GetCodiciMef(Convert.ToInt32(annoInizio), Convert.ToInt32(annoFine));
                return Ok(items);
            }
            catch (Exception ex)
            {
                string messaggio = $"Errore durante la GetDdtMefCodes in DdtMefController --- {ex}";
                log.Error(messaggio);
                return BadRequest(messaggio);
            }
        }
    }
}