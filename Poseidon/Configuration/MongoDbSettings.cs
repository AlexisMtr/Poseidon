using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poseidon.Configuration
{
    public class MongoDbSettings
    {
        public string DefaultConnectionString { get; set; }
        public string DefaultDbName { get; set; }
    }
}
