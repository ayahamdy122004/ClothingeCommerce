using ClothingStore.Entities;

namespace E_Commerce.Services.PaymentServices
{
   
        public interface IPaymentService
        {
            Task<bool> ProcessAsync(Order order, CancellationToken cancellationToken = default);
        }
    

}