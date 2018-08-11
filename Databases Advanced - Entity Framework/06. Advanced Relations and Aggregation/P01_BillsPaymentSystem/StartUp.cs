using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using P01_BillsPaymentSystem.Data;
using P01_BillsPaymentSystem.Data.Models;
using P01_BillsPaymentSystem.Initializer;

namespace P01_BillsPaymentSystem
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new BillsPaymentSystemContext())
            {
                int userId = int.Parse(Console.ReadLine());

                User user = GetUser(context, userId);

                if (user == null)
                {
                    Console.WriteLine($"User with id {userId} not found!");
                }
                else
                {
                    string userInfo = GetUserInfo(user);

                    Console.WriteLine(userInfo);
                }

                PayBills(context, user, 10);
            }
        }

        private static void PayBills(BillsPaymentSystemContext context, User user, decimal amount)
        {
            decimal bankAccountMoney = user.PaymentMethods
                .Where(p => p.BankAccount != null)
                .Sum(p => p.BankAccount.Balance);

            decimal creditcardMoney = user.PaymentMethods
                .Where(p => p.CreditCard != null)
                .Sum(p => p.CreditCard.LimitLeft);

            decimal totalMoney = bankAccountMoney + creditcardMoney;

            if (totalMoney < amount)
            {
                Console.WriteLine("Insufficient funds!");
            }
            else
            {
                BankAccount[] bankAccounts = user.PaymentMethods
                    .Where(p => p.BankAccount != null)
                    .Select(p => p.BankAccount)
                    .OrderBy(b => b.BankAccountId).ToArray();

                foreach (BankAccount account in bankAccounts)
                {
                    if (account.Balance >= amount)
                    {
                        account.Withdraw(amount);

                        amount = 0;

                        break;
                    }
                    else
                    {
                        account.Withdraw(account.Balance);

                        amount -= account.Balance;
                    }
                }

                if (amount > 0)
                {
                    CreditCard[] creditCards = user.PaymentMethods
                        .Where(p => p.CreditCard != null)
                        .Select(p => p.CreditCard)
                        .OrderBy(c => c.CreditCardId)
                        .ToArray();

                    foreach (CreditCard card in creditCards)
                    {
                        if (card.LimitLeft >= amount)
                        {
                            card.Withdraw(amount);
                            break;
                        }
                        else
                        {
                            card.Withdraw(card.LimitLeft);

                            amount -= card.LimitLeft;
                        }
                    }
                }

                context.SaveChanges();
            }
        }

        private static string GetUserInfo(User user)
        {
            var userInfo = new StringBuilder();

            userInfo.AppendLine($"User: {user.FirstName} {user.LastName}");

            userInfo.AppendLine("Bank Accounts:");

            BankAccount[] bankAccounts = user.PaymentMethods
                .Where(p => p.BankAccount != null)
                .Select(p => p.BankAccount).ToArray();

            foreach (BankAccount account in bankAccounts)
            {
                userInfo.AppendLine($"-- ID: {account.BankAccountId}");
                userInfo.AppendLine($"--- Balance: {account.Balance:F2}");
                userInfo.AppendLine($"--- Bank: {account.BankName}");
                userInfo.AppendLine($"--- SWIFT: {account.SwiftCode}");
            }

            userInfo.AppendLine("Credit Cards:");

            CreditCard[] creditCards = user.PaymentMethods
                .Where(p => p.CreditCard != null)
                .Select(p => p.CreditCard).ToArray();

            foreach (CreditCard card in creditCards)
            {
                userInfo.AppendLine($"-- ID: {card.CreditCardId}");
                userInfo.AppendLine($"--- Limit: {card.Limit:F2}");
                userInfo.AppendLine($"--- Money Owed: {card.MoneyOwed:F2}");
                userInfo.AppendLine($"--- Limit Left:: {card.LimitLeft:F2}");
                userInfo.AppendLine(
                    $"--- Expiration Date: {card.ExpirationDate.ToString("yyyy/MM", CultureInfo.InvariantCulture)}");
            }

            return userInfo.ToString().TrimEnd();
        }

        private static User GetUser(BillsPaymentSystemContext context, int userId)
        {
            User user = context.Users
                .Where(u => u.UserId == userId)
                .Include(u => u.PaymentMethods)
                .ThenInclude(p => p.BankAccount)
                .Include(u => u.PaymentMethods)
                .ThenInclude(p => p.CreditCard)
                .FirstOrDefault();

            return user;
        }
    }
}
