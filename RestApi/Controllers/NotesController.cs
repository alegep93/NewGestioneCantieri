using System;
using System.Web.Http;
using System.Web.Http.Description;
using Database.Models;
using Database.DAO;
using System.Collections.Generic;
using log4net;

namespace RestApi.Controllers
{
    [RoutePrefix("api/Notes")]
    public class NotesController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger("FileLog");

        [HttpGet]
        [ResponseType(typeof(List<Nota>))]
        [Route("All")]
        public IHttpActionResult GetAll()
        {
            try
            {
                List<Nota> items = NoteDAO.GetAll();
                return Ok(items);
            }
            catch (Exception ex)
            {
                string messaggio = $"Errore durante la GetAll in NotesController --- {ex}";
                log.Error(messaggio);
                return BadRequest(messaggio);
            }
        }

        [HttpPost]
        [ResponseType(typeof(long))]
        [Route("Insert")]
        public IHttpActionResult Insert([FromBody] string nota)
        {
            try
            {
                long idNota = NoteDAO.Insert(nota);
                return Ok(idNota);
            }
            catch (Exception ex)
            {
                string messaggio = $"Errore durante la Insert in NotesController --- {ex}";
                log.Error(messaggio);
                return BadRequest(messaggio);
            }
        }

        [HttpDelete]
        [ResponseType(typeof(bool))]
        [Route("Delete")]
        public IHttpActionResult Delete([FromBody] string idNota)
        {
            try
            {
                NoteDAO.Delete(Convert.ToInt64(idNota));
                return Ok(true);
            }
            catch (Exception ex)
            {
                string messaggio = $"Errore durante la Delete in NotesController --- {ex}";
                log.Error(messaggio);
                return BadRequest(messaggio);
            }
        }
    }
}