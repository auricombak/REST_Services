using BRestServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BRestServer
{
    public class AbonnePersistance
    {

        public MySql.Data.MySqlClient.MySqlConnection conn;
        public AbonnePersistance()
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

        public long SaveAbonne(Abonne abo)
        {
            String sqlStoring = "INSERT INTO abonnesTable ( identifiant, password ) VALUES ('" + abo.Identifiant + "','" + abo.Password +"');";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlStoring, conn);
            cmd.ExecuteNonQuery();
            long id = cmd.LastInsertedId;
            return id;
        }

        public Boolean GetAbonne(int identifiant, string password)
        {
            Book b = new Book();
            MySql.Data.MySqlClient.MySqlDataReader mySqlReader = null;

            String sqlString = "SELECT * FROM abonnesTable WHERE identifiant = " + identifiant.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);
            mySqlReader = cmd.ExecuteReader();

            if (mySqlReader.Read())
            {
                mySqlReader.Close();
                sqlString = "SELECT * FROM abonnesTable WHERE identifiant = " + identifiant.ToString() + " AND password = '" + password.ToString()+"'"; 
                cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);
                mySqlReader = cmd.ExecuteReader();

                if(mySqlReader.Read())
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        public Boolean ExistAbonne(int identifiant)
        {
            Book b = new Book();
            MySql.Data.MySqlClient.MySqlDataReader mySqlReader = null;

            String sqlString = "SELECT * FROM abonnesTable WHERE identifiant = " + identifiant.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);
            mySqlReader = cmd.ExecuteReader();

            if (mySqlReader.Read())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}