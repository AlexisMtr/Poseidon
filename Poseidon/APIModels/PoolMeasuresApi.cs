using Poseidon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poseidon.APIModels
{
    public class PoolMeasuresApi
    {
        public Measure Ph { get; set; }
        public Measure Temperature { get; set; }
        public Measure Level { get; set; }
    }
}
