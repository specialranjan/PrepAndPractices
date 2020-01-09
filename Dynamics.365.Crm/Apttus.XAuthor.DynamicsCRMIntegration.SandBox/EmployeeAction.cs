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
using Microsoft.Crm.Sdk.Messages;
using System.IO;
using System.Xml.Linq;

namespace Apttus.XAuthor.DynamicsCRMIntegration.SandBox
{
    public class EmployeeAction
    {

        public EntityCollection getAllEmployeeWithLoyalti()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            QueryExpression query = new QueryExpression("new_employee");
            query.Criteria.AddCondition("new_loyalti", ConditionOperator.GreaterThan, 0);
            query.ColumnSet = new ColumnSet(true);
            EntityCollection EmployeeEntityList = service.RetrieveMultiple(query);
            return EmployeeEntityList;
        }

        public void UpdateEmployeeLoyalti(Guid EmpployeeId)
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            Entity employee = new Entity("new_employee");
            employee.Id = EmpployeeId;
            employee.Attributes.Add("new_loyalti", 60);
            service.Update(employee);
        }

        public void updateAllEmployeeonyByOne()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            Console.WriteLine("update All Employee Started");
            DateTime StartGetTime = DateTime.Now;

            Console.WriteLine("Start Method of All Employee :" + StartGetTime.ToString());
            EntityCollection List = getAllEmployee();
            DateTime EndGetTime = DateTime.Now;
            Console.WriteLine("End Method of All Employee at :" + EndGetTime.ToString());
            double DiffinSeconds = (EndGetTime - StartGetTime).TotalSeconds;
            Console.WriteLine("Method Took Time in Second:" + DiffinSeconds.ToString());
            StartGetTime = DateTime.Now;
            Console.WriteLine("Starting Updating First 500 Records" + StartGetTime.ToString());
            for (int cnt = 0; cnt < List.Entities.Count; cnt++)
            {
                if (cnt == 499)
                    break;
                Entity Item = new Entity("new_employee");
                Item.Id = List.Entities[cnt].Id;
                Item.Attributes.Add("new_jobtitle", "Associate Engineer");
                service.Update(Item);
            }
            EndGetTime = DateTime.Now;
            Console.WriteLine("Completed Updating First 500 Records at : " + EndGetTime.ToString());
            double DiffinSecond = (EndGetTime - StartGetTime).TotalSeconds;
            Console.WriteLine("Total Operation Time in Seconds : " + DiffinSecond.ToString());
            Console.ReadLine();
        }

        public void updateAllEmployeeWithBulkRequest()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            Console.WriteLine("update All Employee Started");
            DateTime StartGetTime = DateTime.Now;

            Console.WriteLine("Start Method of All Employee :" + StartGetTime.ToString());
            EntityCollection List = getAllEmployee();
            DateTime EndGetTime = DateTime.Now;
            Console.WriteLine("End Method of All Employee at :" + EndGetTime.ToString());
            double DiffinSeconds = (EndGetTime - StartGetTime).TotalSeconds;
            Console.WriteLine("Method Took Time in Second:" + DiffinSeconds.ToString());
            StartGetTime = DateTime.Now;
            Console.WriteLine("Prepare Entity Collection to Update 500 Records" + StartGetTime.ToString());
            EntityCollection toUpdate = new EntityCollection();
            for (int cnt = 0; cnt < List.Entities.Count; cnt++)
            {
                if (cnt == 499)
                    break;
                Entity Item = new Entity("new_employee");
                Item.Id = List.Entities[cnt].Id;
                Item.Attributes.Add("new_jobtitle", "CRM Developer");
                toUpdate.Entities.Add(Item);
            }
            EndGetTime = DateTime.Now;
            double DiffinSecond = (EndGetTime - StartGetTime).TotalSeconds;
            Console.WriteLine("Completed Preparing Entity Collection to Update Request at :" + EndGetTime.ToString());

            // Create an ExecuteMultipleRequest object.
            
            var multipleRequest = new ExecuteMultipleRequest()
            {
                // Assign settings that define execution behavior: continue on error, return responses. 
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = false,
                    ReturnResponses = true
                },
                // Create an empty organization request collection.
                Requests = new OrganizationRequestCollection()
            };

            // Add a UpdateRequest for each entity to the request collection.
            foreach (var entity in toUpdate.Entities)
            {
                UpdateRequest updateRequest = new UpdateRequest { Target = entity };
                multipleRequest.Requests.Add(updateRequest);
            }

            // Execute all the requests in the request collection using a single web method call.
            ExecuteMultipleResponse multipleResponse = (ExecuteMultipleResponse)service.Execute(multipleRequest);
            EndGetTime = DateTime.Now;
            DiffinSecond = (EndGetTime - StartGetTime).TotalSeconds;
            Console.WriteLine("Completed Updating 500 Records at : " + EndGetTime.ToString());
            Console.WriteLine("Total Operation Time in Second is :" + DiffinSecond);
            Console.ReadLine();
        }


        public Guid addNewEmployee()
        {
            Guid empId = Guid.Empty;
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            Entity employee = new Entity("new_employee");
            employee.Attributes.Add("new_name", "Ankit Rahevar");
            employee.Attributes.Add("new_loyalti", 40);
            empId = service.Create(employee);
            return empId;


        }


        public void deleteQuote()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            Entity quote = new Entity("quote");
            quote.Id = new Guid("B2960DD9-00C5-E511-80ED-3863BB346A70");
            quote.Attributes.Add("apttus_abandonquote", true);
            service.Update(quote);
        }


        public void retrieveAuditData()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            // Retrieve the audit history for the account and display it.
            RetrieveRecordChangeHistoryRequest changeRequest = new RetrieveRecordChangeHistoryRequest();
            changeRequest.Target = new EntityReference("account", new Guid("A9B96053-43B1-E111-A892-1CC1DEEAE7D7"));

            RetrieveRecordChangeHistoryResponse changeResponse =
                (RetrieveRecordChangeHistoryResponse)service.Execute(changeRequest);

            AuditDetailCollection details = changeResponse.AuditDetailCollection;

        }


        public void getuserbyUserId()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            Entity user = service.Retrieve("systemuser", new Guid("34dbba6e-c858-e111-b538-1cc1deeae7d7"), new ColumnSet(true));


        }

        public EntityCollection getAllEmployee()
        {
            QueryExpression query = new QueryExpression("new_employee");
            query.ColumnSet = new ColumnSet(new string[] { "new_name", "new_loyalti", "new_jobtitle", "new_grade" });
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            EntityCollection List = CRMHelper.RetrieveMultiple(query, service);
            return List;
        }
    }



}
