using System;
using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Initializer
{
    public class CreditCardInitializer
    {
        public static CreditCard[] GetCreditCards()
        {
            CreditCard[] creditCards =
            {
                new CreditCard { Limit = 2999.99m, MoneyOwed = 0.01m, ExpirationDate = DateTime.Now.AddMonths(1) },
                new CreditCard { Limit = 5000.00m, MoneyOwed = 0.00m, ExpirationDate = DateTime.Now.AddMonths(2) }
            };

            return creditCards;
        }
    }
}