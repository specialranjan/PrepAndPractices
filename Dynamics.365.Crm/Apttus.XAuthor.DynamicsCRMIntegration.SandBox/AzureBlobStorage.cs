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
    public class AzureBlobStorage
    {
        public const string AZURE_STORAGE_ACCOUNT = "ankitmscrm";
        public const string AZURE_STORAGE_CONTAINER = "crmannotation";
        //public const string AZURE_STORAGE_KEY = "2ua2JpcTfhCaskRTamdcIBZ47goBw2sP/Cvk7pFoyN3NzeV022jDQec9d1zvrlTYQrwwxWS3WtihXluV0pzzBA==";             
        public const string AZURE_STORAGE_KEY = "2ua2JpcTfhCaskRTamdcIBZ47goBw2sP/Cvk7pFoyN3NzeV022jDQec9d1zvrlTYQrwwxWS3WtihXluV0pzzBA==";             


        public void pushAccountAttachmentToAzureBlob()
        {
            Guid accountId = new Guid("78C36B5E-9473-E511-80E4-3863BB348EE8");
            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            //Get Document Attachment from Annoation associated with this Account

            QueryExpression query = new QueryExpression("annotation");
            query.ColumnSet = new ColumnSet(new string[] { "filename", "documentbody"});
            query.Criteria.AddCondition("isdocument", ConditionOperator.Equal, true);
            query.Criteria.AddCondition("objectid", ConditionOperator.Equal, accountId);

            EntityCollection attachmentList = service.RetrieveMultiple(query);

            BlobHelper blobHeloer = new BlobHelper(AZURE_STORAGE_ACCOUNT, AZURE_STORAGE_KEY);

            foreach(Entity attachment in attachmentList.Entities)
            {
                string documentBody = attachment.Attributes["documentbody"].ToString();
                string fileName = attachment.Attributes["filename"].ToString();

                blobHeloer.PutBlob(AZURE_STORAGE_CONTAINER, fileName, documentBody);
            }

            

        }
    }


}
