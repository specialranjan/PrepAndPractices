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
    public class ABMSCRMAdapter
    {

        public ApplicationObject LoadApplication(Guid uniqueId, Guid appId)
        {
            ApplicationObject App = null;

            MSCRMAdapterController CRMAdapter = new MSCRMAdapterController();
             LoadAppResult  loadAppResult = CRMAdapter.LoadApplication(uniqueId, appId);
            if(loadAppResult != null)
            {
                App = new ApplicationObject();
                App.Config = loadAppResult.config;
                App.Schema = loadAppResult.schema;
                App.Template = loadAppResult.xlstemplate;
                App.TemplateName = loadAppResult.templateName;
            }

            return App;
        }

        public bool saveApplication(Guid appId, byte[] config, byte[] template, string templateName, byte[] scheme, string edition)
        {
            MSCRMAdapterController msCRMAdaptercontroller = new MSCRMAdapterController();
            return msCRMAdaptercontroller.saveApplication(appId, Guid.Empty, config, template, templateName, scheme, edition);
        }

        private ApplicationObject LoadApplicationbyuniqueId(Guid uniqueId)
        {
            return LoadApplication(uniqueId, Guid.Empty);
        }
        public ApplicationObject LoadApplicationByAppId(Guid appId)
        {
            return LoadApplication(Guid.Empty, appId);
        }
    }
}
