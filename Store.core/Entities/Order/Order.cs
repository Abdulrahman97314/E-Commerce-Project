using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Entities.Order
{
    public class Order :BaseEntity
    {
        public Order() { }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod delivertMethod, ICollection<OrderItem> items, decimal subTotal,string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod= delivertMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public Address ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod{ get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        [DataType("decimal(18,2)")]
        public decimal SubTotal { get; set; }
        public decimal GetTotal()
        {
            return SubTotal + DeliveryMethod.Cost;
        }
        public string PaymentIntentId { get; set; }
    }
}
