using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poseidon.Models
{
    public class UserPoolAssociation
    { 
        public int Id { get; set; }
        public User User { get; set; }
        public Pool Pool { get; set; }
    }
}
