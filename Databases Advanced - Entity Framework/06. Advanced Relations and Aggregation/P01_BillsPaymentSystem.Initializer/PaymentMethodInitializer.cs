using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Initializer
{
    public class PaymentMethodInitializer
    {
        public static PaymentMethod[] GetPaymentMethods()
        {
            PaymentMethod[] paymentMethods =
            {
                new PaymentMethod { UserId = 1, BankAccountId = 2, Type = PaymentMethodType.BankAccount },
                new PaymentMethod { UserId = 2, CreditCardId = 1, Type = PaymentMethodType.CreditCard }
            };

            return paymentMethods;
        }
    }
}