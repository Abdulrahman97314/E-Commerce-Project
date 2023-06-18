using Store.Core;
using Store.Core.Entities;
using Store.Core.Entities.Order;
using Store.Core.Repositories;
using Store.Core.Services;
using Store.Core.Specifications.OrderSpecifications;

namespace Store.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;
        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            var basket = await basketRepository.GetBasketAsync(basketId);
            var orderItems = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var productItemOrdered = new ProductItemOrdered(item.Id,item.ProductName,item.PictureUrl);
                if (product is null)
                    return null;
                if (item.Price!=product.Price)
                    item.Price = product.Price;
                var orderItem = new OrderItem(productItemOrdered,item.Price,item.Quantity);
                orderItems.Add(orderItem);
            }
            var subTotal = orderItems.Sum(OI=>OI.Price * OI.Quantity);
            var spec = new OrderWithPaymentIntentIdSpecifications(basket.PaymentIntentId);
            var order = await unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if (order != null)
            {
                order.ShippingAddress = shippingAddress;
                order.DeliveryMethod = deliveryMethod;
                order.SubTotal =subTotal;
                order.Items= orderItems;
                unitOfWork.Repository<Order>().Update(order);
            }
            else
            {
                order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal, basket.PaymentIntentId);
                await unitOfWork.Repository<Order>().AddAsync(order);
            }
            await unitOfWork.CompleteAsync();
            return order;
        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethods()
            =>unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

        public async Task<Order> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderSpecifications(buyerEmail, orderId);
            var order = await unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            return order;
        }
        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);
            var orders = await unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }
    }
}
