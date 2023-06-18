using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications.OrderSpecifications
{
    public class OrderSpecifications : BaseSpecification<Order>
    {
        public OrderSpecifications(string buyerEmail):base(O => O.BuyerEmail ==buyerEmail)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
            AddOrderByDesc(o => o.OrderDate);
        }
        public OrderSpecifications(string buyerEmail,int id) 
            : base(O => O.BuyerEmail == buyerEmail&& O.Id==id)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
        }
    }
}
