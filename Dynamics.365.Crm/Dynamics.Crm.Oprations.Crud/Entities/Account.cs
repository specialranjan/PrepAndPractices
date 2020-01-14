using System;

namespace Dynamics.Crm.Oprations.Crud.Entities
{
    public class Account
    {
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public AccountType AccountType { get; set; }
        public string Address1_Line1 { get; set; }
        public string Address1_Line2 { get; set; }
        public Decimal Address1_Longitude { get; set; }
    }
}
