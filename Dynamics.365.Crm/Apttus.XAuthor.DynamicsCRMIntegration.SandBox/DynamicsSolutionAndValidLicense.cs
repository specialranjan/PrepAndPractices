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
    public class DynamicsSolutionAndValidLicense
    {
        public static string XAUTHOR_LICENSE_ALLOCATION_ENTITY = "apttex_xauthorlicenseallocation";
        
        public bool IsXAuthorExcelSolutionInstalled(string uniqueName)
        {
            try
            {
                bool value = false;
                QueryExpression querySampleSolution = new QueryExpression
                {
                    EntityName = "solution",
                    ColumnSet = new ColumnSet(new string[] { "publisherid", "installedon", "version", "versionnumber", "friendlyname" }),
                };

                querySampleSolution.Criteria.AddCondition("uniquename", ConditionOperator.Equal, uniqueName);
                IOrganizationService service = CRMHelper.ConnectToMSCRM();
                EntityCollection SolutionList = service.RetrieveMultiple(querySampleSolution);
                if (SolutionList.Entities.Count == 1)
                    value = true;
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int getLoggedinUserLicense (Guid userId)
        {
            int licenseType = 0;

            QueryExpression query = new QueryExpression(XAUTHOR_LICENSE_ALLOCATION_ENTITY);
            query.ColumnSet = new ColumnSet(new string[] { "apttex_licensetype" });
            query.Criteria.AddCondition("apttex_assignedto", ConditionOperator.Equal, userId);
            query.Criteria.AddCondition("apttex_licensestatus", ConditionOperator.Equal, true);

            EntityCollection licenseAllocationEntSet = CRMHelper.ConnectToMSCRM().RetrieveMultiple(query);

            if (licenseAllocationEntSet.Entities.Count <= 0)
                return licenseType;
            if (licenseAllocationEntSet.Entities[0].Attributes.Contains("apttex_licensetype") && licenseAllocationEntSet.Entities[0].Attributes["apttex_licensetype"] != null)
            {
                licenseType = licenseAllocationEntSet.Entities[0].GetAttributeValue<OptionSetValue>("apttex_licensetype").Value;

            }

            return licenseType;


        }

    }



   

}
