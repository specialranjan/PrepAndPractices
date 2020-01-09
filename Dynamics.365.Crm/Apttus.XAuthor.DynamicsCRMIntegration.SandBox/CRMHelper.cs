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
using System.Net.Http;
using System.Net;
using System.Web;

namespace Apttus.XAuthor.DynamicsCRMIntegration.SandBox
{
    public class CRMHelper
    {
        public static Guid _USERID;

        

        public static IOrganizationService ConnectToMSCRM()
        {
            try
            {

                string serviceURL = ConfigurationManager.AppSettings["CRMServiceURL"].ToString();
                string userName = ConfigurationManager.AppSettings["UserName"].ToString();
                string password = ConfigurationManager.AppSettings["Password"].ToString();
                IOrganizationService _service;
                ClientCredentials credentials = new ClientCredentials();
                credentials.UserName.UserName = userName;
                credentials.UserName.Password = password;                
                Uri serviceUri = new Uri(serviceURL);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                OrganizationServiceProxy proxy = new OrganizationServiceProxy(serviceUri, null, credentials, null);                
                proxy.EnableProxyTypes();                         
                _service = (IOrganizationService)proxy;
                return _service;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static IOrganizationService ConnectToMSCRM1()
        {
            try
            {

                string serviceURL = ConfigurationManager.AppSettings["CRMServiceURL"].ToString();
                string userName = ConfigurationManager.AppSettings["UserName"].ToString();
                string password = "PN6789^&*(";
                IOrganizationService _service;
                ClientCredentials credentials = new ClientCredentials();
                credentials.UserName.UserName = userName;
                credentials.UserName.Password = password;
                Uri serviceUri = new Uri(serviceURL);
                OrganizationServiceProxy proxy = new OrganizationServiceProxy(serviceUri, null, credentials, null);
                proxy.EnableProxyTypes();                
                _service = (IOrganizationService)proxy;
                return _service;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static OrganizationServiceProxy getOrganizationProxy()
        {
            try
            {

                string serviceURL = ConfigurationManager.AppSettings["CRMServiceURL"].ToString();
                string userName = ConfigurationManager.AppSettings["UserName"].ToString();
                string password = ConfigurationManager.AppSettings["Password"].ToString();
                IOrganizationService _service;
                ClientCredentials credentials = new ClientCredentials();
                credentials.UserName.UserName = userName;
                credentials.UserName.Password = password;
                Uri serviceUri = new Uri(serviceURL);
                OrganizationServiceProxy proxy = new OrganizationServiceProxy(serviceUri, null, credentials, null);                
                return proxy;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public static string RetrieveRecord(string url)
        //{
        //    HttpClient httpClient = CRMHelper.CreateCrmConnectionForOnPremise();

        //    HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, url);
        //    req.Method = HttpMethod.Get;

        //    Task<HttpResponseMessage> response = httpClient.SendAsync(req);

        //    HttpResponseMessage resp = response.Result;
        //    Task<string> responseBodyAsText = resp.Content.ReadAsStringAsync();
        //    return responseBodyAsText.Result;
        //}

        public static EntityCollection RetrieveMultiple(QueryExpression query, IOrganizationService service)
        {
            EntityCollection EntityList = new EntityCollection();

            try
            {
                int pageNumber = 1;
                RetrieveMultipleRequest multiRequest;
                RetrieveMultipleResponse multiResponse = new RetrieveMultipleResponse();                
                do
                {
                    query.PageInfo.Count = 5000;
                    query.PageInfo.PagingCookie = (pageNumber == 1) ? null : multiResponse.EntityCollection.PagingCookie;
                    query.PageInfo.PageNumber = pageNumber++;
                    multiRequest = new RetrieveMultipleRequest();
                    multiRequest.Query = query;
                    multiResponse = (RetrieveMultipleResponse)service.Execute(multiRequest);                    
                    EntityList.Entities.AddRange(multiResponse.EntityCollection.Entities);
                    Console.WriteLine("Fetch 5000 record Set & Total Records Fetch So far are : " + EntityList.Entities.Count);
                }
                while (multiResponse.EntityCollection.MoreRecords);

            }
            catch (Exception ex)
            {
                throw ex;

            }
            return EntityList;
        }


       
    }
}
