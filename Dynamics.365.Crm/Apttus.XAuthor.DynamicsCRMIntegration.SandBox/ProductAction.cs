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
    public class ProductAction
    {

        public void updateAllProductToActive()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            QueryExpression query = new QueryExpression("product");
            query.ColumnSet = new ColumnSet(new string[] {"name" });
            query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 2);
            query.Criteria.AddCondition("createdon", ConditionOperator.Yesterday);
            EntityCollection productentityList = service.RetrieveMultiple(query);

            Console.WriteLine("Total Product to be Procceed are : " + productentityList.Entities.Count);
            int counter = 0;
            foreach(Entity Item in productentityList.Entities)
            {
                SetStateRequest publishRequest = new SetStateRequest
                {
                    EntityMoniker = new EntityReference(Item.LogicalName, Item.Id),
                    State = new OptionSetValue(0),
                    Status = new OptionSetValue(1)
                };
                service.Execute(publishRequest);
                counter++;

                if (counter % 100 == 0)
                    Console.WriteLine("Total Product Updated are : " + counter);
               
            }
        }

        public void getAllProducts()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            QueryExpression query = new QueryExpression("product");
            query.TopCount = 20;
            EntityCollection List = service.RetrieveMultiple(query); 

        }
    }
}
