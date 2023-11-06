using System;
using System.Collections.Generic;

namespace StoreClient.DatabaseModels
{
    public class ORDERDETAILS
    {
        public int? Id { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public int Volume { get; set; }
        public double Concentration { get; set; }
        public OrderStatusMapping Status { get; set; } = OrderStatusMapping.ORDERED;
        public DateTime? DeliveredDate { get; set; }
        public DateTime? SettledDate { get; set; }
        public string StatusMapping
        {
            get
            {
                switch (Status)
                {
                    case OrderStatusMapping.ORDERED:
                        return "Zamówione";
                    case OrderStatusMapping.PREPARED:
                        return "Przygotowane";
                    case OrderStatusMapping.DELIVERED:
                        return "Dostarczone";
                    case OrderStatusMapping.SETTLED:
                        return "Rozliczone";
                    default:
                        return "";
                }
            }
            set
            {
                if (value == "Zamówione") Status = OrderStatusMapping.ORDERED;
                else if (value == "Przygotowane") Status = OrderStatusMapping.PREPARED;
                else if (value == "Dostarczone") Status = OrderStatusMapping.DELIVERED;
                else if(value == "Rozliczone") Status = OrderStatusMapping.SETTLED;
            }
        }
        public static List<string> StatusOptions
        {
            get
            {
                return new List<string>()
                {
                    "Zamówione",
                    "Przygotowane",
                    "Dostarczone",
                    "Rozliczone"
                };
            }
        }
        public int? OrderId { get; set; }
    }
}
