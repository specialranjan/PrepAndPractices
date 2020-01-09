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
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Discovery;
using System.Configuration;

namespace Apttus.XAuthor.DynamicsCRMIntegration.SandBox
{
    public class SolutionAction
    {
        public bool ImportSolution()
        {
            bool _isSucceed = false;
            string SolutionDirectory = @"C:\Solution";
            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            ImportSolutionRequest SolutionRequest;
            byte[] Solution;

            string[] files = Directory.GetFiles(SolutionDirectory);
            foreach (string file in files)
            {
                Solution = File.ReadAllBytes(file);
                SolutionRequest = new ImportSolutionRequest();
                SolutionRequest.CustomizationFile = Solution;
                SolutionRequest.ImportJobId = Guid.NewGuid();
                SolutionRequest.ConvertToManaged = false;

                try
                {
                    service.Execute(SolutionRequest);
                    Entity ImportJob = new Entity("importjob");
                    ImportJob = service.Retrieve(ImportJob.LogicalName, SolutionRequest.ImportJobId, new ColumnSet(true));
                    XDocument xdoc = XDocument.Parse(ImportJob["data"].ToString());

                    string ImportSolutionName = xdoc.Descendants("solutionManifest").Descendants("UniqueName").First().Value;

                    bool SolutionImportResult = xdoc.Descendants("solutionManifest").Descendants("result").First().FirstAttribute.Value == "success" ? true : false;

                    //Guid? Solutionid = GetSDol


                }

                catch (Exception ex)
                {
                    continue;
                }



            }





            return _isSucceed;
        }

        public RetrieveAllEntitiesResponse RetrieveAllEntities()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            RetrieveAllEntitiesRequest request = new RetrieveAllEntitiesRequest()
            {
                EntityFilters = EntityFilters.Entity,
                RetrieveAsIfPublished = true
            };

            // Retrieve the MetaData
            RetrieveAllEntitiesResponse response = (RetrieveAllEntitiesResponse)service.Execute(request);
            var filter = response.EntityMetadata.Where(field => field.LogicalName == "account");
            var filterLevel1 = response.EntityMetadata.Where(data => data.IsCustomizable.Value == true).ToList();
            return response;
        }



        public void getEntityMetaData()
        {

            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            RetrieveVersionRequest versionReq = new RetrieveVersionRequest();
            RetrieveVersionResponse resp = (RetrieveVersionResponse)service.Execute(versionReq);
            //assigns the version to a string
            string VersionNumber = resp.Version;

            bool isKeyCompatibleVersion = isCompatibleVersion(VersionNumber);

            MetadataFilterExpression EntityFilter = new MetadataFilterExpression(LogicalOperator.And);
            EntityFilter.Conditions.Add(new MetadataConditionExpression("LogicalName", MetadataConditionOperator.Equals, "contact"));

            MetadataPropertiesExpression EntityProperties = new MetadataPropertiesExpression()
            {
                AllProperties = false
            };

            EntityProperties.PropertyNames.AddRange(new string[] { "Attributes", "OneToManyRelationships", "LogicalName", "DisplayName", "PrimaryIdAttribute", "PrimaryNameAttribute" });
            if (isKeyCompatibleVersion)
                EntityProperties.PropertyNames.Add("Keys");


            MetadataPropertiesExpression AttributeProperties = new MetadataPropertiesExpression() { AllProperties = false };
            AttributeProperties.PropertyNames.AddRange("AttributeType", "LogicalName", "DisplayName", "SchemaName", "AttributeType", "IsPrimaryName", "IsValidForUpdate", "OptionSet");

            EntityQueryExpression entityQueryExpression = new EntityQueryExpression()
            {
                Criteria = EntityFilter,
                Properties = EntityProperties,
                AttributeQuery = new AttributeQueryExpression()
                {
                    Properties = AttributeProperties
                }//,
                //RelationshipQuery = new RelationshipQueryExpression() { Properties = new MetadataPropertiesExpression() {  AllProperties = true} }
            };

            RetrieveMetadataChangesRequest req = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression
            };

            var response = (RetrieveMetadataChangesResponse)service.Execute(req);
        }

        private bool isCompatibleVersion(string versionNo)
        {
            List<string> majorAndMinorVersion = versionNo.Split('.').ToList();

            double majorVersion = Convert.ToDouble(majorAndMinorVersion[0] + "." + majorAndMinorVersion[1]);

            if (majorVersion > 7.0)
                return true;
            else
                return false;

        }

        public void GetAllEntity()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();




            Console.WriteLine("Starting Fetch Methods");
            DateTime startTime = DateTime.Now;
            var properties = new MetadataPropertiesExpression();
            properties.PropertyNames.AddRange("LogicalName", "DisplayName", "PrimaryIdAttribute", "PrimaryNameAttribute");

            //An entity query expression to combine the filter expressions and property expressions for the query.
            var entityQueryExpression = new EntityQueryExpression { Properties = properties, };

            //Retrieve the metadata for the query without a ClientVersionStamp
            var retrieveMetadataChangesRequest = new RetrieveMetadataChangesRequest
            {
                Query = entityQueryExpression
            };

            var response = (RetrieveMetadataChangesResponse)service.Execute(retrieveMetadataChangesRequest);

            List<string> NameList = new List<string>();
            List<string> emptyList = new List<string>();

            foreach (EntityMetadata item in response.EntityMetadata)
            {
                string name = item.DisplayName.UserLocalizedLabel != null ? item.DisplayName.UserLocalizedLabel.Label : item.SchemaName;

                NameList.Add(name);

                if (string.IsNullOrEmpty(name))
                    emptyList.Add(item.LogicalName);

            }

            NameList.Sort();

            DateTime endTime = DateTime.Now;

            Console.WriteLine("Time Taken with entityQueryExpress Method is :" + (endTime - startTime).TotalSeconds);

            startTime = DateTime.Now;

            RetrieveAllEntitiesRequest request = new RetrieveAllEntitiesRequest()
            {
                EntityFilters = EntityFilters.Entity,
                RetrieveAsIfPublished = true
            };

            // Retrieve the MetaData
            RetrieveAllEntitiesResponse response1 = (RetrieveAllEntitiesResponse)service.Execute(request);
            endTime = DateTime.Now;
            Console.WriteLine("Time Taken with RetrieveAllEntitiesRequest Method is :" + (endTime - startTime).TotalSeconds);

            foreach (EntityMetadata item in response1.EntityMetadata)
            {
                if (emptyList.Contains(item.LogicalName))
                {
                    Console.ReadLine();
                }
            }

            Console.ReadLine();

        }

        public List<RetrieveEntityResponse> RetrieveEntities(List<string> entityNames)
        {
            List<RetrieveEntityResponse> entityResponses = new List<RetrieveEntityResponse>();
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
            foreach (string entityName in entityNames)
            {
                RetrieveEntityRequest updateRequest = new RetrieveEntityRequest()
                {
                    EntityFilters = EntityFilters.Entity | EntityFilters.Attributes,
                    LogicalName = entityName,
                    RetrieveAsIfPublished = true
                };
                multipleRequest.Requests.Add(updateRequest);
            }

            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            ExecuteMultipleResponse response = (ExecuteMultipleResponse)service.Execute(multipleRequest);
            foreach (var item in response.Responses)
            {
                entityResponses.Add((RetrieveEntityResponse)item.Response);
            }
            return entityResponses;

        }

        public RetrieveEntityResponse RetrieveEntity(List<string> entityName)
        {
            RetrieveEntityResponse response1 = null;
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            foreach (string item in entityName)
            {
                RetrieveEntityRequest request = new RetrieveEntityRequest()
                {
                    EntityFilters = EntityFilters.Attributes | EntityFilters.Relationships,
                    LogicalName = item,
                    RetrieveAsIfPublished = true
                };

                RetrieveEntityResponse response = (RetrieveEntityResponse)service.Execute(request);

                //var result1 = response.EntityMetadata.Attributes.Where(f => f.RequiredLevel.Value == AttributeRequiredLevel.ApplicationRequired || f.RequiredLevel.Value == AttributeRequiredLevel.SystemRequired);
                var AttributeList = response.EntityMetadata.Attributes.OrderBy(f => f.LogicalName);


                var allFields = response.EntityMetadata.Attributes;
                var result2 = response.EntityMetadata.Attributes.Where(f => f.IsLogical == false && f.AttributeType.Value.ToString() != "Virtual" && string.IsNullOrEmpty(f.AttributeOf)
                    && (f.IsValidForCreate.Value == true || f.IsValidForUpdate.Value == true || f.IsValidForAdvancedFind.Value == true));
                var Result3 = response.EntityMetadata.Attributes.Where(f => f.IsValidForAdvancedFind.Value == false || f.AttributeType.Value == AttributeTypeCode.Virtual);

                var Result4 = response.EntityMetadata.Attributes.Where(a => a.IsValidForAdvancedFind.Value == true && a.AttributeType.Value != AttributeTypeCode.Virtual);

                var Result5 = Result4.Where(f => f.IsValidForCreate == true || f.IsValidForUpdate == true);

                var Result6 = Result4.Except(Result5).ToList();
                List<string> Level1Fields = result2.Select(f => f.LogicalName).ToList();

                //&& f.IsValidForAdvancedFind.Value == true && f.IsValidForCreate.Value ==true && f.IsValidForUpdate.Value == true);


                var virtualList = allFields.Where(f => f.AttributeType.Value == AttributeTypeCode.Virtual).ToList();
                List<string> virtualListString = virtualList.Select(f => f.LogicalName).ToList();

                var attributeof = allFields.Where(f => !string.IsNullOrEmpty(f.AttributeOf)).ToList();
                List<string> attributeOfString = attributeof.Select(f => f.LogicalName).ToList();

                var combineList = allFields.Where(f => f.AttributeType.Value == AttributeTypeCode.Virtual && !string.IsNullOrEmpty(f.AttributeOf)).ToList();
                List<string> combineListString = combineList.Select(f => f.LogicalName).ToList();

                var isRenamabled = allFields.Where(f => f.IsRenameable.Value == false).ToList();
                List<string> isrenablestring = isRenamabled.Select(f => f.LogicalName).ToList();

                var MissingFieldsfromAllFields = allFields.Except(result2).ToList();
                var MissingFieldsFromAllFields2 = allFields.Except(Result3).ToList();







                //var result3 = result2.Where(f => f.IsValidForCreate.Value == true || f.IsValidForUpdate.Value == true  || f.IsValidForAdvancedFind.Value == true);
                //List<string> Level2Fields = result3.Select(f => f.LogicalName).ToList();

                //List<string> firstMainDiff = allFields.Except(Level1Fields).ToList();
                //List<string> secondMainDiff = allFields.Except(Level2Fields).ToList();
                //List<string> FirstSecondDiff = Level1Fields.Except(Level2Fields).ToList();


            }
            return response1;
        }

        public IEnumerable<EntityMetadata> getSolutionEntities(string SolutionUniqueName)
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            // get solution components for solution unique name
            QueryExpression componentsQuery = new QueryExpression
            {
                EntityName = "solutioncomponent",
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression(),
            };
            LinkEntity solutionLink = new LinkEntity("solutioncomponent", "solution", "solutionid", "solutionid", JoinOperator.Inner);
            solutionLink.LinkCriteria = new FilterExpression();
            solutionLink.LinkCriteria.AddCondition(new ConditionExpression("uniquename", ConditionOperator.Equal, SolutionUniqueName));
            componentsQuery.LinkEntities.Add(solutionLink);
            componentsQuery.Criteria.AddCondition(new ConditionExpression("componenttype", ConditionOperator.Equal, 1));
            EntityCollection ComponentsResult = service.RetrieveMultiple(componentsQuery);
            //Get all entities
            RetrieveAllEntitiesRequest AllEntitiesrequest = new RetrieveAllEntitiesRequest()
            {
                EntityFilters = EntityFilters.Entity,
                RetrieveAsIfPublished = true
            };
            RetrieveAllEntitiesResponse AllEntitiesresponse = (RetrieveAllEntitiesResponse)service.Execute(AllEntitiesrequest);
            //Join entities Id and solution Components Id 
            var entityList = AllEntitiesresponse.EntityMetadata.Join(ComponentsResult.Entities.Select(x => x.Attributes["objectid"]), x => x.MetadataId, y => y, (x, y) => x);
            return entityList;
        }

        public void getEntityNamefromID()
        {
            Guid ID = new Guid("0AB01E00-65D4-E511-80EF-3863BB369D50");
            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            List<string> entityList = new List<string>();
            entityList.Add("contact");
            entityList.Add("account");
            entityList.Add("opportunity");
            string entityName = string.Empty;
            foreach (string item in entityList)
            {
                try
                {
                    Entity Output = service.Retrieve(item, ID, new ColumnSet());
                    entityName = Output.LogicalName;
                    return;
                }
                catch
                {
                    continue;
                }
            }
        }


        public void getEntityNamebyCode()
        {
            string entityLogicalName = String.Empty;

            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            int objectTypeCode = 10150;

            MetadataFilterExpression EntityFilter = new MetadataFilterExpression(LogicalOperator.And);
            EntityFilter.Conditions.Add(new MetadataConditionExpression("ObjectTypeCode", MetadataConditionOperator.Equals, objectTypeCode));

            MetadataPropertiesExpression mpe = new MetadataPropertiesExpression();
            mpe.AllProperties = false;
            mpe.PropertyNames.Add("DisplayName");
            mpe.PropertyNames.Add("ObjectTypeCode");
            mpe.PropertyNames.Add("PrimaryIdAttribute");
            mpe.PropertyNames.Add("PrimaryNameAttribute");

            EntityQueryExpression entityQueryExpression = new EntityQueryExpression()
            {
                Criteria = EntityFilter,
                Properties = mpe
            };



            RetrieveMetadataChangesRequest retrieveMetadataChangesRequest = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression
            };

            RetrieveMetadataChangesResponse initialRequest = (RetrieveMetadataChangesResponse)service.Execute(retrieveMetadataChangesRequest);

            if (initialRequest.EntityMetadata.Count == 1)
            {
                entityLogicalName = initialRequest.EntityMetadata[0].LogicalName;
            }

        }


        public void getOrganizationUsingDiscovery()
        {
            string serviceURL = "https://disco.crm.dynamics.com/XRMServices/2011/Discovery.svc";
            string userName = "xa-dynamics-1@apttex.com";
            string password = "x@uthor1";
            OrganizationInfo OrganizationList = GetOrganizations(serviceURL, userName, password);

        }

        public static OrganizationInfo GetOrganizations(string DiscoverServiceURL, string UserName, string Password)
        {
            ClientCredentials credentials = new ClientCredentials();
            credentials.UserName.UserName = UserName;
            credentials.UserName.Password = Password;

            using (var discoveryProxy = new DiscoveryServiceProxy(new Uri(DiscoverServiceURL), null, credentials, null))
            {
                discoveryProxy.Authenticate();

                // Get all Organizations using Discovery Service

                RetrieveOrganizationsRequest retrieveOrganizationsRequest = new RetrieveOrganizationsRequest()
                {
                    AccessType = EndpointAccessType.Default,
                    Release = OrganizationRelease.Current
                };

                RetrieveOrganizationsResponse retrieveOrganizationsResponse =
                (RetrieveOrganizationsResponse)discoveryProxy.Execute(retrieveOrganizationsRequest);

                if (retrieveOrganizationsResponse.Details.Count > 0)
                {
                    var orgs = new List<String>();
                    OrganizationInfo OrgInfo = new OrganizationInfo();
                    List<string> FriendlyName = new List<string>();
                    List<Guid> OrganizationId = new List<Guid>();
                    List<string> OrganizationVersion = new List<string>();
                    List<string> State = new List<string>();
                    List<string> UniqueName = new List<string>();
                    List<string> URlName = new List<string>();
                    List<string> WebApplication = new List<string>();
                    List<string> OrganizationService = new List<string>();
                    List<string> OrganizationDataService = new List<string>();

                    foreach (OrganizationDetail orgInfo in retrieveOrganizationsResponse.Details)
                    {
                        FriendlyName.Add(orgInfo.FriendlyName);
                        OrganizationId.Add(orgInfo.OrganizationId);
                        OrganizationVersion.Add(orgInfo.OrganizationVersion);
                        State.Add(orgInfo.State.ToString());
                        UniqueName.Add(orgInfo.UniqueName);
                        WebApplication.Add(orgInfo.Endpoints[EndpointType.WebApplication]);
                        OrganizationService.Add(orgInfo.Endpoints[EndpointType.OrganizationService]);
                        OrganizationDataService.Add(orgInfo.Endpoints[EndpointType.OrganizationDataService]);
                        URlName.Add(orgInfo.UrlName);
                    }
                    OrgInfo.FriendlyName = FriendlyName;
                    OrgInfo.OrganizationId = OrganizationId;
                    OrgInfo.OrganizationVersion = OrganizationVersion;
                    OrgInfo.State = State;
                    OrgInfo.UniqueName = UniqueName;
                    OrgInfo.WebApplication = WebApplication;
                    OrgInfo.OrganizationService = OrganizationService;
                    OrgInfo.OrganizationDataService = OrganizationDataService;
                    OrgInfo.URlName = URlName;
                    return OrgInfo;
                }
                else
                    return null;
            }
        }

        protected RetrieveMetadataChangesResponse GetMetadataChanges(EntityQueryExpression entityQueryExpression, String clientVersionStamp, DeletedMetadataFilters deletedMetadataFilter, IOrganizationService service)
        {
            RetrieveMetadataChangesRequest retrieveMetadataChangesRequest = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression,
                ClientVersionStamp = clientVersionStamp,
                DeletedMetadataFilters = deletedMetadataFilter
            };

            return (RetrieveMetadataChangesResponse)service.Execute(retrieveMetadataChangesRequest);
        }


        public class OrganizationInfo
        {
            public List<string> FriendlyName { get; set; }
            public List<Guid> OrganizationId { get; set; }
            public List<string> OrganizationVersion { get; set; }
            public List<string> State { get; set; }
            public List<string> UniqueName { get; set; }
            public List<string> URlName { get; set; }
            public List<string> WebApplication { get; set; }
            public List<string> OrganizationService { get; set; }
            public List<string> OrganizationDataService { get; set; }

        }



        //}



    }
}
