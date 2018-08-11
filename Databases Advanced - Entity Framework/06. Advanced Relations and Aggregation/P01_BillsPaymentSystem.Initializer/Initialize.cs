using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using P01_BillsPaymentSystem.Data;
using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Initializer
{
    public class Initialize
    {
        public static void Seed(BillsPaymentSystemContext context)
        {
            InsertUsers(context);
            InsertCreditCards(context);
            InsertBankAccounts(context);
            InsertPaymentMethods(context);
        }

        private static void InsertPaymentMethods(BillsPaymentSystemContext context)
        {
            PaymentMethod[] payments = PaymentMethodInitializer.GetPaymentMethods();

            for (int index = 0; index < payments.Length; index++)
            {
                if (IsValid(payments[index]))
                {
                    context.PaymentMethods.Add(payments[index]);
                }
            }

            context.SaveChanges();
        }

        private static void InsertBankAccounts(BillsPaymentSystemContext context)
        {
            BankAccount[] bankAccounts = BankAccountInitializer.GetBankAccounts();

            for (int index = 0; index < bankAccounts.Length; index++)
            {
                if (IsValid(bankAccounts[index]))
                {
                    context.BankAccounts.Add(bankAccounts[index]);
                }
            }

            context.SaveChanges();
        }

        private static void InsertCreditCards(BillsPaymentSystemContext context)
        {
            CreditCard[] creditCards = CreditCardInitializer.GetCreditCards();

            for (int index = 0; index < creditCards.Length; index++)
            {
                if (IsValid(creditCards[index]))
                {
                    context.CreditCards.Add(creditCards[index]);
                }
            }

            context.SaveChanges();
        }

        private static void InsertUsers(BillsPaymentSystemContext context)
        {
            User[] users = UserInitializer.GetUsers();

            for (int index = 0; index < users.Length; index++)
            {
                if (IsValid(users[index]))
                {
                    context.Users.Add(users[index]);
                }
            }

            context.SaveChanges();
        }

        public static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);

            var results = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, results, true);
        }
    }
}
