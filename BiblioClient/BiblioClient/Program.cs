using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections;
using Newtonsoft.Json;

namespace BiblioClient
{
    class Program
    {

        static void Main(string[] args)
        {
            RestClient rc = new RestClient();
            int choix = 0;
            int identifiant = 0;


            while (true)
            {

                Console.WriteLine("--- Menu bibliotheque ---");
                if (identifiant == 0)
                {
                    Console.WriteLine("Inscription                     : 0");
                    Console.WriteLine("Connection                      : 1");
                }
                else
                {
                    Console.WriteLine(" --------- Identifiant : " + identifiant + "-----------");
                }
                Console.WriteLine("Consultation des livres         : 2");
                Console.WriteLine("Recherche de livre par Isbn     : 3");
                Console.WriteLine("Recherche de livre par Auteur   : 4");
                Console.WriteLine("Commenter un livre par son Isbn : 5");
                Console.WriteLine("Gestion du catalogue (Admin)    : 6");
                Console.WriteLine("Gestion des abonnes  (Admin)    : 7");
                Console.WriteLine("Sortie                          : <Entré>");

                Console.WriteLine("Choix : ");


                choix = Convert.ToInt32(Console.ReadLine());


                switch (choix)
                {
                    case 0:
                        identifiant = inscription(rc);
                        break;

                    case 1:
                        identifiant = connection(rc);
                        break;

                    case 2:
                        consultation(rc);
                        break;

                    case 3:
                        rechercheParIsbn(rc);
                        break;

                    case 4:
                        rechercheParAuteur(rc);
                        break;

                    case 5:
                        if (identifiant != 0) {
                            commenterLivreParIsbn(rc, identifiant);
                        }
                        else
                        {
                            identifiant=connection(rc);
                        }

                        break;

                    default:
                        break;
                }

            }
        }

        static int inscription(RestClient restClient)
        {
            Random rnd = new Random();
            int identifiant = rnd.Next(10000, 99999);

            Console.Clear();
            Console.WriteLine("____________________________________________");
            Console.WriteLine("_______________| Inscription |______________");
            Console.WriteLine("");
            Console.WriteLine("Votre identifiant est le :  " + identifiant.ToString() + ": Notez le bien ");
            Console.WriteLine("____________________________________________");
            Console.WriteLine("{Par défaut votre mot de passe est : admin :} ");
            Console.WriteLine("____________________________________________");
            Console.WriteLine("Choisissez votre mot de passe :");
            String mdp = Console.ReadLine(); ;
            if (mdp == null || mdp == String.Empty)
            {
                mdp = "admin";
            }
            string postAbonne = "{\"Identifiant\":\"" + identifiant + "\", \"Password\":\"" + mdp + "\"}";

            restClient.EndPoint = "http://localhost:50502/api/Abonnes";
            restClient.httpVerb = HttpVerb.POST;

            string response = String.Empty;
            restClient.makeHttpRequest(postAbonne);
            Console.Clear();
            return identifiant;

        }

        static int connection(RestClient restClient)
        {
            int identifiant;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("____________________________________________");
                Console.WriteLine("_______________| Connection |______________");
                Console.WriteLine("");
                Console.WriteLine("Entrez votre identifiant  :  ");
                try
                {
                    identifiant = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch
                {
                    Console.WriteLine("Un identifiant est composé de chiffres ");
                }
            }
            while (true)
            {
                Console.WriteLine("____________________________________________");
                Console.WriteLine("Entrez votre mot de passe  :  ");

                String mdp = Console.ReadLine();
                Console.WriteLine("____________________________________________");

                if (mdp == null || mdp == String.Empty)
                {
                    Console.Clear();
                    Console.WriteLine("Le mot de passe n'a pas été saisi ! ");
                }
                else
                {

                    restClient.EndPoint = "http://localhost:50502/api/Abonnes/" + identifiant + "/" + mdp;
                    restClient.httpVerb = HttpVerb.GET;

                    Boolean auth = Convert.ToBoolean(restClient.makeHttpRequest(null));

                    if (auth)
                    {
                        Console.Clear();
                        return identifiant;
                    }
                    else
                    {
                        Console.Clear();
                        return 0;
                    }
                }
            }

        }

        static void consultation(RestClient restClient)
        {


            Console.Clear();
            Console.WriteLine("____________________________________________");
            Console.WriteLine("______________| Consultation |______________");

            restClient.EndPoint = "http://localhost:50502/api/Books";
            restClient.httpVerb = HttpVerb.GET;

            string response = String.Empty;
            response = restClient.makeHttpRequest(null);
            List<Book> books = JsonConvert.DeserializeObject<List<Book>>(response);

            foreach (Book b in books)
            {
                Console.WriteLine("____________________________________________");
                Console.WriteLine("Titre :  " + b.Title);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Auteur :  " + b.Author);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Editeur :  " + b.Editor);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Isbn :  " + b.Isbn);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Nombre d'exemplaires :  " + b.NbrExemplaires);
                Console.WriteLine("____________________________________________");

                restClient.EndPoint = "http://localhost:50502/api/Commentaires/?isbn=" + b.Isbn.ToString();
                restClient.httpVerb = HttpVerb.GET;

                string response2 = String.Empty;
                response2 = restClient.makeHttpRequest(null);
                List<Commentaire> commentaires = JsonConvert.DeserializeObject<List<Commentaire>>(response2);
                if (commentaires.Any())
                {
                    int i = 0;
                    foreach (Commentaire c in commentaires)
                    {
                        i++;
                        Console.WriteLine();
                        Console.WriteLine("[Commentaire N : " + i + "]");
                        Console.WriteLine("____________________________________________");
                        Console.WriteLine("Id auteur commentaire  :  " + c.Id_abonne);
                        Console.WriteLine("--------------------------------------------");
                        Console.WriteLine("Auteur :  " + c.Content);
                        Console.WriteLine("____________________________________________");

                    }

                }
                Console.WriteLine("*****************************************************************");
                Console.WriteLine();
                Console.WriteLine();
            }
            Console.ReadLine();
            Console.Clear();

        }

        static void rechercheParIsbn(RestClient restClient)
        {

            int isbn;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("____________________________________________");
                Console.WriteLine("___________| Recherche par Isbn |___________");
                Console.WriteLine("");
                Console.WriteLine("Indiquez l'ISBN du livre souhaité :");
                try
                {
                    isbn = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch
                {
                    Console.WriteLine("Un identifiant est composé de chiffres ");
                }
            }

            restClient.EndPoint = "http://localhost:50502/api/Books/?isbn=" + isbn.ToString();
            restClient.httpVerb = HttpVerb.GET;
            string response = String.Empty;
            response = restClient.makeHttpRequest(null);
            Book b = JsonConvert.DeserializeObject<Book>(response);
            try
            {
                Console.WriteLine("____________________________________________");
                Console.WriteLine("Titre :  " + b.Title);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Auteur :  " + b.Author);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Editeur :  " + b.Editor);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Isbn :  " + b.Isbn);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Nombre d'exemplaires :  " + b.NbrExemplaires);
                Console.WriteLine("____________________________________________");

                restClient.EndPoint = "http://localhost:50502/api/Commentaires/?isbn=" + b.Isbn.ToString();
                restClient.httpVerb = HttpVerb.GET;

                string response2 = String.Empty;
                response2 = restClient.makeHttpRequest(null);
                List<Commentaire> commentaires = JsonConvert.DeserializeObject<List<Commentaire>>(response2);
                if (commentaires.Any())
                {
                    int i = 0;
                    foreach (Commentaire c in commentaires)
                    {
                        i++;
                        Console.WriteLine();
                        Console.WriteLine("[Commentaire N : " + i + "]");
                        Console.WriteLine("____________________________________________");
                        Console.WriteLine("Id auteur commentaire  :  " + c.Id_abonne);
                        Console.WriteLine("--------------------------------------------");
                        Console.WriteLine("Auteur :  " + c.Content);
                        Console.WriteLine("____________________________________________");

                    }

                }
                Console.WriteLine("*****************************************************************");
            }
            catch
            {
                Console.WriteLine("L'isbn rentré ne correspond à aucuns livres ..");
            }
            Console.ReadLine();
            Console.Clear();

        }

        static void rechercheParAuteur(RestClient restClient)
        {

            string auteur;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("____________________________________________");
                Console.WriteLine("__________| Recherche par Auteur |__________");
                Console.WriteLine("");
                Console.WriteLine("Entrez le nom de l'auteur recherché");
                auteur = Console.ReadLine();

                if (auteur == null || auteur == String.Empty)
                {
                    Console.Clear();
                    return;
                }
                break;

            }

            restClient.EndPoint = "http://localhost:50502/api/Books/?auteur=" + auteur;
            restClient.httpVerb = HttpVerb.GET;
            string response = String.Empty;
            response = restClient.makeHttpRequest(null);
            List<Book> books = JsonConvert.DeserializeObject<List<Book>>(response);

            foreach (Book b in books) {
                Console.WriteLine("____________________________________________");
                Console.WriteLine("Titre :  " + b.Title);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Auteur :  " + b.Author);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Editeur :  " + b.Editor);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Isbn :  " + b.Isbn);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Nombre d'exemplaires :  " + b.NbrExemplaires);
                Console.WriteLine("____________________________________________");

                restClient.EndPoint = "http://localhost:50502/api/Commentaires/?isbn=" + b.Isbn.ToString();
                restClient.httpVerb = HttpVerb.GET;

                string response2 = String.Empty;
                response2 = restClient.makeHttpRequest(null);
                List<Commentaire> commentaires = JsonConvert.DeserializeObject<List<Commentaire>>(response2);
                if (commentaires.Any())
                {
                    int i = 0;
                    foreach (Commentaire c in commentaires)
                    {
                        i++;
                        Console.WriteLine();
                        Console.WriteLine("[Commentaire N : " + i + "]");
                        Console.WriteLine("____________________________________________");
                        Console.WriteLine("Id auteur commentaire  :  " + c.Id_abonne);
                        Console.WriteLine("--------------------------------------------");
                        Console.WriteLine("Auteur :  " + c.Content);
                        Console.WriteLine("____________________________________________");

                    }

                }
                Console.WriteLine("*****************************************************************");
            }
            if (!books.Any())
            {
                Console.WriteLine("L'auteur rentré n'est pas dans la base de donnée ..");
            }

            Console.ReadLine();
            Console.Clear();

        
        }

        static void commenterLivreParIsbn(RestClient restClient, int identifiant)
        {

            int isbn;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("____________________________________________");
                Console.WriteLine("___________| Recherche par Isbn |___________");
                Console.WriteLine("");
                Console.WriteLine("Indiquez l'ISBN du livre souhaité :");
                try
                {
                    isbn = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch
                {
                    Console.WriteLine("Un identifiant est composé de chiffres ");
                }
            }

            restClient.EndPoint = "http://localhost:50502/api/Books/?isbn=" + isbn.ToString();
            restClient.httpVerb = HttpVerb.GET;
            string response = String.Empty;
            response = restClient.makeHttpRequest(null);
            Book b = JsonConvert.DeserializeObject<Book>(response);
            try
            {
                Console.WriteLine("____________________________________________");
                Console.WriteLine("Titre :  " + b.Title);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Auteur :  " + b.Author);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Editeur :  " + b.Editor);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Isbn :  " + b.Isbn);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Nombre d'exemplaires :  " + b.NbrExemplaires);
                Console.WriteLine("____________________________________________");

                restClient.EndPoint = "http://localhost:50502/api/Commentaires/?isbn=" + b.Isbn.ToString();
                restClient.httpVerb = HttpVerb.GET;

                string response2 = String.Empty;
                response2 = restClient.makeHttpRequest(null);
                List<Commentaire> commentaires = JsonConvert.DeserializeObject<List<Commentaire>>(response2);
                if (commentaires.Any())
                {
                    int i = 0;
                    foreach (Commentaire c in commentaires)
                    {
                        i++;
                        Console.WriteLine();
                        Console.WriteLine("[Commentaire N : " + i + "]");
                        Console.WriteLine("____________________________________________");
                        Console.WriteLine("Id auteur commentaire  :  " + c.Id_abonne);
                        Console.WriteLine("--------------------------------------------");
                        Console.WriteLine("Auteur :  " + c.Content);
                        Console.WriteLine("____________________________________________");

                    }

                }
                Console.WriteLine("*****************************************************************");
            }
            catch
            {
                Console.WriteLine("L'isbn rentré ne correspond à aucuns livres ..");
                Console.ReadLine();
                Console.Clear();
                return;
            }
            Console.WriteLine();

            Console.WriteLine("____________________________________________");
            Console.WriteLine("_________ Ajout de commentaire _____________");
            Console.WriteLine("Entrez votre mot de passe : ");
            string mdp = Console.ReadLine();
            if (mdp == null || mdp == String.Empty)
            {
                return;
            }
            string content;
            while (true)
            {
                Console.WriteLine("Entrez le contenu de votre message : ");
                content = Console.ReadLine();
                if (content != null && content != String.Empty)
                {
                    break;
                }
            }
            //JsonConvert.SerializeObject
            string postAbonne = "{\"Isbn\":\"" + isbn.ToString() + "\", \"Id_abonne\":\"" + identifiant.ToString() + "\", \"Content\":\"" + content + "\"}";
            restClient.EndPoint = "http://localhost:50502/api/Commentaires/"+mdp;
            restClient.httpVerb = HttpVerb.POST;
            Console.WriteLine(postAbonne);
            Console.ReadLine();
            string response3 = String.Empty;

            response3 = restClient.makeHttpRequest(postAbonne);



            Console.WriteLine("Le commentaire a bien été crée ! " + response3);
            Console.ReadLine();
            Console.Clear();
        }


    }
}
