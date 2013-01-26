using System;

namespace TranyrLogistics.Models.Utility
{
    public class CustomerModel
    {
        public static string GenerateCustomerNumber(Customer customer)
        {
            if (customer is Individual)
            {
                return ((Individual)customer).LastName.Substring(0, 1).ToUpper() + ((Individual)customer).FirstName.Substring(0, 1).ToUpper() + String.Format("{0:HHmmssfff}", DateTime.Now);
            }
            else if (customer is Company)
            {
                return ((Company)customer).Name.Substring(0, 2).ToUpper() + String.Format("{0:HHmmssfff}", DateTime.Now);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}