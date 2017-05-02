using BancaMovilServer.DbModels;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
            String fileName = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "badges.db");
            String connectionString = "Data Source=" + fileName;
            DbProviderFactory sqlFactory = new System.Data.SQLite.SQLiteFactory();
            PetaPoco.Database db = new PetaPoco.Database(connectionString, sqlFactory);
            return db;
        }
    }
}
