using BRestServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections;

namespace BRestServer.Controllers
{
    public class BooksController : ApiController
    {


        /// <summary>
        /// Réccupère tout les livres
        /// </summary>
        /// <returns></returns>
        // GET: api/Books
        public ArrayList Get()
        {
            BookPersistance bp = new BookPersistance();
            return bp.getAll();

        }

        /// <summary>
        /// Reccupère le livre par son isbn
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns></returns>
        [HttpGet]
        // GET: api/Books/?isbn=isbn
        public Book GetByIsbn(int isbn)
        {
            BookPersistance bp = new BookPersistance();
            Book b = bp.getBookByIsbn(isbn);

            //Appeller une fonction qui prendra en id un Isbn ou un titre
            return b ;
        }

        /// <summary>
        /// Récupère les livres de l'auteur
        /// </summary>
        /// <param name="auteur"></param>
        /// <returns></returns>
        [HttpGet]
        // GET: api/Books?auteur=auteur
        public ArrayList GetByAuteur(string auteur)
        {
            BookPersistance bp = new BookPersistance();
            return bp.getBookByAuteur(auteur);

        }


        /// <summary>
        /// Ajoute un livre
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        // POST: api/Books
        public HttpResponseMessage Post([FromBody]Book value)
        {
            BookPersistance bp = new BookPersistance();
            long id;
            id = bp.saveBook(value);
            value.ID = id;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            return response;
        }

        /// <summary>
        /// Modifie le livre donné par son isbn
        /// </summary>
        /// <param name="isbn"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        // PUT: api/Books/5
        public HttpResponseMessage Put(int isbn, [FromBody]Book book)
        {
            BookPersistance bp = new BookPersistance();
            Boolean recordExisted = false;
            recordExisted = bp.updateBook(book , isbn);

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

        /// <summary>
        /// Suprime un Livre par son Isbn
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns></returns>
        // DELETE: api/Books/?isbn=isbn
        public HttpResponseMessage Delete(int isbn)
        {
            BookPersistance bp = new BookPersistance();
            Boolean recordExisted = false;
            recordExisted = bp.deleteBook( isbn);

            HttpResponseMessage response ;

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
