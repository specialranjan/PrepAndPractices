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
    public class FieldLevelSecurityAction
    {

        public void createNewFieldLevelSecurityProfile()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            //Create Security Profile
            Entity profile = new Entity("fieldsecurityprofile");
            profile["name"] = "Profile Created From Code";
            profile["description"] = "Profile Created from Code and just to use for test purpose";
            Guid profileId = service.Create(profile);

            // Create Field Permission
            Entity permission = new Entity("fieldpermission");
            permission["fieldsecurityprofileid"] = new EntityReference(profile.LogicalName, profileId);
            permission["entityname"] = "opportunity";
            permission["attributelogicalname"] = "estimatedvalue";
            permission["canread"] = new OptionSetValue(FieldPermissionType.NotAllowed);
            permission["cancreate"] = new OptionSetValue(FieldPermissionType.NotAllowed);
            permission["canupdate"] = new OptionSetValue(FieldPermissionType.NotAllowed);
            Guid permissionId = service.Create(permission);

            // Associate Field Security Profile with Users
            Guid userId = new Guid("0CEAF899-6D01-4577-80DF-0EDEBCE57570");
            Relationship relationShip = new Relationship("systemuserprofiles_association");
            EntityReferenceCollection collection = new EntityReferenceCollection();
            collection.Add(new EntityReference(profile.LogicalName, profileId));
            service.Associate("systemuser", userId, relationShip, collection);            
        }
    }
}
