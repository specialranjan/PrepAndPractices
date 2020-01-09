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
using BusinessEntity;
using Microsoft.Crm.Sdk.Messages;

namespace Apttus.XAuthor.DynamicsCRMIntegration.SandBox
{
    public class AnnotationAction
    {
        public List<Annotation> getAllAnnotationbyObjectId(Guid ObjectId)
        {
            List<Annotation> AnnotationList = new List<Annotation>();
            QueryExpression query = new QueryExpression("annotation");
            ColumnSet cols = new ColumnSet(new string[] { "annotationid", "filename", "documentbody","subject","mimetype","notetext"});
            query.ColumnSet = cols;
            query.Criteria.AddCondition("objectid", ConditionOperator.Equal, ObjectId);
            query.Orders.Add(new OrderExpression("createdon", OrderType.Ascending));

            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            EntityCollection annotationEntityList = service.RetrieveMultiple(query);

            foreach (Entity Item in annotationEntityList.Entities)
                AnnotationList.Add(BuildEntityObject(Item));
            
            return AnnotationList;
        }

        public string getCurentUserLang()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            return getUserLanguage(service);
        }


        public void getAccountAnnotation()
        {

            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            Guid attachmentId = new Guid("1f3be793-1269-e611-80ea-5065f38af9f1");
            var columnSets = new string[] { "filename", "documentbody"};

            /* Retrive Multiple Query */
            //QueryExpression RetrieveMultiple = new QueryExpression("annotation");
            //RetrieveMultiple.ColumnSet = new ColumnSet(columnSets);
            //LinkEntity linktoAccount = new LinkEntity("annotation", "account", "objectid", "accountid", JoinOperator.Inner);
            //linktoAccount.LinkCriteria.AddCondition("name", ConditionOperator.Equal, "Choho Winery");
            //RetrieveMultiple.LinkEntities.Add(linktoAccount);

            QueryExpression RetrieveMultiple = new QueryExpression("annotation");
            RetrieveMultiple.ColumnSet = new ColumnSet(columnSets);
            RetrieveMultiple.Criteria.AddCondition("annotationid", ConditionOperator.Equal, attachmentId);
            


            var conversionRequest = new QueryExpressionToFetchXmlRequest
            {
                Query = RetrieveMultiple
            };
            var response = (QueryExpressionToFetchXmlResponse)service.Execute(conversionRequest);

            // Use the converted query to make a retrieve multiple request to Microsoft Dynamics CRM.
            String RetrieveMultipleXML = response.FetchXml;

            
            EntityCollection retrieveMulipleCollection = service.RetrieveMultiple(RetrieveMultiple);


            Console.WriteLine("Retrieve Multiple Query Result with Length : " + readLength(retrieveMulipleCollection.Entities[0]));

            /****** End Here *********/


            /*  x-Author Query */
            QueryExpression xAthorQuery = new QueryExpression()
            {
                Distinct = true,
                EntityName = "annotation",
                ColumnSet = new ColumnSet(columnSets),
                Criteria =
                {
                    Filters = 
                            { 
                                new FilterExpression
                                {
                                    FilterOperator = Microsoft.Xrm.Sdk.Query.LogicalOperator.And,
                                        Conditions = 
                                        {
                                            new ConditionExpression("annotationid", ConditionOperator.Equal, attachmentId)
                                        }
                                }
                            }
                }

            };

            conversionRequest = new QueryExpressionToFetchXmlRequest
            {
                Query = xAthorQuery
            };
            response = (QueryExpressionToFetchXmlResponse)service.Execute(conversionRequest);

            // Use the converted query to make a retrieve multiple request to Microsoft Dynamics CRM.
            RetrieveMultipleXML = response.FetchXml;


            EntityCollection xAthorQueryCollection = service.RetrieveMultiple(xAthorQuery);
            Console.WriteLine("Retrieve Multiple Query Result with Length : " + readLength(xAthorQueryCollection.Entities[0]));
            /******* End Here *******/

            /* Retrieve by Primary Key */
            Entity SingleResponse = service.Retrieve("annotation", attachmentId, new ColumnSet(columnSets));
            Console.WriteLine("Retrieve Multiple Query Result with Length : " + readLength(SingleResponse));

            Console.ReadLine();

        }

        private int readLength(Entity Item)
        {
            int length = 0;
            if(Item.Attributes.Contains("documentbody") && Item.Attributes["documentbody"] != null)
            {
                string document = Item.Attributes["documentbody"].ToString();
                length = document.Length;
            }
            return length;
        }

        public string getUserLanguage(IOrganizationService service)
        {
            string Language = string.Empty;
            Guid UserId = CRMHelper._USERID;

            QueryExpression query = new QueryExpression("usersettings");
            query.ColumnSet = new ColumnSet(new string[] { "uilanguageid" });
            EntityCollection userSettingsEntityList = service.RetrieveMultiple(query);
            if(userSettingsEntityList.Entities.Count >0)
            {
                Language = userSettingsEntityList.Entities[0].Attributes["uilanguageid"].ToString();
            }

            return Language;

        }
     
        private Annotation BuildEntityObject(Entity Data)
        {
            Annotation Item = new Annotation();
            Item.AnnotationId = Data.Id;

            if(Data.Attributes.Contains("filename") && Data.Attributes["filename"] != null)
            {
                Item.FileName = Data.Attributes["filename"].ToString();
            }
            if (Data.Attributes.Contains("documentbody") && Data.Attributes["documentbody"] != null)
            {
                Item.DocumentBody = Data.Attributes["documentbody"].ToString();
            }
            if (Data.Attributes.Contains("subject") && Data.Attributes["subject"] != null)
            {
                Item.Subject = Data.Attributes["subject"].ToString();
            }
            if (Data.Attributes.Contains("mimetype") && Data.Attributes["mimetype"] != null)
            {
                Item.MimeType = Data.Attributes["mimetype"].ToString();
            }
            if (Data.Attributes.Contains("notetext") && Data.Attributes["notetext"] != null)
            {
                Item.NoteText = Data.Attributes["notetext"].ToString();
            }
            return Item;
        }


    }
}
