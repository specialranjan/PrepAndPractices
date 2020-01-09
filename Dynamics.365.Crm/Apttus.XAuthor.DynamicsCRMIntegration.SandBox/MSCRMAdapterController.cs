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

namespace Apttus.XAuthor.DynamicsCRMIntegration.SandBox
{
    public class MSCRMAdapterController
    {
        public LoadAppResult LoadApplication(Guid uniqueId, Guid appId)
        {
            LoadAppResult appObject = null;
            if (appId == Guid.Empty)
                appId = getAppIdbyAppUniqueId(uniqueId);

            if (appId != Guid.Empty)
            {
                IOrganizationService service = CRMHelper.ConnectToMSCRM();
                QueryExpression query = new QueryExpression("annotation");
                query.ColumnSet = new ColumnSet(new string[] { "filename", "documentbody" });
                query.Criteria.AddCondition("objectid", ConditionOperator.Equal, appId);
                appObject = convertEntityToApplicationObject(service.RetrieveMultiple(query));
            }
            return appObject;
        }


        public bool saveApplication(Guid appId, Guid uniqueId, byte[] config, byte[] template, string templateName ,byte[] scheme, string edition)
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            bool _isSaveApp = true;
            int editionval = 0;
            if (appId == Guid.Empty)
                appId = getAppIdbyAppUniqueId(uniqueId);
            if(!string.IsNullOrEmpty(edition))
                editionval = getEditionOptionSetValuebyText(service, "apttus_xapps_application","apttus_xapps_edition",edition);

            if (editionval > 0)
            {
                // Update App Entity with Edition Value
                Entity appEntity = new Entity("apttus_xapps_application");
                appEntity.Id = appId;
                appEntity.Attributes.Add("apttus_xapps_edition", new OptionSetValue(editionval));
                service.Update(appEntity);
            }

            // Remove existing Attachments and Create new Attachments
            QueryExpression attachementQ = new QueryExpression("annotation");
            attachementQ.Criteria.AddCondition("objectid", ConditionOperator.Equal, appId);
            EntityCollection existingAttachmentEntitySet = service.RetrieveMultiple(attachementQ);
            foreach (Entity item in existingAttachmentEntitySet.Entities)
                service.Delete(item.LogicalName, item.Id);

            //Save ConfigFile
            if (saveAnnotation(appId, "apttus_xapps_application", "AppDefinition.xml", config, service) == Guid.Empty)
                _isSaveApp = false;
            //Save Template File
            if (saveAnnotation(appId, "apttus_xapps_application", templateName, template, service) == Guid.Empty)
                _isSaveApp = false;
            //Save Schema
            if (saveAnnotation(appId, "apttus_xapps_application", "ExternalSchema.json", scheme, service) == Guid.Empty)
                _isSaveApp = false;

            return _isSaveApp;
        }


        private Guid saveAnnotation(Guid appId, string appLogicalName, string fileName, byte[] file, IOrganizationService service)
        {
            Guid annotationId = Guid.Empty;
            Entity annotation = new Entity("annotation");
            annotation.Attributes.Add("objectid", new EntityReference(appLogicalName, appId));
            annotation.Attributes.Add("documentbody", System.Convert.ToBase64String(file));
            annotation.Attributes.Add("filename", fileName);
            annotationId= service.Create(annotation);
            return annotationId;

        }
        private Guid getAppIdbyAppUniqueId(Guid uniqueId)
        {
            Guid appId = Guid.Empty;
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            QueryExpression query = new QueryExpression("apttus_xapps_application");
            query.Criteria.AddCondition("apttus_xapps_uniqueid", ConditionOperator.Equal, uniqueId.ToString());
            EntityCollection appEntityList = service.RetrieveMultiple(query);
            if(appEntityList.Entities.Count > 0)
            {
                appId = appEntityList.Entities[0].Id;
            }
            return appId;
        }

        private LoadAppResult convertEntityToApplicationObject(EntityCollection annotationList)
        {
            LoadAppResult appObject =null;

            if(annotationList.Entities.Count > 0)
            {
                // get Template File
                appObject = new LoadAppResult();
                var filteredList =  annotationList.Entities.Where(item => item.Attributes["filename"].ToString().Contains("Template")).ToList();
                if (filteredList != null && filteredList.Count > 0)
                {
                    appObject.xlstemplate = Convert.FromBase64String(filteredList[0].Attributes["documentbody"].ToString());
                    appObject.templateName = filteredList[0].Attributes["filename"].ToString();
                }

                // get config File                
                filteredList = annotationList.Entities.Where(item => item.Attributes["filename"].ToString().Contains("AppDefinition")).ToList();
                if (filteredList != null && filteredList.Count > 0)                
                    appObject.config= Convert.FromBase64String(filteredList[0].Attributes["documentbody"].ToString());

                // get schema File
                filteredList = annotationList.Entities.Where(item => item.Attributes["filename"].ToString().Contains("ExternalSchema")).ToList();
                if(filteredList != null && filteredList.Count > 0)                
                    appObject.schema = Convert.FromBase64String(filteredList[0].Attributes["documentbody"].ToString());
            }
            return appObject;
        }

        private int getEditionOptionSetValuebyText(IOrganizationService service, string EntityName, string optionSetName, string SearchText)
        {
            int value = 0;
            OptionSetMetadata optionSetValues = getOptionSetValue(EntityName, optionSetName, service);
            foreach (OptionMetadata optionMetadata in optionSetValues.Options)
            {
                if (optionMetadata.Label.UserLocalizedLabel.Label.ToLower() == SearchText.ToLower())
                {
                    value = optionMetadata.Value.Value;
                    break;
                }

            }
            return value;
        }


        private OptionSetMetadata getOptionSetValue(string entityName, string attributeName, IOrganizationService service)
        {            
            RetrieveAttributeRequest retrieveAttributeRequest = new RetrieveAttributeRequest();
            retrieveAttributeRequest.EntityLogicalName = entityName;
            retrieveAttributeRequest.LogicalName = attributeName;
            retrieveAttributeRequest.RetrieveAsIfPublished = true;

            RetrieveAttributeResponse retrieveAttributeResponse =
              (RetrieveAttributeResponse)service.Execute(retrieveAttributeRequest);
            PicklistAttributeMetadata picklistAttributeMetadata =
              (PicklistAttributeMetadata)retrieveAttributeResponse.AttributeMetadata;

            OptionSetMetadata optionsetMetadata = picklistAttributeMetadata.OptionSet;

            return optionsetMetadata;           
            
        }
    }

    public class LoadAppResult
    {
        public byte[] config { get; set; }
        public byte[] xlstemplate { get; set; }
        public byte[] schema { get; set; }
        public string templateName { get; set; }
    }

    public class SaveAppRequest
    {
        public byte[] config { get; set; }
        public byte[] xlstemplate { get; set; }
        public byte[] schema { get; set; }
        public string templateName { get; set; }
        public Guid uniqueId { get; set; }
        public Guid appId { get; set; }
             
    }

}
