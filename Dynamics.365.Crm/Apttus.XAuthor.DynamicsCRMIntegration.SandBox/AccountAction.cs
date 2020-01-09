using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Messages;
using System.Configuration;

namespace Apttus.XAuthor.DynamicsCRMIntegration.SandBox
{
    public class AccountAction
    {



        public void getAllContactsCount()
        {
            QueryExpression query = new QueryExpression("contact");
            query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            EntityCollection List = CRMHelper.RetrieveMultiple(query,service); 

            Console.ReadLine();
        }

        public List<Account> getAllCustomerAccounts()
        {

            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            /*
             * 
             * Select AccountId, AccountName
             * From Account
             * Where AccountStatus= 'Customer'
             * Order by AccountName Asec
             * */

            QueryExpression query = new QueryExpression("account");
            query.ColumnSet = new ColumnSet(new string[] { "name"});                        
            query.Orders.Add(new OrderExpression("name", OrderType.Ascending));
            EntityCollection accountEntityList = service.RetrieveMultiple(query);
            List<Account> AccountList = new List<Account>();
            foreach (Entity Item in accountEntityList.Entities)
            {
                AccountList.Add(FillData(Item));
            }
            return AccountList;

        }

        public List<Account> getTop10Account()
        {
            List<Account> AccountList = new List<Account>();
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            QueryExpression query = new QueryExpression("account");
            query.TopCount = 20;     
            EntityCollection acocuntEntitySet = service.RetrieveMultiple(query);
            foreach (Entity Item in acocuntEntitySet.Entities)
            {
                AccountList.Add(FillData(Item));
            }
            return AccountList;
        }

        private Account FillData(Entity Data)
        {
            Account Item = new Account();
            Item.AccountId = Data.Id;

            if (Data.Attributes.Contains("name") && Data.Attributes["name"] != null)
            {
                Item.AccountName = Data.Attributes["name"].ToString();
            }

            if (Data.Attributes.Contains("new_accounttype") && Data.Attributes["new_accounttype"] != null)
            {
                AccountType Type = new AccountType();
                Type.Value = ((OptionSetValue)Data.Attributes["new_accounttype"]).Value;
                Type.Text = Data.FormattedValues["new_accounttype"].ToString();
            }

            return Item;
        }

        public void updateAccount(Guid acocuntId)
        {
            Entity account = new Entity("account");
            account.Id = acocuntId;

            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            account.Attributes.Add("address1_longitude", Convert.ToDecimal(500));

            service.Update(account);
        }

        public Guid InsertAccount()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            Entity Account = new Entity("account");
            Account.Attributes.Add("name", "Test Account");

            Guid AccountId = service.Create(Account);
            return AccountId;
        }


        public void DeleteAccount()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            Guid AccountId = new Guid();
            service.Delete("account", AccountId);

        }

        public void getAccountbyIndustryCode(Guid contactId)
        {
            QueryExpression query = new QueryExpression("account");
            query.Criteria.AddCondition("primarycontactid", ConditionOperator.Equal, contactId);
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            EntityCollection AccountList = service.RetrieveMultiple(query);
        }

        public void getaccountbylatitude()
        {
            string value = "50.5";
            QueryExpression query = new QueryExpression("account");
            query.ColumnSet = new ColumnSet(new string[] { "address1_latitude" });
            query.Criteria.AddCondition("address1_latitude", ConditionOperator.GreaterThan, Convert.ToDouble(value));
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            EntityCollection AccountList = service.RetrieveMultiple(query);
        }


        public void updatePhnAccounts()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            QueryExpression query = new QueryExpression("account");
            query.Criteria.AddCondition("new_includeinsalesfunnel", ConditionOperator.Equal, true);
            query.Criteria.AddCondition("new_accounttype", ConditionOperator.Equal, 3);
            query.Criteria.AddCondition("new_totalnoofdairycows", ConditionOperator.Null);

            EntityCollection accountLists = service.RetrieveMultiple(query);
            Random random = new Random();
            foreach(Entity Item in accountLists.Entities)
            {   
                int randomNumber = random.Next(0, 1000);
                Entity account = new Entity("account");
                account.Attributes.Add("new_totalnoofdairycows", randomNumber);
                account.Id = Item.Id;
                service.Update(account);
            }
       }



        public void updatePhnAccountsPrimaryAddress()
        {
            QueryExpression query = new QueryExpression("account");
            query.ColumnSet = new ColumnSet(new string[] { "name", "new_addressstaging_county", "new_addressstaging_postalcode", "new_addressactual_county", "address1_postalcode" });
            query.Criteria.AddCondition("new_addressactual_county", ConditionOperator.Null);

            IOrganizationService service = CRMHelper.ConnectToMSCRM1();
            EntityCollection accountList = service.RetrieveMultiple(query);

            Console.WriteLine("Total Account Records : " + accountList.Entities.Count);
            for(int count = 0; count < accountList.Entities.Count; count++)
            {
                Entity singleAccount = accountList.Entities[count];
                Entity updateAccount = new Entity(singleAccount.LogicalName);
                updateAccount.Id = singleAccount.Id;
                bool isRequiredUpdate = false;

                if (singleAccount.Attributes.Contains("address1_postalcode") && singleAccount.Attributes["address1_postalcode"] != null)
                {
                    string stagingzipCode = singleAccount.Attributes["address1_postalcode"].ToString();
                    EntityReference stagingCounty = getCountyFromZipCodeEntity(stagingzipCode, service);
                    if (stagingCounty != null)
                    {
                        updateAccount.Attributes.Add("new_addressactual_county", new EntityReference(stagingCounty.LogicalName, stagingCounty.Id));
                        isRequiredUpdate = true;
                    }
                }

                //if (singleAccount.Attributes.Contains("address2_postalcode") && singleAccount.Attributes["address2_postalcode"] != null)
                //{
                //    string actualzipCode = singleAccount.Attributes["address2_postalcode"].ToString();
                //    EntityReference actualCounty = getCountyFromZipCodeEntity(actualzipCode, service);
                //    if (actualCounty != null)
                //    {
                //        updateAccount.Attributes.Add("new_address2actual_county", new EntityReference(actualCounty.LogicalName, actualCounty.Id));
                //        isRequiredUpdate = true;
                //    }
                //}


                if(isRequiredUpdate)
                {
                    service.Update(updateAccount);
                }

                Console.WriteLine("Current Record Count : " + count);
            }
        }

        private EntityReference getCountyFromZipCodeEntity(string zipCode, IOrganizationService service)
        {
            EntityReference county = null;
            QueryExpression query = new QueryExpression("new_zipcode");
            query.ColumnSet = new ColumnSet(new string[] { "new_county" });
            query.Criteria.AddCondition("new_zipcode", ConditionOperator.Equal, zipCode);
            EntityCollection zipCodeCollection = service.RetrieveMultiple(query);
            if(zipCodeCollection != null && zipCodeCollection.Entities.Count > 0)
            {
                if (zipCodeCollection.Entities[0].Attributes.Contains("new_county") && zipCodeCollection.Entities[0].GetAttributeValue<EntityReference>("new_county") != null)                
                    county = zipCodeCollection.Entities[0].GetAttributeValue<EntityReference>("new_county");
            }
            return county;
        }
    }

    public class Account
    {
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public AccountType AccountType { get; set; }
    }
    public class AccountType
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
}
