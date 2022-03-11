using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MandACONSOLE
{
    class SQLQuerys
    {
        //QUERY List

        //Inizialize and creating of DB and Tables

        public string InsertAnimePlanet = "INSERT INTO AnimePlanet (Title, Type, Producer, Release_, Rating, Description, Tags, Cover, LastScan) " +
                                    "VALUES (@Title, @Type, @Producer, @Release, @Rating, @Description, @Tags, @Cover, (SELECT CURRENT_TIMESTAMP AS 'CURRENT_TIMESTAMP'));";

        public string InsertMangaPlanet = "INSERT INTO MangaPlanet (Title, Type, Producer, Release_, Rating, Description, Tags, Cover, LastScan) " +
                                    "VALUES (@Title, @Type, @Producer, @Release, @Rating, @Description, @Tags, @Cover, (SELECT CURRENT_TIMESTAMP AS 'CURRENT_TIMESTAMP'));";

        public string InsertNovelPlanet = "INSERT INTO NovelPlanet (Title, Type, Producer, Release_, Rating, Description, Tags, Cover, LastScan) " +
                                    "VALUES (@Title, @Type, @Producer, @Release, @Rating, @Description, @Tags, @Cover, (SELECT CURRENT_TIMESTAMP AS 'CURRENT_TIMESTAMP'));";

        public void Createdbifnotexist()
        {
            string createDb = "CREATE DATABASE IF NOT EXISTS MangaAnimeDbV1;";
            string createTable = "CREATE TABLE IF NOT EXISTS AnimePlanet (id int AUTO_INCREMENT PRIMARY KEY," +
                                    "samevalue int," +
                                    "Title varchar(60)," +
                                    "Type varchar(30)," +
                                    "Producer varchar(40)," +
                                    "Release_ varchar(25)," +
                                    "Rating varchar(4)," +
                                    "Description TEXT," +
                                    "Tags TEXT," +
                                    "Cover BLOB," +
                                    "Valid int," +
                                    "LastScan date" +
                                    ");";
            string createTable2 = "CREATE TABLE IF NOT EXISTS MangaPlanet (id int AUTO_INCREMENT PRIMARY KEY," +
                                    "samevalue int," +
                                    "Title varchar(60)," +
                                    "Type varchar(30)," +
                                    "Producer varchar(40)," +
                                    "Release_ varchar(25)," +
                                    "Rating varchar(4)," +
                                    "Description TEXT," +
                                    "Tags TEXT," +
                                    "Cover BLOB," +
                                    "Valid int," +
                                    "LastScan date" +
                                    ");";
            string createTable3 = "CREATE TABLE IF NOT EXISTS NovelPlanet (id int AUTO_INCREMENT PRIMARY KEY," +
                                    "samevalue int," +
                                    "Title varchar(60)," +
                                    "Type varchar(30)," +
                                    "Producer varchar(40)," +
                                    "Release_ varchar(25)," +
                                    "Rating varchar(4)," +
                                    "Description TEXT," +
                                    "Tags TEXT," +
                                    "Cover BLOB," +
                                    "Valid int," +
                                    "LastScan date" +
                                    ");";

            SQLHandler SQLhandler = new SQLHandler();
            MySqlConnection conn = SQLhandler.SQLConnection();
            MySqlCommand sqlCmd = new MySqlCommand(createDb, conn);
            sqlCmd.ExecuteReader();

            SQLHandler.Database = "MangaAnimeDbV1";
            conn = SQLhandler.SQLConnection();
            sqlCmd = new MySqlCommand(createTable, conn);
            sqlCmd.ExecuteReader();
            conn.Close();

            SQLHandler.Database = "MangaAnimeDbV1";
            conn = SQLhandler.SQLConnection();
            sqlCmd = new MySqlCommand(createTable2, conn);
            sqlCmd.ExecuteReader();
            conn.Close();

            SQLHandler.Database = "MangaAnimeDbV1";
            conn = SQLhandler.SQLConnection();
            sqlCmd = new MySqlCommand(createTable3, conn);
            sqlCmd.ExecuteReader();
            conn.Close();



        }
        public bool ExistingItemCheck(MySqlConnection conn, string TitleName)
        {
            string existingItemCheck = "SELECT @collumnName FROM AnimePlanet WHERE AnimePlanet.Title = @collumnName;";
            SQLHandler SQLhandler = new SQLHandler();
            conn = SQLhandler.SQLConnection();
            MySqlCommand sqlCmd = new MySqlCommand(existingItemCheck, conn);
            sqlCmd.Parameters.Add("@collumnName", MySqlDbType.VarChar).Value = TitleName;
            using MySqlDataReader reader = sqlCmd.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine("Item existing!");
                return reader.Read();
            }
            conn.Close();
            return false;

        }
    }
}
