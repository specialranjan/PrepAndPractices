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
using Microsoft.Crm.Sdk.Messages;
namespace Apttus.XAuthor.DynamicsCRMIntegration.SandBox
{
    class ZipCodeAction
    {
        public EntityCollection getAllUSZipCode(IOrganizationService service)
        {
            

            QueryExpression query = new QueryExpression("new_zipcode");
            query.ColumnSet = new ColumnSet(new string[] { "new_zipcode","new_country"});
            FilterExpression feOr = new FilterExpression(LogicalOperator.Or);
            for (char c = 'a'; c <= 'z' ; c++)
            {
                feOr.AddCondition("new_zipcode", ConditionOperator.BeginsWith, c.ToString()); 
            }

            FilterExpression feAnd = new FilterExpression(LogicalOperator.And);
            feAnd.AddCondition("new_country", ConditionOperator.Equal,"US");
            query.Criteria.AddFilter(feAnd);
            query.Criteria.AddFilter(feOr);
            EntityCollection zipCodeList = CRMHelper.RetrieveMultiple(query, service);
            return zipCodeList;
        }


        public void updateUSZipCode()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM1();
            EntityCollection usZipCodeList = getAllUSZipCode(service);

            
            Console.WriteLine("Total Zip Code to Proceed are : " + usZipCodeList.Entities.Count);

            EntityCollection newEntityCollection = new EntityCollection();
            foreach(Entity item in usZipCodeList.Entities)
            {
                newEntityCollection.Entities.Add(item);
                //if(item.Attributes.Contains("new_country") && item.Attributes["new_country"] != null)
                //{

                //}
                //else
                //{
                //    newEntityCollection.Entities.Add(item);
                //}
            }

            Console.WriteLine("Total Records to Proceed : " + newEntityCollection.Entities.Count);
            foreach(Entity item in newEntityCollection.Entities)
            {
                Entity newEntity = new Entity(item.LogicalName);
                newEntity.Id = item.Id;
                newEntity.Attributes.Add("new_country", "CA");
                service.Update(newEntity);
            }

            //ExecuteMultipleRequest mulitRequest = null;
            //for (int count = 0; count < newEntityCollection.Entities.Count; count++)
            //{
            //    if(count == 0 || count % 50 == 0)
            //        mulitRequest = getNewObject();


            //    Entity Item = newEntityCollection.Entities[count];
            //    Entity newEntity = new Entity(Item.LogicalName);
            //    newEntity.Id = Item.Id;
            //    newEntity.Attributes.Add("new_country", "CA");
            //    UpdateRequest updateRequest = new UpdateRequest { Target = newEntity };
            //    mulitRequest.Requests.Add(updateRequest);

            //    if ((count + 1) % 50 == 0 || count == newEntityCollection.Entities.Count)
            //    {
            //        // Execute all the requests in the request collection using a single web method call.
            //        ExecuteMultipleResponse multipleResponse = (ExecuteMultipleResponse)service.Execute(mulitRequest);
            //        Console.WriteLine("Bunch of 1000 Record Deleted  & Total record Proceed are :" + count);
            //    }

            //}

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

    }
}
