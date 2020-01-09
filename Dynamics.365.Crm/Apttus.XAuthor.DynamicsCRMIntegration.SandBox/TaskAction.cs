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
    public class TaskAction
    {
        public void getAllTaskByAccountId()
        {
            Guid AccountId = new Guid("64E065F0-D8DE-E511-80F0-3863BB346A70");
            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            QueryExpression query = new QueryExpression("task");
            query.ColumnSet = new ColumnSet(true);
            query.Criteria.AddCondition("regardingobjectid", ConditionOperator.Equal, AccountId);
            EntityCollection TaskList = service.RetrieveMultiple(query);
                

        }

        public void getAllTaskByContactId()
        {
            Guid ContactId = new Guid("B6F08E3E-3DBF-E511-80ED-3863BB369D50");
            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            QueryExpression query = new QueryExpression("task");
            query.ColumnSet = new ColumnSet(true);
            query.Criteria.AddCondition("regardingobjectid", ConditionOperator.Equal, ContactId);
            EntityCollection TaskList = service.RetrieveMultiple(query);
        }

        public void ZipCodeActions()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM1();

            // Retrieval
            QueryExpression query = new QueryExpression("new_zipcode");
            query.TopCount = 10;
            query.ColumnSet = new ColumnSet(new string[] { "new_state", "new_city" });

            EntityCollection coll = service.RetrieveMultiple(query);

            //Update
            Entity UpdateItem = new Entity("new_zipcode");
            UpdateItem.Id = coll.Entities[0].Id;
            UpdateItem.Attributes.Add("new_state", coll.Entities[0].Attributes["new_state"].ToString());
            service.Update(UpdateItem);

            //Create
            Entity AddItem = new Entity("new_zipcode");
            AddItem.Attributes.Add("new_state", "LA");
            service.Create(AddItem);


        }
    }
}
