using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restapi_Pluszpont.Models
{
    public class BetterUserDatabaseSettings
    {
        
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string BetterUsersCollectionName { get; set; } = null!;
    }
}