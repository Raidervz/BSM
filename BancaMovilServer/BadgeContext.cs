using BancaMovilServer.DbModels;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaMovilServer
{
    public class BadgeContext
    {
        public Badge GetById(int id)
        {
            String sql = "select * from Badges where Id =" + id.ToString();
            return BadgeContext.GetDatabase().FirstOrDefault<Badge>(sql);
        }
        public void Add(Badge badge)
        {
            BadgeContext.GetDatabase().Insert(badge);
        }
        internal void update(Badge badge)
        {
            BadgeContext.GetDatabase().Update(badge);
        }
        internal void delete(Badge badge)
        {
            BadgeContext.GetDatabase().Delete(badge);
        }

        public List<Badge> GetAll()
        {
            return BadgeContext.GetDatabase().Fetch<Badge>("select * from Badges");
        }

        public List<Badge> SearchByName(string parameter = "")
        {
            if (parameter == "")
            {
                return BadgeContext.GetDatabase().Fetch<Badge>("select * from Badges");
            }

            string sql = String.Format("select * from Badges where title like '%{0}%' or description like '%{0}%'", parameter);

            return BadgeContext.GetDatabase().Fetch<Badge>(sql);
        }
        

        private static PetaPoco.Database GetDatabase()
        {
            // A sqlite database is just a file.
            String fileName = "badges.db";
            if (!File.Exists(fileName))
            {
                SeedDatabase(fileName);
            }
            String connectionString = String.Format("Data Source={0}", fileName);
            DbProviderFactory sqlFactory = new System.Data.SQLite.SQLiteFactory();
            PetaPoco.Database db = new PetaPoco.Database(connectionString, sqlFactory);
            return db;
        }

        private static void SeedDatabase(string fileName)
        {
            File.Create(fileName).Dispose();

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
    }
}
