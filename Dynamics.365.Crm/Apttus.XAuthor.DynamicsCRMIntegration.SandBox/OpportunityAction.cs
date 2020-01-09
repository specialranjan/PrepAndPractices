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
    public class OpportunityAction
    {

        public void getOpportunityMetaData()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            RetrieveEntityRequest req = new RetrieveEntityRequest()
            {
                EntityFilters = EntityFilters.All,                
                LogicalName = "contact",
                RetrieveAsIfPublished = true
            };

            RetrieveEntityResponse res = (RetrieveEntityResponse)service.Execute(req);
            var result = res.EntityMetadata.Attributes.Where(a => a.IsLogical == false).Where(aa => aa.AttributeType.Value.ToString() != "Virtual").ToList();

        }


        public void updateOpportunityState()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            Guid oppId = new Guid("3A399DB7-21C5-E511-80ED-3863BB346A70");

            Entity oppEntity = new Entity("opportunityclose");
            oppEntity.Attributes.Add("opportunityid", new EntityReference("opportunity", oppId));

            WinOpportunityRequest winOppReq = new WinOpportunityRequest();
            winOppReq.OpportunityClose = oppEntity;
            winOppReq.Status = new OptionSetValue(3);

            WinOpportunityResponse winOppResp = (WinOpportunityResponse)service.Execute(winOppReq);
        }

        public void changeOppOwner()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            Guid oppId = new Guid("F2B3D5A9-F5C4-E511-80ED-3863BB346A70");
            Entity oppEntity = new Entity("opportunity");
            oppEntity.Attributes.Add("ownerid", new EntityReference("systemuser", new Guid("0CEAF899-6D01-4577-80DF-0EDEBCE57570")));
            oppEntity.Id = oppId;
            service.Update(oppEntity);
        }

        public void CreateQuoteFromOpp()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            Guid oppId = new Guid("B1488A7A-3FEC-E511-8102-3863BB3660F0");

            /*****Step 1: Create Quote with Line Item from Opportunity***********/
            var genQuoteFromOppRequest = new GenerateQuoteFromOpportunityRequest
            {
                OpportunityId = oppId,
                ColumnSet = new ColumnSet(CONSTANT.QUOTE_ID_FIELD, CONSTANT.QUOTE_NAME_FIELD)
            };

            var genQuoteFromOppResponse = (GenerateQuoteFromOpportunityResponse)
                service.Execute(genQuoteFromOppRequest);
            Guid quoteId = genQuoteFromOppResponse.Entity.Id;


           

            /********** Step 3: Set Current Quote as Primary and reset all older quote with Primary as false*********/
            setCurrentQuotAsPrimary(service, oppId, quoteId);

            /**********Step 4 : Create Product Configuration From Quote*********************/
            createProductConfigFromQuote(service, quoteId);

            /****************Step 5 : Update Opportunity with Quote Created********************/
            // Update Opporunity
            Entity opportunity = new Entity(CONSTANT.OPPORTUNITY_ENTITY);
            opportunity.Id = oppId;

            // Reset Trigger Flag to Flalse
            opportunity.Attributes.Add(CONSTANT.OPPORTUNITY_TRIGGERED_ACTION_FIELD, false);
            // Set above created quote to use further
            opportunity.Attributes.Add(CONSTANT.OPPORTUNITY_TRIGGERED_QUOTE_FIELD, new EntityReference(CONSTANT.QUOTE_ENTITY, quoteId));

            if (quoteId != Guid.Empty)
                service.Update(opportunity);


            /********* Step 2 : Delete recently created quote line items*********/
            deleteQuoteProducts(service, quoteId);


            Console.WriteLine("Quote generated from the Opportunity.");
        }

        private void deleteQuoteProducts(IOrganizationService service, Guid quoteId)
        {
            // Get All Quote Products
            QueryExpression query = new QueryExpression(CONSTANT.QUOTE_PRODUCT_ENTITY);
            query.Criteria.AddCondition(CONSTANT.QUOTE_ID_FIELD, ConditionOperator.Equal, quoteId);

            EntityCollection quoteProductList = service.RetrieveMultiple(query);

            if (quoteProductList.Entities.Count > 0)
            {
                // Create Execute Multiple Request
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

                foreach (Entity item in quoteProductList.Entities)
                {
                    DeleteRequest deleteRequest = new DeleteRequest { Target = item.ToEntityReference() };
                    multipleRequest.Requests.Add(deleteRequest);
                }
                ExecuteMultipleResponse multipleResponse = (ExecuteMultipleResponse)service.Execute(multipleRequest);
            }

        }


        public void insertQuoteDetail()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            Entity quoteDetail = new Entity("quotedetail");
            Guid quoteId = new Guid("DB064E35-6AEF-E511-80F4-3863BB346A70");
            quoteDetail.Attributes.Add("quoteid", new EntityReference("quote", quoteId));
            quoteDetail.Attributes.Add("apttus_qpconfig_adjustmenttype", new OptionSetValue(100000003));
            quoteDetail.Attributes.Add("apttus_qpconfig_linenumber", Convert.ToDecimal(1));
            quoteDetail.Attributes.Add("apttus_qpconfig_netprice",new Money(2366));
            quoteDetail.Attributes.Add("apttus_proposal_productid", new EntityReference("product", new Guid("305953c1-cab9-e511-80f1-3863bb34ea28")));            
            quoteDetail.Attributes.Add("apttus_proposal_quantity", Convert.ToDecimal(1));
            quoteDetail.Attributes.Add("apttus_proposal_unit_price", new Money(2666));
            
            // if required
            quoteDetail.Attributes.Add("productid", new EntityReference("product", new Guid("305953c1-cab9-e511-80f1-3863bb34ea28")));
            quoteDetail.Attributes.Add("uomid", new EntityReference("uom", new Guid("97B0FD1B-DD58-4AD4-ACDD-EB270F959558")));

            service.Create(quoteDetail);
        }

        private void setCurrentQuotAsPrimary(IOrganizationService service, Guid opportunityId, Guid quoteId)
        {
            // Reset first all earlier Quote with Primary field to False
            QueryExpression query = new QueryExpression(CONSTANT.QUOTE_ENTITY);
            query.Criteria.AddCondition(CONSTANT.QUOTE_PRIMARY_FLAG_FIELD, ConditionOperator.Equal, true);
            EntityCollection primaryQuoteList = service.RetrieveMultiple(query);

            if (primaryQuoteList.Entities.Count > 0)
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

                foreach (Entity Item in primaryQuoteList.Entities)
                {
                    Item.Attributes[CONSTANT.QUOTE_PRIMARY_FLAG_FIELD] = false;
                    UpdateRequest updateRequest = new UpdateRequest { Target = Item };
                    multipleRequest.Requests.Add(updateRequest);
                }
                ExecuteMultipleResponse multipleResponse = (ExecuteMultipleResponse)service.Execute(multipleRequest);
            }

            // set Current Quote To Primary
            Entity quote = new Entity(CONSTANT.QUOTE_ENTITY);
            quote.Id = quoteId;
            quote.Attributes.Add(CONSTANT.QUOTE_PRIMARY_FLAG_FIELD, true);
            service.Update(quote);

        }


        private Guid createProductConfigFromQuote(IOrganizationService service, Guid quoteId)
        {
            Guid productConfigId = Guid.Empty;

            Entity productConfig = new Entity(CONSTANT.PRODUCT_CONFIGURATION_ENTITY);

            Entity quoteEntity = service.Retrieve(CONSTANT.QUOTE_ENTITY, quoteId, new ColumnSet(true));

            // quote reference
            productConfig.Attributes.Add(CONSTANT.PROD_CONFIG_QUOTE_FIELD, new EntityReference(CONSTANT.QUOTE_ENTITY, quoteId));

            // Set name field
            if (quoteEntity.Attributes.Contains(CONSTANT.QUOTE_NAME_FIELD) && quoteEntity.Attributes[CONSTANT.QUOTE_NAME_FIELD] != null)
                productConfig.Attributes.Add(CONSTANT.PROD_CONFIG_NAME_FIELD, quoteEntity.Attributes[CONSTANT.QUOTE_NAME_FIELD].ToString());

            // Set Pricelist
            if (quoteEntity.Attributes.Contains(CONSTANT.QUOTE_PRICELIST_FIELD) && quoteEntity.GetAttributeValue<EntityReference>(CONSTANT.QUOTE_PRICELIST_FIELD) != null)
            {
                EntityReference priceListRef = quoteEntity.GetAttributeValue<EntityReference>(CONSTANT.QUOTE_PRICELIST_FIELD);
                productConfig.Attributes.Add(CONSTANT.PROD_CONFIG_PRICELIST_FIELD, priceListRef);
            }

            // Set Bill to Account
            if (quoteEntity.Attributes.Contains(CONSTANT.QUOTE_BILLTO_ACCOUNT_FIELD) && quoteEntity.GetAttributeValue<EntityReference>(CONSTANT.QUOTE_BILLTO_ACCOUNT_FIELD) != null)
            {
                EntityReference billToRef = quoteEntity.GetAttributeValue<EntityReference>(CONSTANT.QUOTE_BILLTO_ACCOUNT_FIELD);
                productConfig.Attributes.Add(CONSTANT.PROD_CONFIG_BILLTO_ACCOUNT_FIELD, billToRef);
            }

            productConfigId = service.Create(productConfig);
            return productConfigId;
        }



        public void updateOpp(Guid OppId)
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            Entity oppEntity = new Entity("opportunity");
            oppEntity.Id = OppId;
            string CloseDate = "29-10-2016 12:00 AM";
            DateTime CloseDateVal;

            if (DateTime.TryParse(CloseDate, out CloseDateVal))
                oppEntity.Attributes.Add("estimatedclosedate", CloseDateVal);

            service.Update(oppEntity);
        }

        public void getAllOppbyCustomerId()
        {
            Guid CustomerId = new Guid("F2612DD3-B8C4-E511-80ED-3863BB346A70");
            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            QueryExpression query = new QueryExpression("opportunity");
            query.ColumnSet = new ColumnSet(true);
            query.Criteria.AddCondition("customerid", ConditionOperator.Equal, CustomerId);
            EntityCollection TaskList = service.RetrieveMultiple(query);


        }
    }

    public static class CONSTANT
    {
        //Entity 
        public const string QUOTE_ENTITY = "quote";
        public const string OPPORTUNITY_ENTITY = "opportunity";
        public const string QUOTE_PRODUCT_ENTITY = "quotedetail";
        public const string PRODUCT_CONFIGURATION_ENTITY = "apttus_config2_productconfiguration";


        //Fields
        public const string QUOTE_ID_FIELD = "quoteid";
        public const string QUOTE_NAME_FIELD = "name";
        public const string QUOTE_PRIMARY_FLAG_FIELD = "apttus_proposal_primary";
        public const string QUOTE_PRICELIST_FIELD = "pricelevelid";
        public const string QUOTE_BILLTO_ACCOUNT_FIELD = "apttus_QPConfig_BillToAccountIdid";


        public const string OPPORTUNITY_TRIGGERED_QUOTE_FIELD = "apttus_triggeredquote";
        public const string OPPORTUNITY_TRIGGERED_ACTION_FIELD = "apttus_triggercreatequoteaction";

        public const string PROD_CONFIG_QUOTE_FIELD = "apttus_qpconfig_proposaldid";
        public const string PROD_CONFIG_NAME_FIELD = "apttus_name";
        public const string PROD_CONFIG_PRICELIST_FIELD = "apttus_config2_pricelistidid";
        public const string PROD_CONFIG_BILLTO_ACCOUNT_FIELD = "apttus_config2_billtoaccountidid";




    }
}
