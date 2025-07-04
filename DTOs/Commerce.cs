using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class Commerce : BaseDTO
    {
        public string LegalId { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string Status { get; set; }

        public string IBAN { get; set; }

        public decimal CommissionRate { get; set; }
    }
}
