using DataAccess.CRUD;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreApp
{
    public class CommerceManager : BaseManager
    {
        public void Create(Commerce commerce)
        {
            try
            {
                ValidateFields(commerce);

                var cCrud = new CommerceCrudFactory();

                /*var existingById = cCrud.RetrieveById<Commerce>(commerce.LegalId);
                if (existingById != null)
                    throw new Exception("Ya existe un comercio registrado con esta cédula jurídica.");

                var existingByEmail = cCrud.RetrieveByEmail<Commerce>(commerce);
                if (existingByEmail != null)
                    throw new Exception("El correo electrónico ya está registrado a otro comercio.");*/

                // Establece el estado inicial como "pendiente"
                commerce.Status = "Pendiente";
                commerce.CommissionRate = 0.00m; // Establece una tasa de comisión por defecto

                cCrud.Create(commerce);

                // Aquí podrías retornar o registrar el mensaje: "Comercio registrado correctamente"             
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        private void ValidateFields(Commerce commerce)
        {
            if (string.IsNullOrWhiteSpace(commerce.LegalId))
                throw new Exception("La cédula jurídica es obligatoria.");
            if (!Regex.IsMatch(commerce.LegalId, @"^\d{10,}$"))
                throw new Exception("Debe contener al menos 10 dígitos numéricos.");

            if (string.IsNullOrWhiteSpace(commerce.Name))
                throw new Exception("El nombre del comercio es obligatorio.");
            if (commerce.Name.Length > 100)
                throw new Exception("No debe exceder los 100 caracteres.");

            if (string.IsNullOrWhiteSpace(commerce.Phone))
                throw new Exception("El teléfono es obligatorio.");
            if (!Regex.IsMatch(commerce.Phone, @"^\d{8}$"))
                throw new Exception("Debe tener 8 dígitos numéricos.");

            if (string.IsNullOrWhiteSpace(commerce.Email))
                throw new Exception("El correo electrónico es obligatorio.");
            if (!Regex.IsMatch(commerce.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new Exception("Ingrese un correo válido.");

            if (commerce.Latitude == 0 || commerce.Longitude == 0)
                throw new Exception("Debe seleccionar una ubicación en el mapa.");

            if (string.IsNullOrWhiteSpace(commerce.IBAN))
                throw new Exception("La cuenta bancaria es obligatoria.");
            if (!Regex.IsMatch(commerce.IBAN, @"^\d{22}$"))
                throw new Exception("Debe tener un formato válido.");
        }
    }
}
