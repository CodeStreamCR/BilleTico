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
    public class FinancialEntityManager
    {
        public FinancialEntityManager() { }

        // =============================================
        // MÉTODOS HU 2.1 - REGISTRO
        // =============================================

        /*
         * Método para la creación de una entidad financiera
         * Valida formato del correo electrónico y número de teléfono (T12)
         * Verifica que el código bancario no esté duplicado (T12)
         * Verifica que la cédula jurídica no esté duplicada (T12)
         * Asegura que todos los campos obligatorios estén llenos (T12)
         * Almacena solicitud en estado "pendiente" (T13)
         */
        public void Create(FinancialEntity financialEntity)
        {
            //Validar que todos los campos obligatorios estén llenos
            if (ValidateRequiredFields(financialEntity))
            {
                //Validar formato del correo electrónico
                if (ValidateEmailFormat(financialEntity.Email))
                {
                    //Validar formato del número de teléfono
                    if (ValidatePhoneFormat(financialEntity.Phone))
                    {
                        //Validar formato de cédula jurídica
                        if (ValidateLegalIdFormat(financialEntity.LegalId))
                        {
                            //Validar formato de código bancario
                            if (ValidateBankCodeFormat(financialEntity.BankCode))
                            {
                                var feCrud = new FinancialEntityCrudFactory();

                                //Consultar en la DB si existe con ese código bancario
                                var feExist = feCrud.RetrieveByBankCode<FinancialEntity>(financialEntity);

                                if (feExist == null)
                                {
                                    //Consultar si existe por cédula jurídica
                                    feExist = feCrud.RetrieveByLegalId<FinancialEntity>(financialEntity);

                                    if (feExist == null)
                                    {
                                        // Establecer estado como "Pendiente" (T13)
                                        financialEntity.Status = "Pendiente";
                                        financialEntity.CommissionRate = 0.00m; // Inicializar comisión
                                        feCrud.Create(financialEntity);
                                    }
                                    else
                                    {
                                        throw new Exception("Esta cédula jurídica ya está registrada");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Este código bancario ya está registrado");
                                }
                            }
                            else
                            {
                                throw new Exception("El código bancario solo puede contener letras y números");
                            }
                        }
                        else
                        {
                            throw new Exception("La cédula jurídica debe contener 12 dígitos numéricos");
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

        // =============================================
        // MÉTODOS HU 2.2 - APROBACIÓN
        // =============================================

        /*
         * T15: Obtener lista de entidades pendientes
         */
        public List<FinancialEntity> GetPendingEntities()
        {
            var feCrud = new FinancialEntityCrudFactory();
            return feCrud.RetrieveByStatus<FinancialEntity>("Pendiente");
        }

        /*
         * T16: Obtener detalles de una solicitud específica
         */
        public FinancialEntity GetEntityDetails(int entityId)
        {
            var feCrud = new FinancialEntityCrudFactory();
            return feCrud.RetrieveById<FinancialEntity>(entityId);
        }

        /*
         * T17: Aprobar entidad financiera con comisión
         */
        public void ApproveEntity(int entityId, decimal commissionRate)
        {
            // Validar que la comisión sea válida
            if (!ValidateCommissionRate(commissionRate))
            {
                throw new Exception("La comisión debe estar entre 0.01% y 99.99%");
            }

            var feCrud = new FinancialEntityCrudFactory();

            // Verificar que la entidad existe y está pendiente
            var entity = feCrud.RetrieveById<FinancialEntity>(entityId);
            if (entity == null)
            {
                throw new Exception("La entidad financiera no existe");
            }

            if (entity.Status != "Pendiente")
            {
                throw new Exception("Solo se pueden aprobar entidades en estado pendiente");
            }

            // Aprobar la entidad
            feCrud.ApproveEntity(entityId, commissionRate);
        }

        /*
         * T17: Rechazar entidad financiera
         */
        public void RejectEntity(int entityId)
        {
            var feCrud = new FinancialEntityCrudFactory();

            // Verificar que la entidad existe y está pendiente
            var entity = feCrud.RetrieveById<FinancialEntity>(entityId);
            if (entity == null)
            {
                throw new Exception("La entidad financiera no existe");
            }

            if (entity.Status != "Pendiente")
            {
                throw new Exception("Solo se pueden rechazar entidades en estado pendiente");
            }

            // Rechazar la entidad
            feCrud.RejectEntity(entityId);
        }

        /*
         * T19: Validar que solo entidades activas puedan operar
         */
        public bool CanEntityOperate(int entityId)
        {
            var feCrud = new FinancialEntityCrudFactory();
            var entity = feCrud.RetrieveById<FinancialEntity>(entityId);

            return entity != null && entity.Status == "Activa";
        }

        public List<FinancialEntity> GetActiveEntities()
        {
            var feCrud = new FinancialEntityCrudFactory();
            return feCrud.RetrieveByStatus<FinancialEntity>("Activa");
        }

        public List<FinancialEntity> GetRejectedEntities()
        {
            var feCrud = new FinancialEntityCrudFactory();
            return feCrud.RetrieveByStatus<FinancialEntity>("Rechazada");
        }

        // =============================================
        // MÉTODOS DE VALIDACIÓN PRIVADOS
        // =============================================

        private bool ValidateRequiredFields(FinancialEntity financialEntity)
        {
            return !string.IsNullOrEmpty(financialEntity.LegalId) &&
                   !string.IsNullOrEmpty(financialEntity.BankCode) &&
                   !string.IsNullOrEmpty(financialEntity.Name) &&
                   !string.IsNullOrEmpty(financialEntity.Phone) &&
                   !string.IsNullOrEmpty(financialEntity.Email) &&
                   financialEntity.Latitude != 0 &&
                   financialEntity.Longitude != 0;
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

            // Debe tener exactamente 12 dígitos numéricos
            string legalIdPattern = @"^[0-9]{12}$";
            return Regex.IsMatch(legalId, legalIdPattern);
        }

        private bool ValidateBankCodeFormat(string bankCode)
        {
            if (string.IsNullOrEmpty(bankCode))
                return false;

            // Solo letras y números
            string bankCodePattern = @"^[A-Za-z0-9]+$";
            return Regex.IsMatch(bankCode, bankCodePattern);
        }

        private bool ValidateCommissionRate(decimal commissionRate)
        {
            // Comisión debe estar entre 0.01% y 99.99%
            return commissionRate >= 0.01m && commissionRate <= 99.99m;
        }
    }
}