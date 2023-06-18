using System.Runtime.Serialization;

namespace Store.Core.Entities.Order
{
    public enum OrderStatus
    {
        Pending,
        PaymentRecived,
        PaymentFailed
    }
}
