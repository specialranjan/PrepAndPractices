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
    public class ProxyCRMAction
    {

        public void CreateAccountWithUserImporsonate()
        {
            ServiceReference1.ProxyServiceClient ProxyCRMServiceObj = new ServiceReference1.ProxyServiceClient();

            DateTime startTime = DateTime.Now;
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            DateTime EndTime = DateTime.Now;

            var DiffinSecond = (EndTime - startTime).TotalSeconds;
            Console.WriteLine("Total Time Taken in Connect in seconds :" + DiffinSecond);
            Guid UserId = getCurrentUserId(service);
            UserId = new Guid("3DE87036-0DA1-E511-80ED-3863BB3C2660");

            Guid accountId = ProxyCRMServiceObj.CreateAccountwithImporonatedUser(UserId.ToString());            
        }

        public void PassOrganizationProxyData()
        {
            OrganizationServiceProxy service = CRMHelper.getOrganizationProxy();

            ServiceReference1.ProxyServiceClient Obj = new ServiceReference1.ProxyServiceClient();

            System.ServiceModel.Description.ClientCredentials cred = service.ClientCredentials;


            string url = service.ServiceManagement.CurrentServiceEndpoint.ListenUri.ToString();
            
            
            Obj.getConnectedandReturnAccountName(cred.UserName.UserName,cred.UserName.Password, url);
            

        }

        private Guid getCurrentUserId(IOrganizationService service)
        {
            WhoAmIRequest userRequest = new WhoAmIRequest();
            WhoAmIResponse user = (WhoAmIResponse)service.Execute(userRequest);
            return user.UserId;

        }

        public void ConnectCRM()
        {
            DateTime startTime = DateTime.Now;
            IOrganizationService service = CRMHelper.ConnectToMSCRM();
            DateTime EndTime = DateTime.Now;

            var DiffinSecond = (EndTime - startTime).TotalSeconds;
            Console.WriteLine("Total Time Taken in Connect in seconds :" + DiffinSecond);

        }

    }
}
