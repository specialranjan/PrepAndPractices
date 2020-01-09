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
   public class SecurityTeamAction
    {
        public void GetAllSecurityteam()
        {
            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            /*
             * 
             * Select AccountId, AccountName
             * From Account
             * Where AccountStatus= 'Customer'
             * Order by AccountName Asec
             * */

            string teamName = "FinanceTeam";
            QueryExpression query = new QueryExpression("systemuser");
            query.ColumnSet = new ColumnSet(new string[] { "fullname" ,"domainname"});
            LinkEntity TeamOwnerShipLinkEntity = new LinkEntity("systemusuer", "teammembership", "systemuserid", "systemuserid", JoinOperator.Inner);
            LinkEntity TeamLinkEnitty = new LinkEntity("teammembership", "team", "teamid", "teamid", JoinOperator.Inner);
            TeamLinkEnitty.LinkCriteria.AddCondition("name", ConditionOperator.Equal, teamName);
            TeamOwnerShipLinkEntity.LinkEntities.Add(TeamLinkEnitty);
            query.LinkEntities.Add(TeamOwnerShipLinkEntity);
         

            EntityCollection accountEntityList = service.RetrieveMultiple(query);




            // Create the AddMembersTeamRequest object.
            AddMembersTeamRequest addRequest = new AddMembersTeamRequest();

            // Set the AddMembersTeamRequest TeamID property to the object ID of 
            // an existing team.
            addRequest.TeamId = new Guid("A9DE5707-7D0D-E811-A958-000D3AF083FD");

            // Set the AddMembersTeamRequest MemberIds property to an 
            // array of GUIDs that contains the object IDs of one or more system users.
            Guid[] memberIds = new[] { new Guid("C68F0F3F-680D-E811-A95B-000D3AF07CE4") };
            addRequest.MemberIds = memberIds;

            // Execute the request.
            service.Execute(addRequest);

            RemoveMembersTeamRequest removeRequest = new RemoveMembersTeamRequest();

            removeRequest.TeamId = new Guid("A9DE5707-7D0D-E811-A958-000D3AF083FD");

            // Set the AddMembersTeamRequest MemberIds property to an 
            // array of GUIDs that contains the object IDs of one or more system users.

            removeRequest.MemberIds = memberIds;

            // Execute the request.
            service.Execute(removeRequest);


            //service.Associate("team", new Guid("A9DE5707-7D0D-E811-A958-000D3AF083FD"), new Relationship("teamroles_association"),)

        }
    }
}
