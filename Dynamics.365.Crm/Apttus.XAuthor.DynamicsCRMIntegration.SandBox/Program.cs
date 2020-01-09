using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Crm.Sdk.Messages;
using System.ServiceModel;
using Microsoft.Xrm.Sdk.Discovery;
using System.Xml;


namespace Apttus.XAuthor.DynamicsCRMIntegration.SandBox
{
    class Program
    {
        static void Main(string[] args)
        {
            SecurityTeamAction objSecTeamAction = new SecurityTeamAction();
            objSecTeamAction.GetAllSecurityteam();



        }

        public static string RemoveDuplicates(string input)
        {
            char[] chars = input.ToCharArray();

            string output = "";
            foreach (char c in chars)
                if (!output.Contains(c))
                    output = output + c;

            return output;
        }
    }


    class AuthenticateWithNoHelp
    {
        #region Class Level Members
        // To get discovery service address and organization unique name, 
        // Sign in to your CRM org and click Settings, Customization, Developer Resources.
        // On Developer Resource page, find the discovery service address under Service Endpoints and organization unique name under Your Organization Information.
        private String _discoveryServiceAddress = "https://disco.crm.dynamics.com/XRMServices/2011/Discovery.svc";
        private String _organizationUniqueName = "apttex";
        // Provide your user name and password.
        private String _userName = "gramachandran@apttex.onmicrosoft.com";
        private String _password = "Ganapath1";

        // Provide domain name for the On-Premises org.
        private String _domain = "";

        #endregion Class Level Members


        /// <summary>
        /// 
        /// </summary>
        public void Run()
        {
            IServiceManagement<IDiscoveryService> serviceManagement =
                        ServiceConfigurationFactory.CreateManagement<IDiscoveryService>(
                        new Uri(_discoveryServiceAddress));
            AuthenticationProviderType endpointType = serviceManagement.AuthenticationType;

            // Set the credentials.
            AuthenticationCredentials authCredentials = GetCredentials(serviceManagement, endpointType);


            String organizationUri = String.Empty;
            // Get the discovery service proxy.
            using (DiscoveryServiceProxy discoveryProxy =
                GetProxy<IDiscoveryService, DiscoveryServiceProxy>(serviceManagement, authCredentials))
            {
                // Obtain organization information from the Discovery service. 
                if (discoveryProxy != null)
                {
                    // Obtain information about the organizations that the system user belongs to.
                    OrganizationDetailCollection orgs = DiscoverOrganizations(discoveryProxy);
                    // Obtains the Web address (Uri) of the target organization.
                    organizationUri = FindOrganization(_organizationUniqueName,
                        orgs.ToArray()).Endpoints[EndpointType.OrganizationService];

                }
            }


            if (!String.IsNullOrWhiteSpace(organizationUri))
            {
                IServiceManagement<IOrganizationService> orgServiceManagement =
                    ServiceConfigurationFactory.CreateManagement<IOrganizationService>(
                    new Uri(organizationUri));

                // Set the credentials.
                AuthenticationCredentials credentials = GetCredentials(orgServiceManagement, endpointType);

                // Get the organization service proxy.
                using (OrganizationServiceProxy organizationProxy =
                    GetProxy<IOrganizationService, OrganizationServiceProxy>(orgServiceManagement, credentials))
                {
                    // This statement is required to enable early-bound type support.
                    organizationProxy.EnableProxyTypes();

                    // Now make an SDK call with the organization service proxy.
                    // Display information about the logged on user.
                    Guid userid = ((WhoAmIResponse)organizationProxy.Execute(
                        new WhoAmIRequest())).UserId;
                    //SystemUser systemUser = organizationProxy.Retrieve("systemuser", userid,
                    //    new ColumnSet(new string[] { "firstname", "lastname" })).ToEntity<SystemUser>();
                    //Console.WriteLine("Logged on user is {0} {1}.",
                    //    systemUser.FirstName, systemUser.LastName);
                }
            }

        }

        /// <summary>
        /// Obtain the AuthenticationCredentials based on AuthenticationProviderType.
        /// </summary>
        /// <param name="service">A service management object.</param>
        /// <param name="endpointType">An AuthenticationProviderType of the CRM environment.</param>
        /// <returns>Get filled credentials.</returns>
        private AuthenticationCredentials GetCredentials<TService>(IServiceManagement<TService> service, AuthenticationProviderType endpointType)
        {
            AuthenticationCredentials authCredentials = new AuthenticationCredentials();

            switch (endpointType)
            {
                case AuthenticationProviderType.ActiveDirectory:
                    authCredentials.ClientCredentials.Windows.ClientCredential =
                        new System.Net.NetworkCredential(_userName,
                            _password,
                            _domain);
                    break;
                case AuthenticationProviderType.LiveId:
                    authCredentials.ClientCredentials.UserName.UserName = _userName;
                    authCredentials.ClientCredentials.UserName.Password = _password;
                    authCredentials.SupportingCredentials = new AuthenticationCredentials();
                    authCredentials.SupportingCredentials.ClientCredentials = null;
                    //Microsoft.Crm.Services.Utility.DeviceIdManager.LoadOrRegisterDevice();
                    break;
                default: // For Federated and OnlineFederated environments.                    
                    authCredentials.ClientCredentials.UserName.UserName = _userName;
                    authCredentials.ClientCredentials.UserName.Password = _password;
                    // For OnlineFederated single-sign on, you could just use current UserPrincipalName instead of passing user name and password.
                    // authCredentials.UserPrincipalName = UserPrincipal.Current.UserPrincipalName;  // Windows Kerberos

                    // The service is configured for User Id authentication, but the user might provide Microsoft
                    // account credentials. If so, the supporting credentials must contain the device credentials.
                    if (endpointType == AuthenticationProviderType.OnlineFederation)
                    {
                        IdentityProvider provider = service.GetIdentityProvider(authCredentials.ClientCredentials.UserName.UserName);
                        if (provider != null && provider.IdentityProviderType == IdentityProviderType.LiveId)
                        {
                            authCredentials.SupportingCredentials = new AuthenticationCredentials();
                            authCredentials.SupportingCredentials.ClientCredentials = null;
                            //Microsoft.Crm.Services.Utility.DeviceIdManager.LoadOrRegisterDevice();
                        }
                    }

                    break;
            }

            return authCredentials;
        }

        /// <summary>
        /// Discovers the organizations that the calling user belongs to.
        /// </summary>
        /// <param name="service">A Discovery service proxy instance.</param>
        /// <returns>Array containing detailed information on each organization that 
        /// the user belongs to.</returns>
        public OrganizationDetailCollection DiscoverOrganizations(
            IDiscoveryService service)
        {
            if (service == null) throw new ArgumentNullException("service");
            RetrieveOrganizationsRequest orgRequest = new RetrieveOrganizationsRequest();
            RetrieveOrganizationsResponse orgResponse =
                (RetrieveOrganizationsResponse)service.Execute(orgRequest);

            return orgResponse.Details;
        }

        /// <summary>
        /// Finds a specific organization detail in the array of organization details
        /// returned from the Discovery service.
        /// </summary>
        /// <param name="orgUniqueName">The unique name of the organization to find.</param>
        /// <param name="orgDetails">Array of organization detail object returned from the discovery service.</param>
        /// <returns>Organization details or null if the organization was not found.</returns>
        /// <seealso cref="DiscoveryOrganizations"/>
        public OrganizationDetail FindOrganization(string orgUniqueName,
            OrganizationDetail[] orgDetails)
        {
            if (String.IsNullOrWhiteSpace(orgUniqueName))
                throw new ArgumentNullException("orgUniqueName");
            if (orgDetails == null)
                throw new ArgumentNullException("orgDetails");
            OrganizationDetail orgDetail = null;

            foreach (OrganizationDetail detail in orgDetails)
            {
                if (String.Compare(detail.UniqueName, orgUniqueName,
                    StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    orgDetail = detail;
                    break;
                }
            }
            return orgDetail;
        }

        /// <summary>
        /// Generic method to obtain discovery/organization service proxy instance.
        /// </summary>
        /// <typeparam name="TService">
        /// Set IDiscoveryService or IOrganizationService type to request respective service proxy instance.
        /// </typeparam>
        /// <typeparam name="TProxy">
        /// Set the return type to either DiscoveryServiceProxy or OrganizationServiceProxy type based on TService type.
        /// </typeparam>
        /// <param name="serviceManagement">An instance of IServiceManagement</param>
        /// <param name="authCredentials">The user's Microsoft Dynamics CRM logon credentials.</param>
        /// <returns></returns>
        private TProxy GetProxy<TService, TProxy>(
            IServiceManagement<TService> serviceManagement,
            AuthenticationCredentials authCredentials)
            where TService : class
            where TProxy : ServiceProxy<TService>
        {
            Type classType = typeof(TProxy);

            if (serviceManagement.AuthenticationType !=
                AuthenticationProviderType.ActiveDirectory)
            {
                AuthenticationCredentials tokenCredentials =
                    serviceManagement.Authenticate(authCredentials);
                // Obtain discovery/organization service proxy for Federated, LiveId and OnlineFederated environments. 
                // Instantiate a new class of type using the 2 parameter constructor of type IServiceManagement and SecurityTokenResponse.
                return (TProxy)classType
                    .GetConstructor(new Type[] { typeof(IServiceManagement<TService>), typeof(SecurityTokenResponse) })
                    .Invoke(new object[] { serviceManagement, tokenCredentials.SecurityTokenResponse });
            }

            // Obtain discovery/organization service proxy for ActiveDirectory environment.
            // Instantiate a new class of type using the 2 parameter constructor of type IServiceManagement and ClientCredentials.
            return (TProxy)classType
                .GetConstructor(new Type[] { typeof(IServiceManagement<TService>), typeof(ClientCredentials) })
                .Invoke(new object[] { serviceManagement, authCredentials.ClientCredentials });
        }
    }
    public class BuildObjects
    {
        public static string AppDefinitionFilePath = @"C:\AppFiles\AppDefinition.xml";
        public static string TemplateXLSFilePath = @"C:\AppFiles\Template.xlsx";
        public static string ExternalSchemaJsonFilePath = @"C:\AppFiles\ExternalSchema.json";

        public static DynamicsCRMConnectionProperties BuildCRMConnectionString()
        {
            DynamicsCRMConnectionProperties CRMConnection = new DynamicsCRMConnectionProperties();
            CRMConnection.ServiceURL = "https://apttex.api.crm.dynamics.com/XRMServices/2011/Organization.svc";
            CRMConnection.UserName = "gramachandran@apttex.onmicrosoft.com";
            CRMConnection.Password = "Ganapath1";
            return CRMConnection;
        }


        public static List<Annotation> BuildAnnotationObjects()
        {
            List<Annotation> AnnotationList = new List<Annotation>();
            Annotation Item = new Annotation();
            // AppDefinitation.xml File
            byte[] filecontent = System.IO.File.ReadAllBytes(AppDefinitionFilePath);
            string encodeddata = System.Convert.ToBase64String(filecontent);
            Item.FileName = "AppDefinitation.xml";
            Item.MimeType = "APPLICATION/XML";
            Item.NoteText = "Application Definitaiton Mapping File";
            Item.Subject = "Application Definiation";
            Item.DocumentBody = encodeddata;
            AnnotationList.Add(Item);

            // Clear Local Variables
            Item = new Annotation();
            encodeddata = string.Empty;
            filecontent = null;

            // Template.xml File

            filecontent = System.IO.File.ReadAllBytes(TemplateXLSFilePath);
            encodeddata = System.Convert.ToBase64String(filecontent);
            Item.FileName = "Template.xlsx";
            Item.MimeType = "application/excel";
            Item.NoteText = "Template XLSX File";
            Item.Subject = "Data Template File";
            Item.DocumentBody = encodeddata;
            AnnotationList.Add(Item);

            // Clear Local Variables
            Item = new Annotation();
            encodeddata = string.Empty;
            filecontent = null;

            // Externalschema.json File

            filecontent = System.IO.File.ReadAllBytes(ExternalSchemaJsonFilePath);
            encodeddata = System.Convert.ToBase64String(filecontent);
            Item.FileName = "ExternalSchema.json";
            Item.MimeType = "application/json";
            Item.NoteText = "External Schema Json File";
            Item.Subject = "External Schema Json Format";
            Item.DocumentBody = encodeddata;
            AnnotationList.Add(Item);

            return AnnotationList;

        }



    }

}
