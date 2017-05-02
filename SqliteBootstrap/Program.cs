using SqliteBootstrap.DbModels;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqliteBootstrap
{
    class Program
    {
        static void Main(string[] args)
        {
            String folder = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            String fileName = Path.Combine(folder, "badges.db");
            if (!File.Exists(fileName))
            {
                SeedDatabase(fileName);
            }
            QueryDatabaseOrm(fileName);
            Console.ReadKey();
        }

        private static void SeedDatabase(string fileName)
        {
            string dbConnection = String.Format("Data Source={0}", fileName);
            String sql = @"
                create table [Badges] (
                [Id] INTEGER PRIMARY KEY ASC,
                [Title] varchar(20) ,
                [Description] varchar(255),
                [Level] int)";
            ExecuteNonQuery(dbConnection, sql);

            sql = @"insert into Badges ([Title], [Description], [Level]) 
             values ('Site MVP', 'Awarded to members who contribute often and wisely', 2);";
            ExecuteNonQuery(dbConnection, sql);
        }

        private static int ExecuteNonQuery(string dbConnection, string sql)
        {
            SQLiteConnection cnn = new SQLiteConnection(dbConnection);
            try
            {
                cnn.Open();
                SQLiteCommand mycommand = new SQLiteCommand(cnn);
                mycommand.CommandText = sql;
                int rowsUpdated = mycommand.ExecuteNonQuery();
                return rowsUpdated;
            }
            catch (Exception fail)
            {
                Console.WriteLine(fail.Message);
                return 0;
            }
            finally
            {
                cnn.Close();
            }
        }

        private static void QueryDatabaseOrm(string fileName)
        {
            // create a database "context" object
            String connectionString = String.Format("Data Source={0}", fileName);
            DbProviderFactory sqlFactory = new System.Data.SQLite.SQLiteFactory();
            PetaPoco.Database db = new PetaPoco.Database(connectionString, sqlFactory);

            // load an array of POCO for Badges
            String sql = "select * from Badges";
            foreach (Badge rec in db.Query<Badge>(sql))
            {
                Console.WriteLine("{0} {1} {2}", rec.Id, rec.Title, rec.Description);
            }
        }
    }
}
