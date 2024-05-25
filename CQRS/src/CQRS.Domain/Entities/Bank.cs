using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Domain.Entities
{
    public class Bank
    {  
        public string Name { get; set; }
        public string Bic { get; set; }
        public string Country { get; set; }
    }
}
