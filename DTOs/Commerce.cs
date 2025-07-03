using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class Commerce : BaseDTO
    {
        public string legalEntity { get; set; }

        public string name { get; set; }

        public string phone { get; set; }

        public string email { get; set; }

        public decimal latitude { get; set; }

        public decimal longitude { get; set; }

        public string status { get; set; }

        public string IBAN { get; set; }

        public decimal commissionRate { get; set; }
    }
}
