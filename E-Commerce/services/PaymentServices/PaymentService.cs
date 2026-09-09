using ClothingStore.Entities;
using E_Commerce.Services.PaymentServices;

namespace E_Commerce.services.PaymantServices
{
    public class PaymentService : IPaymentService
    {
        public Task<bool> ProcessAsync(Order order, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }
    }
}
