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

namespace Apttus.XAuthor.DynamicsCRMIntegration.SandBox
{
    public class BulkRequestUpdate
    {
        public EntityCollection getAllProjects(IOrganizationService service)
        {
            EntityCollection List = new EntityCollection();
            QueryExpression query = new QueryExpression("apttex_project");
            query.ColumnSet = new ColumnSet(new string[] { "apttex_name" });
            List = service.RetrieveMultiple(query);
            return List;
        }

        public EntityCollection getAllEvents(IOrganizationService service)
        {
            EntityCollection List = new EntityCollection();
            QueryExpression query = new QueryExpression("apttex_event");
            query.ColumnSet = new ColumnSet(new string[] { "apttex_name" });
            List = service.RetrieveMultiple(query);
            return List;
        }
        public EntityCollection getAllEmployee(IOrganizationService service)
        {
            EntityCollection List = new EntityCollection();
            QueryExpression query = new QueryExpression("new_employee");
            query.ColumnSet = new ColumnSet(new string[] { "new_name" });
            query.TopCount = 1000;
            List = service.RetrieveMultiple(query);
            return List;
        }



        public void BulkUpsertRequest()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            //var multipleRequest = new ExecuteMultipleRequest()
            //{
            //    // Assign settings that define execution behavior: continue on error, return responses. 
            //    Settings = new ExecuteMultipleSettings()
            //    {
            //        ContinueOnError = false,
            //        ReturnResponses = true
            //    },
            //    // Create an empty organization request collection.
            //    Requests = new OrganizationRequestCollection()
            //};

            //string accountKey = "accountnumber";
            //string keyValue = string.Empty;

            //// First Account Record
            //keyValue = "123";
            //Entity Account = new Entity("account", accountKey, "123");
            //Account.Attributes.Add("name", keyValue);

            //UpsertRequest upReq = new UpsertRequest();
            //upReq.Target = Account;
            //multipleRequest.Requests.Add(upReq);

            //// Second Account Record
            //keyValue = "456";
            //Account = new Entity("account", accountKey, keyValue);
            //Account.Attributes.Add("name", keyValue);

            //upReq = new UpsertRequest();
            //upReq.Target = Account;
            //multipleRequest.Requests.Add(upReq);

            //// Third Account Record
            //keyValue = "789";
            //Account = new Entity("account", accountKey, keyValue);
            //Account.Attributes.Add("name", keyValue);

            //// Third Account Record
            //keyValue = "7890";
            //Account = new Entity("account", accountKey, keyValue);
            //Account.Attributes.Add("name", keyValue);

            //upReq = new UpsertRequest();
            //upReq.Target = Account;
            //multipleRequest.Requests.Add(upReq);
            //ExecuteMultipleResponse multipleResponse = (ExecuteMultipleResponse)service.Execute(multipleRequest);

            
            EntityKeyMetadata Data = new EntityKeyMetadata();

            Data.KeyAttributes = new string[] { "apttus_contactnumber" };
            Data.DisplayName = new Label("Contact Number", 1033);
            Data.LogicalName = "apttus_contactnumber";
            Data.SchemaName = "apttus_telephone1";

            CreateEntityKeyRequest request1 = new CreateEntityKeyRequest()
            {
                EntityKey = Data,
                EntityName = "contact",
                
            };
           CreateEntityKeyResponse response = (CreateEntityKeyResponse)service.Execute(request1);

        }

        public void UpdateRecordwithBunchRequest(EntityCollection UpdateEntityList, IOrganizationService service, string nameToPrint, int bunchLimit)
        {
            try
            {
                List<UpdateRequest> UpdateList = new List<UpdateRequest>();
                ExecuteMultipleRequest MultiRequest = null;
                List<ExecuteMultipleRequest> MultirequestList = new List<ExecuteMultipleRequest>();
                for (int count = 0; count < UpdateEntityList.Entities.Count; count++)
                {
                    Entity Item = UpdateEntityList.Entities[count];
                    if (MultiRequest == null)
                        MultiRequest = getNewObject();
                    UpdateRequest updateRequest = new UpdateRequest { Target = Item };
                    MultiRequest.Requests.Add(updateRequest);

                    if ((count + 1) % bunchLimit == 0)
                    {
                        MultirequestList.Add(MultiRequest);
                        MultiRequest = getNewObject();
                    }
                    if ((count == UpdateEntityList.Entities.Count - 1) && ((count + 1) % bunchLimit != 0))
                        MultirequestList.Add(MultiRequest);
                }
                Console.WriteLine("Prepare List of " + bunchLimit + " Records Bunch for : " + nameToPrint + " With List Count :" + MultirequestList.Count);
                ExecuteMultipleResponse Response = null;
                DateTime startTime = DateTime.Now;
                Console.WriteLine("Starting Bulk Update Request for " + nameToPrint + " at :  " + startTime.ToString());
                for (int index = 0; index < MultirequestList.Count; index++)
                {

                    ExecuteMultipleResponse multipleResponse = (ExecuteMultipleResponse)service.Execute(MultirequestList[index]);
                    if (Response == null)
                    {
                        Response = new ExecuteMultipleResponse();
                        Response = multipleResponse;
                    }
                    else
                    {
                        Response.Responses.AddRange(multipleResponse.Responses);
                    }
                }
                DateTime endTime = DateTime.Now;
                double timeDiff = (endTime - startTime).TotalSeconds;
                Console.WriteLine("Completed Bulk Update Request for " + nameToPrint + " at : " + endTime.ToString());
                Console.WriteLine("Bulk Operation Took Total Seconds : " + timeDiff.ToString());
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        private ExecuteMultipleRequest getNewObject()
        {
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
            return multipleRequest;
        }

        private void CreateAlternateKey(string entityName, List<string> entityFieldName, string keyDisplayName, string keyLogicalName)
        {
            EntityKeyMetadata Data = new EntityKeyMetadata();
            Data.KeyAttributes = entityFieldName.ToArray();
            Data.DisplayName = new Label(keyDisplayName, 1033);
            Data.LogicalName = keyLogicalName;
            Data.SchemaName = keyLogicalName;

            CreateEntityKeyRequest request1 = new CreateEntityKeyRequest()
            {
                EntityKey = Data,
                EntityName = entityName,

            };

            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            CreateEntityKeyResponse response = (CreateEntityKeyResponse)service.Execute(request1);
        }

    }
}
