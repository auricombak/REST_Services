using BRestServer.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BRestServer.Controllers
{
    public class CommentairesController : ApiController
    {

        /// <summary>
        /// Récupère les commentaires d'un livre par son isbn
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns></returns>
        [HttpGet]
        // GET: api/Commentaires/?isbn=isbn
        public ArrayList getCommentaires(int isbn)
        {
            CommentairePersistance cp = new CommentairePersistance();
            return cp.getCommentaires(isbn);
        }

        /// <summary>
        /// Ajoute un commentaire après vérification du password donnée dans l'url
        /// </summary>
        /// <param name="value"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        // Post: api/Commentaires/password    
        [HttpPost]
        [Route("api/Commentaires/{password}")]
        public HttpResponseMessage Post([FromBody]Commentaire value, string password)
        {
            AbonnePersistance ap = new AbonnePersistance();
            if (ap.GetAbonne(value.Id_abonne, password)) {
                CommentairePersistance cp = new CommentairePersistance();
                cp.saveCommentaire(value);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                return response;
            }
            else
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Forbidden);
                return response;
            }           
        }

        /// <summary>
        /// Supprime le commentaire selon son id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Commentaires/id
        public HttpResponseMessage Delete(int id)
        {
            CommentairePersistance bp = new CommentairePersistance();
            Boolean recordExisted = false;
            recordExisted = bp.deleteCommentaire(id);

            HttpResponseMessage response;

            if (recordExisted)
            {
                response = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return response;
        }

    }
}
