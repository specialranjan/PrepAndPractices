using System;
using System.Linq;
using Dynamics.Crm.Oprations.Crud.Entities;

namespace Dynamics.Crm.Oprations.Crud
{
    class Program
    {
        static void Main(string[] args)
        {
            var accountId = AccountActions.AddAttribute();
            //var logicalName = AccountActions.geEntityLogicalName(110);
            //var accounts = AccountActions.GetTop5Accounts();
            //AccountActions.PrintAccountList(accounts);

            //var account = AccountActions.GetAccountById(accounts.Select(x => x.AccountId).FirstOrDefault());
            
            

            //account = AccountActions.GetAccountById(account.AccountId);

            //AccountActions.PrintAccount(account);
            
            Console.ReadLine();            
        }

        
    }
}
