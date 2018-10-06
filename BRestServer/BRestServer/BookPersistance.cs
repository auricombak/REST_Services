using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BRestServer.Models;
using MySql.Data;
using System.Collections;

namespace BRestServer
{
    public class BookPersistance
    {
        public MySql.Data.MySqlClient.MySqlConnection conn;
        public BookPersistance()
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
        public long saveBook(Book book)
        {
            String sqlStoring = "INSERT INTO BooksTable ( isbn ,titre ,editeur ,auteur ,nb_exemplaires ) VALUES ('" + book.Isbn + "','" + book.Title + "','" + book.Editor
                + "','" + book.Author + "','" + book.NbrExemplaires + "');";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlStoring, conn);
            cmd.ExecuteNonQuery();
            long id = cmd.LastInsertedId;
            return id;
        }

        public Book getBookByIsbn(int isbn)
        {
            Book b = new Book();
            MySql.Data.MySqlClient.MySqlDataReader mySqlReader = null;

            String sqlString = "SELECT * FROM booksTable WHERE isbn = " + isbn.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

            mySqlReader = cmd.ExecuteReader();

            if (mySqlReader.Read())
            {
                b.ID = mySqlReader.GetInt32(0);
                b.Isbn = mySqlReader.GetInt32(1);
                b.Title = mySqlReader.GetString(2);
                b.Editor = mySqlReader.GetString(3);
                b.Author = mySqlReader.GetString(4);
                b.NbrExemplaires = mySqlReader.GetInt32(5);
                return b;
            }
            else
            {
                return null;
            }
        }


        public ArrayList getBookByAuteur(string auteur)
        {

            MySql.Data.MySqlClient.MySqlDataReader mySqlReader = null;

            String sqlString = "SELECT * FROM booksTable WHERE auteur = '" + auteur.ToString()+"'";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);
            ArrayList books = new ArrayList();
            mySqlReader = cmd.ExecuteReader();

            while (mySqlReader.Read())
            {
                Book b = new Book();
                b.ID = mySqlReader.GetInt32(0);
                b.Isbn = mySqlReader.GetInt32(1);
                b.Title = mySqlReader.GetString(2);
                b.Editor = mySqlReader.GetString(3);
                b.Author = mySqlReader.GetString(4);
                b.NbrExemplaires = mySqlReader.GetInt32(5);
                books.Add(b);
            }
            return books;
            
        }

        public ArrayList getAll()
        {

            MySql.Data.MySqlClient.MySqlDataReader mySqlReader = null;

            String sqlString = "SELECT * FROM booksTable";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);
            ArrayList books = new ArrayList();
            mySqlReader = cmd.ExecuteReader();

            while (mySqlReader.Read())
            {
                Book b = new Book();
                b.ID = mySqlReader.GetInt32(0);
                b.Isbn = mySqlReader.GetInt32(1);
                b.Title = mySqlReader.GetString(2);
                b.Editor = mySqlReader.GetString(3);
                b.Author = mySqlReader.GetString(4);
                b.NbrExemplaires = mySqlReader.GetInt32(5);
                books.Add(b);
            }
            return books;

        }

        public Boolean updateBook(Book book, int isbn)
        {
            MySql.Data.MySqlClient.MySqlDataReader mySqlReader = null;
            Book b = new Book();

            String sqlString = "SELECT * FROM booksTable WHERE isbn = " + isbn.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);
            mySqlReader = cmd.ExecuteReader();
            if (mySqlReader.Read())
            {
                mySqlReader.Close();
                String sqlStoring = "UPDATE BooksTable SET ( isbn = " + book.Isbn + ",titre = '" + book.Title + "',editeur ='"+ book.Editor
                + "',auteur = '" + book.Author +"' ,nb_exemplaires = "+ book.NbrExemplaires + "+) WHERE isbn = "+book.Isbn ;


                cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

                cmd.ExecuteNonQuery();
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean deleteBook(int isbn)
        {
            MySql.Data.MySqlClient.MySqlDataReader mySqlReader = null;
            Book b = new Book();

            String sqlString = "SELECT * FROM booksTable WHERE isbn = " + isbn.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);
            mySqlReader = cmd.ExecuteReader();
            if (mySqlReader.Read())
            {
                mySqlReader.Close();
                sqlString = "DELETE FROM booksTable WHERE isbn = " + isbn.ToString();
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