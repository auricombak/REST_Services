using BRestServer.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BRestServer
{
    public class CommentairePersistance
    {
        public MySql.Data.MySqlClient.MySqlConnection conn;
        public CommentairePersistance()
        {
            string myConnectionString;
            myConnectionString = "Server=127.0.0.1;uid=root;pwd=edrf34TER;database=bibliobd";
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                throw ex;
            }
        }
        public void saveCommentaire(Commentaire commentaire)
        {
            
            String sqlStoring = "INSERT INTO CommentairesTable ( isbn , id_abonne, content ) VALUES ('" + commentaire.Isbn + "','" + commentaire.Id_abonne + "','" + commentaire.Content + "');";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlStoring, conn);
            cmd.ExecuteNonQuery();

        }

        public ArrayList getCommentaires(int isbn)
        {

            MySql.Data.MySqlClient.MySqlDataReader mySqlReader = null;

            String sqlString = "SELECT * FROM commentairesTable WHERE isbn = " + isbn.ToString() ;
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);
            ArrayList commentaires = new ArrayList();
            mySqlReader = cmd.ExecuteReader();

            while (mySqlReader.Read())
            {
                Commentaire c = new Commentaire();
                c.ID = mySqlReader.GetInt32(0);
                c.Isbn = mySqlReader.GetInt32(1);
                c.Id_abonne = mySqlReader.GetInt32(2);
                c.Content = mySqlReader.GetString(3);
                commentaires.Add(c);
            }
            return commentaires;
        }


        public Boolean deleteCommentaire( int id )
        {
            MySql.Data.MySqlClient.MySqlDataReader mySqlReader = null;
            Commentaire b = new Commentaire();

            String sqlString = "SELECT * FROM commentaireTable WHERE ID = " + id.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);
            mySqlReader = cmd.ExecuteReader();
            if (mySqlReader.Read())
            {
                mySqlReader.Close();
                sqlString = "DELETE FROM commentaireTable WHERE ID = " + id.ToString();
                cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

                cmd.ExecuteNonQuery();
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}