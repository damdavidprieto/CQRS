using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Domain.Entities
{
    public interface IAuditable
    {
        string CreatedBy { get; set; }
        DateTime CreatedDateTime { get; set; }
        string UpdatedBy { get; set; }
        DateTime? UpdatedDateTime { get; set; }
    }
    public class Bank : IAuditable
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
        public string? CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
    }
}
