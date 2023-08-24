﻿using StoceClient.DatabaseModels;
using System.ComponentModel.DataAnnotations;

namespace StoreClient.DatabaseModels
{
    public class ORDERDETAILS
    {
        public int? Id { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public int Volume { get; set; }
        public double Concentration { get; set; }
        public OrderStatusMapping Status { get; set; }
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
                    default:
                        return "";
                }
            }
            set
            {
                if (value == "Zamówione") Status = OrderStatusMapping.ORDERED;
                else if (value == "Przygotowane") Status = OrderStatusMapping.PREPARED;
                else if (value == "Dostarczone") Status = OrderStatusMapping.DELIVERED;
            }
        }
        public ORDER Order { get; set; }
    }
}