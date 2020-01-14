namespace LocksAndMonitor
{
    using System;
    using System.Threading;
    internal class Account1
    {
        int balance;
        Object ranjanLock = new Object();
        Random random = new Random();

        public Account1(int initialBalance)
        {
            balance = initialBalance;
        }

        int Withdraw(int amount)
        {
            if (balance < 0)
            {
                throw new Exception("Not enough balance");
            }

            lock (ranjanLock)
            {

                if (balance >= amount)
                {
                    Console.WriteLine("Amount drawn: " + amount);
                    balance = balance - amount;
                    return balance;
                }
            }
            return 0;
        }

        public void WithdrawRandomOnly()
        {
            for (int i = 0; i < 100; i++)
            {
                var balance = Withdraw(random.Next(2000, 5000));
                if (balance > 0)
                {
                    Console.WriteLine("Balance left " + balance);
                }
                else
                {
                    Console.WriteLine("Balance left " + balance);
                }
            }
        }
    }
}
