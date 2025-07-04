using DataAccess.DAO;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CRUD
{
    public class CommerceCrudFactory : CrudFactory
    {

        public CommerceCrudFactory()
        {
            _sqlDao = SqlDao.GetInstance();
        }

        public override void Create(BaseDTO baseDTO)
        {
            var commerce = baseDTO as Commerce;
            var sqlOperation = new SqlOperation() { ProcedureName = "CRE_COMMERCE_PR" };

            sqlOperation.AddStringParameter("P_LegalId", commerce.LegalId);
            sqlOperation.AddStringParameter("P_Name", commerce.Name);
            sqlOperation.AddStringParameter("P_Phone", commerce.Phone);
            sqlOperation.AddStringParameter("P_Email", commerce.Email);
            sqlOperation.AddDecimalParameter("P_Latitude", commerce.Latitude);
            sqlOperation.AddDecimalParameter("P_Longitude", commerce.Longitude);
            sqlOperation.AddStringParameter("P_Status", commerce.Status);
            sqlOperation.AddStringParameter("P_IBAN", commerce.IBAN);
            sqlOperation.AddDecimalParameter("P_CommissionRate", commerce.CommissionRate);

            throw new NotImplementedException();
        }

        public override void Delete(BaseDTO baseDTO)
        {
            throw new NotImplementedException();
        }

        public override T Retrieve<T>()
        {
            throw new NotImplementedException();
        }

        public override List<T> RetrieveAll<T>()
        {
            throw new NotImplementedException();
        }

        public override T RetrieveById<T>(int id)
        {
            throw new NotImplementedException();
        }

        public override void Update(BaseDTO baseDTO)
        {
            throw new NotImplementedException();
        }

        private Commerce BuildCommerce(Dictionary<string, object> row)
        {
            var commerce = new Commerce()
            {
                Id = (int)row["Id"],
                Created = (DateTime)row["Created"],
                //Updated = (DateTime)row["Updated"],
                LegalId = (string)row["LegalId"],
                Name = (string)row["Name"],
                Phone = (string)row["Phone"],
                Email = (string)row["Email"],
                Latitude = (decimal)row["Latitude"],
                Longitude = (decimal)row["Longitude"],
                Status = (string)row["Status"],
                IBAN = (string)row["IBAN"],
                CommissionRate = (decimal)row["CommissionRate"]
            };
            return commerce;
        }
            
    }
}
