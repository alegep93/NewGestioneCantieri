using Database.DAO;
using Database.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace RestApi.Controllers
{
    [RoutePrefix("api/Utenti")]
    public class UtentiController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger("FileLog");

        [HttpGet]
        [ResponseType(typeof(Utente))]
        [Route("Login")]
        public IHttpActionResult Login([FromUri] string username, [FromUri] string password)
        {
            try
            {
                Utente items = UtentiDAO.Login(username, password);
                return Ok(items);
            }
            catch (Exception ex)
            {
                string messaggio = $"Errore durante la Login in UtentiController --- {ex}";
                log.Error(messaggio);
                return BadRequest(messaggio);
            }
        }
    }
}