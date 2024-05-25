using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Domain.Entities
{
    public class Bank
    {
        public Bank(string name, string bic, string country)
        {
            Name = name;
            Bic = bic;
            Country = country;
        }

        public string Name { get; set; }
        public string Bic { get; set; }
        public string Country { get; set; }
    }
}
