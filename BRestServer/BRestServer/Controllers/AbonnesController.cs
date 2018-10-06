using BRestServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BRestServer.Controllers
{
    public class AbonnesController : ApiController
    {

        /// <summary>
        /// Renvoie vrai si l'identifiant et le mot de passe sont valides et concordent
        /// </summary>
        /// <param name="identifiant"></param>
        /// <param name="password"></param>
        /// <returns></returns>

        // GET: api/Abonne/"identifiant"/"password"
        [Route("api/Abonnes/{identifiant}/{password}")]
        [HttpGet]
        public Boolean Get(int identifiant, string password)
        {
            AbonnePersistance ap = new AbonnePersistance();
            return ap.GetAbonne(identifiant, password);
        }

        /// <summary>
        /// Ajoute un abonne
        /// </summary>
        /// <param name="abonne"></param>
        /// <returns></returns>
        // POST: api/Abonne
        public long Post([FromBody] Abonne abonne)
        {
            AbonnePersistance ap = new AbonnePersistance();
            long res = ap.SaveAbonne(abonne);
            return res;
        }

    }
}
