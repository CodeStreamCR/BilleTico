using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class FinancialEntity : BaseDTO
    {
        public string LegalId { get; set; } // Cédula jurídica nvarchar(12)
        public string BankCode { get; set; } // Código bancario nvarchar(10)
        public string Name { get; set; } // Nombre nvarchar(100)
        public string Phone { get; set; } // Teléfono nvarchar(8)
        public string Email { get; set; } // Email nvarchar(100)
        public decimal Latitude { get; set; } // Coordenada de latitud
        public decimal Longitude { get; set; } // Coordenada de longitud
        public string Status { get; set; } // Estado: "Pendiente", "Activa", "Rechazada"
        public decimal CommissionRate { get; set; } // Comisión asignada (HU 2.2)
    }
}