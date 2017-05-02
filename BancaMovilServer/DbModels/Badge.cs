using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaMovilServer.DbModels
{
    [PetaPoco.TableName("Badges")]
    [PetaPoco.PrimaryKey("Id")]
    public class Badge
    {
        public Int64 Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public Int32? Level { get; set; }
    }
}
