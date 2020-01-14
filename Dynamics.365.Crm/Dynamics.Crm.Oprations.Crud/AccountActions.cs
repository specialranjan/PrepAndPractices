namespace Dynamics.Crm.Oprations.Crud
{
    using System;
    using System.Collections.Generic;
    using Dynamics.Crm.Oprations.Crud.Entities;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Messages;
    using Microsoft.Xrm.Sdk.Metadata;
    using Microsoft.Xrm.Sdk.Metadata.Query;
    using Microsoft.Xrm.Sdk.Query;
    using Microsoft.Crm.Sdk.Messages;

    public class AccountActions
    {
        //List of attributes of Account Entity
        //https://docs.microsoft.com/en-us/dynamics365/customerengagement/on-premises/developer/entities/account

        private static IOrganizationService service = OrgnizationService.Instance;
        
        public static List<Account> GetAccounts()
        {
            List<Account> accounts = new List<Account>();

            QueryExpression query = new QueryExpression("account");
            query.ColumnSet = new ColumnSet(new string[] { "name", "address1_longitude", "address1_line1", "address1_line2" });
            //query.Orders.Add(new OrderExpression("name", OrderType.Ascending));            
            
            EntityCollection entityCollection = service.RetrieveMultiple(query);            
            foreach (Entity Item in entityCollection.Entities)
            {
                accounts.Add(FillData(Item));
            }
            return accounts;
        }

        public static List<Account> GetTop5Accounts()
        {
            List<Account> accounts = new List<Account>();

            QueryExpression query = new QueryExpression("account");
            query.ColumnSet = new ColumnSet(new string[] { "name", "address1_longitude", "address1_line1", "address1_line2" });
            //query.Orders.Add(new OrderExpression("name", OrderType.Descending));
            query.TopCount = 5;

            EntityCollection entityCollection = service.RetrieveMultiple(query);
            foreach (Entity Item in entityCollection.Entities)
            {
                accounts.Add(FillData(Item));
            }
            return accounts;
        }

        public static Account GetAccountById(Guid accountId)
        {
            Account account = new Account();

            QueryExpression query = new QueryExpression("account");
            query.ColumnSet = new ColumnSet(new string[] { "name", "address1_longitude","address1_line1", "address1_line2" });
            query.Criteria.AddCondition("accountid", ConditionOperator.Equal, accountId);

            EntityCollection entityCollection = service.RetrieveMultiple(query);
            foreach (Entity Item in entityCollection.Entities)
            {
                account = FillData(Item);
            }
            return account;
        }

        public static void UpdateAccount(Guid accountId)
        {
            Entity entity = new Entity("account");
            entity.Id = accountId;
            entity.Attributes.Add("address1_longitude", Convert.ToDecimal(150));
            entity.Attributes.Add("address1_line1", "Test Street 1");
            entity.Attributes.Add("address1_line2", "Test Street 2");
            service.Update(entity);
        }

        public static Guid AddAttribute()
        {
            BooleanAttributeMetadata booleanAttribute = new BooleanAttributeMetadata {
                SchemaName = "new_Boolean",
                LogicalName = "new_boolean",
                DisplayName = new Label("Sample Boolean", 1033),
                RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                Description = new Label("Boolean Attribute", 1003),
                OptionSet=new BooleanOptionSetMetadata(
                    new OptionMetadata(new Label("True",1033),1),
                    new OptionMetadata(new Label("False", 1033), 0))
            };

            CreateAttributeRequest createAttributeRequest = new CreateAttributeRequest
            {
                EntityName = "testaccount",
                Attribute = booleanAttribute
            };

            var response = service.Execute(createAttributeRequest);
            return new Guid();
        }

        public static string geEntityLogicalName(int ObjectTypeCode)
        {
            //http://www.dynamicscrm.blog/object-type-codes-cheat-sheet-for-dynamics-365/
            //https://msdynamicscrmblog.wordpress.com/2013/07/18/entity-type-codes-in-dynamics-crm-2011/

            string entityLogicalName = String.Empty;

            MetadataFilterExpression EntityFilter = new MetadataFilterExpression(LogicalOperator.And);
            EntityFilter.Conditions.Add(new MetadataConditionExpression("ObjectTypeCode", MetadataConditionOperator.Equals, ObjectTypeCode));
            MetadataPropertiesExpression mpe = new MetadataPropertiesExpression();
            mpe.AllProperties = false;
            mpe.PropertyNames.Add("ObjectTypeCode");
            EntityQueryExpression entityQueryExpression = new EntityQueryExpression()
            {
                Criteria = EntityFilter,
                Properties = mpe
            };

            RetrieveMetadataChangesResponse initialRequest = GetMetadataChanges(entityQueryExpression, null, DeletedMetadataFilters.OptionSet);
            if (initialRequest.EntityMetadata.Count == 1)
            {
                entityLogicalName = initialRequest.EntityMetadata[0].LogicalName;
            }

            return entityLogicalName;
        }


        protected static RetrieveMetadataChangesResponse GetMetadataChanges(EntityQueryExpression entityQueryExpression, String clientVersionStamp, DeletedMetadataFilters deletedMetadataFilter)
        {
            RetrieveMetadataChangesRequest retrieveMetadataChangesRequest = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression,
                ClientVersionStamp = clientVersionStamp,
                DeletedMetadataFilters = deletedMetadataFilter
            };

            return (RetrieveMetadataChangesResponse)service.Execute(retrieveMetadataChangesRequest);
        }

        public static void PrintAccount(Account account)
        {
            Console.WriteLine("Id:\t{0}", account.AccountId);
            Console.WriteLine("Name:\t{0}", account.AccountName);
            Console.WriteLine("Type:\t{0}", account.AccountType);
            Console.WriteLine("Address1_Line1:\t{0}", account.Address1_Line1);
            Console.WriteLine("Address1_Line2:\t{0}", account.Address1_Line2);
            Console.WriteLine("Address1_Longitude:\t{0}", account.Address1_Longitude);
        }

        public static void PrintAccountList(List<Account> accounts)
        {
            Console.WriteLine("List of Accounts");
            Console.WriteLine("---------------------------------------------");
            foreach (Account account in accounts)
            {
                Console.WriteLine("Id:\t{0}", account.AccountId);
                Console.WriteLine("Name:\t{0}", account.AccountName);
                Console.WriteLine("Type:\t{0}", account.AccountType);
                Console.WriteLine("Address1_Line1:\t{0}", account.Address1_Line1);
                Console.WriteLine("Address1_Line2:\t{0}", account.Address1_Line2);
                Console.WriteLine("Address1_Longitude:\t{0}", account.Address1_Longitude);
                Console.WriteLine(Environment.NewLine);
            }
        }

        private static Account FillData(Entity Data)
        {
            Account Item = new Account();
            Item.AccountId = Data.Id;

            if (Data.Attributes.Contains("name") && Data.Attributes["name"] != null)
            {
                Item.AccountName = Data.Attributes["name"].ToString();
            }

            if (Data.Attributes.Contains("address1_line1") && Data.Attributes["address1_line1"] != null)
            {
                Item.Address1_Line1 = Data.Attributes["address1_line1"].ToString();
            }

            if (Data.Attributes.Contains("address1_line2") && Data.Attributes["address1_line2"] != null)
            {
                Item.Address1_Line2 = Data.Attributes["address1_line2"].ToString();
            }

            if (Data.Attributes.Contains("address1_longitude") && Data.Attributes["address1_longitude"] != null)
            {
                Item.Address1_Longitude =Convert.ToDecimal(Data.Attributes["address1_longitude"].ToString());
            }

            if (Data.Attributes.Contains("new_accounttype") && Data.Attributes["new_accounttype"] != null)
            {
                AccountType Type = new AccountType();
                Type.Value = ((OptionSetValue)Data.Attributes["new_accounttype"]).Value;
                Type.Text = Data.FormattedValues["new_accounttype"].ToString();
            }

            return Item;
        }
    }
}
