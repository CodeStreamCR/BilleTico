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
        public CommerceManager() { }

        public void Create(Commerce commerce)
        {
            try
            {
                //Validar que todos los campos obligatorios estén llenos
                if (ValidateRequiredFields(commerce))
                {
                    //Validar formato del correo electrónico
                    if (ValidateEmailFormat(commerce.Email))
                    {
                        //Validar formato del número de teléfono
                        if (ValidatePhoneFormat(commerce.Phone))
                        {
                            //Validar formato de cédula jurídica
                            if (ValidateLegalIdFormat(commerce.LegalId))
                            {
                                //Validar formato del IBAN
                                if (ValidateIBAN(commerce.IBAN))
                                {
                                    var cCrud = new CommerceCrudFactory();

                                    //Consultar en la DB si existe con ese IBAN
                                    var eExist = cCrud.RetrieveByIBAN<Commerce>(commerce);

                                    if (eExist == null)
                                    {
                                        //Consultar si existe por cédula jurídica
                                        eExist = cCrud.RetrieveByLegalId<Commerce>(commerce);

                                        if (eExist == null)
                                        {
                                            // Establecer estado como "Pendiente" (T22)
                                            commerce.Status = "Pendiente";
                                            commerce.CommissionRate = 0.00m; // Inicializar comisión
                                            cCrud.Create(commerce);
                                        }
                                        else
                                        {
                                            throw new Exception("Esta cédula jurídica ya está registrada");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Este IBAN ya está siendo utilizado en otro usuario, entidad o comercio");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Formato de IBAN inválido. Asegúrese de ingresar un valor que comience con 'CR' seguido de 20 dígitos");
                                }
                            }
                            else
                            {
                                throw new Exception("La cédula jurídica debe contener 10 dígitos numéricos");
                            }
                        }
                        else
                        {
                            throw new Exception("El teléfono debe tener 8 dígitos numéricos");
                        }
                    }
                    else
                    {
                        throw new Exception("Ingrese un correo electrónico válido");
                    }
                }
                else
                {
                    throw new Exception("Todos los campos obligatorios deben estar completos");
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        // =============================================
        // MÉTODOS HU 3.2 - APROBACIÓN
        // =============================================

        /*
         * T24: Obtener lista de comercios pendientes de aprobación
         */

        public List<Commerce> GetPendingCommerce()
        {
            var cCrud = new CommerceCrudFactory();
            return cCrud.RetrieveByStatus<Commerce>("Pendiente");
        }

        /*
         * T25: Vista detallada de una solicitud específica
         */

        public Commerce GetEntityDetails(int commerceId)
        {
            var cCrud = new CommerceCrudFactory();
            return cCrud.RetrieveById<Commerce>(commerceId);
        }

        /*
         * T26: Aprobar comercio y agregar con comisión
         */

        public void ApproveCommerce(int commerceId, decimal commissionRate)
        {
            // Validar que la comisión sea válida
            if (!ValidateCommissionRate(commissionRate))
            {
                throw new Exception("La comisión debe estar entre 0.01% y 99.99%");
            }

            var cCrud = new CommerceCrudFactory();

            // Verificar que el comercio existe y está pendiente
            var commerce = cCrud.RetrieveById<Commerce>(commerceId);
            if (commerce == null)
            {
                throw new Exception("El comercio no existe.");
            }

            if (commerce.Status != "Pendiente")
            {
                throw new Exception("Solo se pueden aprobar comercios en estado pendiente");
            }

            // Aprobar el comercio y establecer la comisión
            cCrud.ApproveCommerce(commerceId, commissionRate);
        }

        /*
         * T26: Rechazar comercio
         */

        public void RejectCommerce(int commerceId)
        {
            var cCrud = new CommerceCrudFactory();

            // Verificar que el comercio existe y está pendiente
            var commerce = cCrud.RetrieveById<Commerce>(commerceId);
            if (commerce == null)
            {
                throw new Exception("El comercio no existe.");
            }

            if (commerce.Status != "Pendiente")
            {
                throw new Exception("Solo se pueden rechazar comercios en estado pendiente");
            }

            // Rechazar el comercio
            cCrud.RejectCommerce(commerceId);
        }

        /*
         * T27: Notificar al comercio que fue aprobado o rechazado
         */

        /*
         * T28: Validar que solo comercios activos puedan operar
         */

        public bool CanCommerceOperate(int commerceId)
        {
            var cCrud = new CommerceCrudFactory();
            var commerce = cCrud.RetrieveById<Commerce>(commerceId);

            return commerce != null && commerce.Status == "Activo";
        }

        public List<Commerce> GetActiveCommerces()
        {
            var cCrud = new CommerceCrudFactory();
            return cCrud.RetrieveByStatus<Commerce>("Activo");
        }

        public List<Commerce> GetRejectedCommerces()
        {
            var cCrud = new CommerceCrudFactory();
            return cCrud.RetrieveByStatus<Commerce>("Rechazado");
        }

        // =============================================
        // MÉTODOS DE VALIDACIÓN PRIVADOS
        // =============================================


        private bool ValidateRequiredFields(Commerce commerce)
        {
            return !string.IsNullOrEmpty(commerce.LegalId) &&
                   !string.IsNullOrEmpty(commerce.Name) &&
                   !string.IsNullOrEmpty(commerce.Phone) &&
                   !string.IsNullOrEmpty(commerce.Email) &&
                   commerce.Latitude != 0 &&
                   commerce.Longitude != 0 &&
                   !string.IsNullOrEmpty(commerce.IBAN);
        }

        private bool ValidateEmailFormat(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        private bool ValidatePhoneFormat(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return false;

            // Debe tener exactamente 8 dígitos numéricos
            string phonePattern = @"^[0-9]{8}$";
            return Regex.IsMatch(phone, phonePattern);
        }

        private bool ValidateLegalIdFormat(string legalId)
        {
            if (string.IsNullOrEmpty(legalId))
                return false;

            // Debe tener exactamente 10 dígitos numéricos
            string legalIdPattern = @"^[0-9]{10}$";
            return Regex.IsMatch(legalId, legalIdPattern);
        }

        private bool ValidateIBAN(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
                return false;

            const string pattern = @"^CR\d{20}$";
            return Regex.IsMatch(iban, pattern, RegexOptions.IgnoreCase);
        }

        private bool ValidateCommissionRate(decimal commissionRate)
        {
            // Comisión debe estar entre 0.01% y 99.99%
            return commissionRate >= 0.01m && commissionRate <= 99.99m;
        }

    }
}
