using System;
using System.Collections.Generic;

#nullable disable

namespace Project_PRN211_TEAM7.Models
{
    public partial class Oder
    {
        public Oder()
        {
            OderDetails = new HashSet<OderDetail>();
        }

        public int OrderId { get; set; }
        public int? Quantity { get; set; }
        public double? TotalPrice { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? CustomerId { get; set; }

        public Oder(int orderId, int? quantity, double? totalPrice, DateTime? orderDate, int? customerId)
        {
            OrderId = orderId;
            Quantity = quantity;
            TotalPrice = totalPrice;
            OrderDate = orderDate;
            CustomerId = customerId;
        }

        public virtual User Customer { get; set; }
        public virtual ICollection<OderDetail> OderDetails { get; set; }
    }
}
