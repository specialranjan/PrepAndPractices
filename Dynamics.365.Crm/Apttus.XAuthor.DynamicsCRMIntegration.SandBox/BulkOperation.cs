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
    public class BulkOperation
    {

        public void DeleteCompletedJobs()
        {
            DateTime DateFilter = DateTime.Now.AddDays(-1);
            List<string> statusList = new List<string>();
            statusList.Add("1");
            statusList.Add("3");
            QueryExpression query = new QueryExpression("asyncoperation");
            query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 3);
            query.Criteria.AddCondition("createdon", ConditionOperator.OnOrBefore, DateFilter);
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            Console.WriteLine("IOrganization Service Object Created");
            EntityCollection EntityList = CRMHelper.RetrieveMultiple(query, service);
            int count = 1;
            ExecuteMultipleRequest mulitRequest = null;

            Console.WriteLine("Total Record to be Deleted are : " + EntityList.Entities.Count);
            for (int i = 0; i < EntityList.Entities.Count; i++)
            {
                if (i == 1 || i % 1000 == 0)
                    mulitRequest = getNewObject();
                Entity Item = EntityList.Entities[i];
                DeleteRequest deleteRequest = new DeleteRequest { Target = Item.ToEntityReference() };
                mulitRequest.Requests.Add(deleteRequest);

                if (count % 200 == 0 || count == EntityList.Entities.Count)
                {
                    // Execute all the requests in the request collection using a single web method call.
                    ExecuteMultipleResponse multipleResponse = (ExecuteMultipleResponse)service.Execute(mulitRequest);
                    Console.WriteLine("Bunch of 200 Record Deleted  & Total record Proceed are :" + count);
                }


                count++;
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

        public static void BulkDelete(IOrganizationService service, DataCollection<EntityReference> entityReferences)
        {
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

            // Add a DeleteRequest for each entity to the request collection.
            foreach (var entityRef in entityReferences)
            {
                DeleteRequest deleteRequest = new DeleteRequest { Target = entityRef };
                multipleRequest.Requests.Add(deleteRequest);
            }


        }


        public void DeleteWaitingJobs()
        {
            DateTime DateFilter = DateTime.Now.AddDays(-1);
            List<string> statusList = new List<string>();
            statusList.Add("1");
            statusList.Add("3");
            QueryExpression query = new QueryExpression("asyncoperation");
            query.Criteria.AddCondition("statuscode", ConditionOperator.NotEqual, 30);
            query.Criteria.AddCondition("createdon", ConditionOperator.OnOrBefore, DateFilter);
            query.Criteria.AddCondition("operationtype", ConditionOperator.Equal, 10);
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            Console.WriteLine("IOrganization Service Object Created");
            EntityCollection EntityList = CRMHelper.RetrieveMultiple(query, service);
            int count = 1;
            ExecuteMultipleRequest mulitRequest = null;
            Console.WriteLine("Total Record to be Deleted are : " + EntityList.Entities.Count);
            for (int i = 0; i < EntityList.Entities.Count; i++)
            {
                if (i == 1 || i % 1000 == 0)
                    mulitRequest = getNewObject();
                Entity Item = EntityList.Entities[i];
                DeleteRequest deleteRequest = new DeleteRequest { Target = Item.ToEntityReference() };
                mulitRequest.Requests.Add(deleteRequest);

                if (count % 1000 == 0 || count == EntityList.Entities.Count)
                {
                    // Execute all the requests in the request collection using a single web method call.
                    ExecuteMultipleResponse multipleResponse = (ExecuteMultipleResponse)service.Execute(mulitRequest);
                    Console.WriteLine("Bunch of 1000 Record Deleted  & Total record Proceed are :" + count);
                }


                count++;
            }
        }


        public void getAttachmentSize()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            QueryExpression query = new QueryExpression("email");
            Console.WriteLine("IOrganization Service Object Created");
            EntityCollection EntityList = CRMHelper.RetrieveMultiple(query, service);

            List<int> FileSizeList = new List<int>();
            foreach (Entity Item in EntityList.Entities)
            {
                if (Item.Attributes.Contains("filesize"))
                {
                    FileSizeList.Add(Convert.ToInt32(Item["filesize"]));
                }
            }

            var TotalSize = 0;
            foreach (int Item in FileSizeList)
                TotalSize = TotalSize + Item;

            var SizeKB = TotalSize / 1024;
            var SizeMB = SizeKB / 1024;
            var SizeGB = SizeMB / 1024;



        }


        public void getEmailAttachment()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            QueryExpression query = new QueryExpression("activitymimeattachment");
            query.ColumnSet = new ColumnSet(new string[] { "filesize" });
            Console.WriteLine("IOrganization Service Object Created");
            EntityCollection EntityList = CRMHelper.RetrieveMultiple(query, service);

            List<int> FileSizeList = new List<int>();
            foreach (Entity Item in EntityList.Entities)
            {
                if (Item.Attributes.Contains("filesize"))
                {
                    FileSizeList.Add(Convert.ToInt32(Item["filesize"]));
                }
            }

            var TotalSize = 0;
            foreach (int Item in FileSizeList)
                TotalSize = TotalSize + Item;

            var SizeKB = TotalSize / 1024;
            var SizeMB = SizeKB / 1024;
            var SizeGB = SizeMB / 1024;



        }

        public void getAnnotationAttachment()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            QueryExpression query = new QueryExpression("annotation");
            query.ColumnSet = new ColumnSet(new string[] { "filesize" });
            query.Criteria.AddCondition("filesize", ConditionOperator.GreaterThan, 0);

            EntityCollection List = CRMHelper.RetrieveMultiple(query, service);

            decimal totalSize = 0;

            foreach (Entity Item in List.Entities)
            {
                decimal filesize = Convert.ToDecimal(Item.Attributes["filesize"].ToString());
                totalSize = totalSize + filesize;
            }

            Console.WriteLine("Total File Size :" + totalSize);
            decimal inKB = totalSize / 1024;
            decimal inMB = inKB / 1024;
            decimal inGB = inMB / 1024;
            Console.ReadLine();

        }

        public ExecuteMultipleResponse InsertBulkDataOnebyOne(int counter)
        {
            EntityCollection toInsert = new EntityCollection();

            for (int i = 1; i <= counter; i++)
            {
                Entity Task = new Entity("task");
                Task.Attributes["subject"] = "Task : " + i.ToString();
                toInsert.Entities.Add(Task);
            }

            IOrganizationService orgServiceProxy = CRMHelper.ConnectToMSCRM();
            ExecuteMultipleResponse responseList = null;
            DateTime StartTime = DateTime.Now;
            Console.WriteLine("Start One by One Process at : " + StartTime);
            int count = 0;
            foreach (var entity in toInsert.Entities)
            {

                // Create an ExecuteMultipleRequest object.
                ExecuteMultipleRequest requestWithResults = new ExecuteMultipleRequest()
                {
                    // Assign settings that define execution behavior: continue on error, return responses. 
                    Settings = new ExecuteMultipleSettings()
                    {
                        ContinueOnError = true,
                        ReturnResponses = true
                    },
                    // Create an empty organization request collection.
                    Requests = new OrganizationRequestCollection()
                };

                if (count == 0)
                {
                    CreateRequest createRequest = new CreateRequest { Target = entity };
                    requestWithResults.Requests.Add(createRequest);
                }
                count++;
                if (responseList == null)
                {
                    ExecuteMultipleResponse responseWithResults = (ExecuteMultipleResponse)orgServiceProxy.Execute(requestWithResults);
                    responseList = responseWithResults;
                }
                else
                {
                    ExecuteMultipleResponseItem item = new ExecuteMultipleResponseItem();
                    try
                    {
                        Guid recordId = orgServiceProxy.Create(entity);
                        item.Fault = null;
                        item.Response = new OrganizationResponse();

                        item.Response.Results = new ParameterCollection();
                        item.Response.Results.Add("id", recordId);
                    }
                    catch (Exception ex)
                    {
                        item.Fault.Message = ex.Message;
                        continue;
                    }
                    responseList.Responses.Add(item);
                }

            }
            DateTime endTime = DateTime.Now;
            Console.WriteLine("Completed One by One Process at : " + endTime);
            var diffInSeconds = (endTime - StartTime).TotalSeconds;
            Console.WriteLine("Total Time in Second : " + diffInSeconds);
            return responseList;
        }

        public ExecuteMultipleResponse InsertBulkAllTogather(int counter)
        {
            EntityCollection toInsert = new EntityCollection();

            for (int i = 1; i <= counter; i++)
            {
                Entity Task = new Entity("task");
                Task.Attributes["subject"] = "Task : " + i.ToString();
                toInsert.Entities.Add(Task);
            }

            IOrganizationService orgServiceProxy = CRMHelper.ConnectToMSCRM();
            ExecuteMultipleResponse responseList = null;
            DateTime StartTime = DateTime.Now;
            Console.WriteLine("Start One by One Process at : " + StartTime);
            ExecuteMultipleRequest requestWithResults = new ExecuteMultipleRequest()
            {
                // Assign settings that define execution behavior: continue on error, return responses. 
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                },
                // Create an empty organization request collection.
                Requests = new OrganizationRequestCollection()
            };

            int count = 0;
            foreach (var entity in toInsert.Entities)
            {
                // Create an ExecuteMultipleRequest object.
                CreateRequest createRequest = new CreateRequest { Target = entity };
                requestWithResults.Requests.Add(createRequest);

            }
            ExecuteMultipleResponse responseWithResults = (ExecuteMultipleResponse)orgServiceProxy.Execute(requestWithResults);
            responseList = responseWithResults;
            DateTime endTime = DateTime.Now;
            Console.WriteLine("Completed One by One Process at : " + endTime);
            var diffInSeconds = (endTime - StartTime).TotalSeconds;
            Console.WriteLine("Total Time in Second : " + diffInSeconds);
            return responseList;
        }

    }
}
