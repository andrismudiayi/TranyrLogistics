using System;

namespace TranyrLogistics.Models.Utility
{
    public class ShipmentModel
    {
        public static string GenerateReferenceNumber(Shipment shipment)
        {
            return String.Format("{0:yyMM}", DateTime.Now) + shipment.Category.ToString().Substring(0, 3) + String.Format("{0:mmssff}", DateTime.Now);
        }
    }
}