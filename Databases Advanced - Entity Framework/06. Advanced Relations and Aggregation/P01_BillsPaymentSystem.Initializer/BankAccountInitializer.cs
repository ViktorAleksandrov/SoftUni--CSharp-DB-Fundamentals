using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Initializer
{
    public class BankAccountInitializer
    {
        public static BankAccount[] GetBankAccounts()
        {
            BankAccount[] bankAccounts =
            {
                new BankAccount { BankName = "OBB", SwiftCode = "swift1", Balance = 500.55m },
                new BankAccount { BankName = "Sibank", SwiftCode = "swift2", Balance = 44.79m }
            };

            return bankAccounts;
        }
    }
}
