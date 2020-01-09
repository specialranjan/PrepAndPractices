using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
    public class CustomLogics
    {

        public void updateAnnotationforMSFT()
        {
            QueryExpression query = new QueryExpression("annotation");
            LinkEntity linkEntity = new LinkEntity("annotation", "apttus_xapps_application", "objectid", "apttus_xapps_applicationid", JoinOperator.Inner);
            linkEntity.LinkCriteria.AddCondition("apttus_name", ConditionOperator.Equal, "Agreement Uploader Dev 1");
            query.LinkEntities.Add(linkEntity);
            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            EntityCollection annotationList = service.RetrieveMultiple(query);

            foreach(Entity annotation in annotationList.Entities)
            {
                Entity anotation1 = new Entity(annotation.LogicalName);
                anotation1.Attributes.Add("isdocument", true);
                anotation1.Id = annotation.Id;
                service.Update(anotation1);
            }
        }


        public List<string> isValidLogicalGrouping()
        {
            List<string> groupList = new List<string>();
            string logicText = "(1 OR 2 OR 3) AND (((4 OR 5) AND (6 OR 7)) AND (8 OR 9 OR 10)) AND (11 OR 12 OR 13)";
            var charArray = logicText.ToCharArray();

            string groupItem = string.Empty;
            int openBracketcnt = 0;
            for (int count = 0; count < charArray.Length; count++)
            {
                if (charArray[count] == '(')
                {
                    openBracketcnt++;
                    if (openBracketcnt <= 1)
                    {
                        if (!string.IsNullOrEmpty(groupItem))
                            groupList.Add(groupItem);
                        groupItem = string.Empty;
                    }
                }
                else if (charArray[count] == ')')
                {
                    openBracketcnt--;
                    if (openBracketcnt == 0)
                    {
                        groupList.Add(groupItem);
                        groupItem = string.Empty;
                    }
                }
                else
                    groupItem = groupItem + charArray[count].ToString();
            }
            if (groupList.Count == 0)
                groupList.Add(groupItem);

            List<string> NumericItemList = groupList.Where(f => System.Text.RegularExpressions.Regex.IsMatch(f, @"\d") == true).ToList();
            List<string> validGroupList = new List<string>();
            for (int count = 0; count < NumericItemList.Count; count++)
            {
                string realVal = string.Empty;
                string item = NumericItemList[count];
                string[] numbers = System.Text.RegularExpressions.Regex.Split(item, @"\D+");
                foreach (string value in numbers)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        int i = int.Parse(value);
                        realVal = realVal + i.ToString() + "|";
                    }
                }
                if (!string.IsNullOrEmpty(realVal))
                {
                    realVal = realVal.Substring(0, realVal.Length - 1);
                    validGroupList.Add(realVal);
                }
            }
            return validGroupList;
        }


        public List<String> GetAllFiles(String directory)
        {
            List<string> AllFiles = Directory.GetFiles(directory, "*", SearchOption.AllDirectories).ToList();
            return AllFiles;

        }

        public List<String> DirSearch(string sDir)
        {
            List<String> files = new List<String>();
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    if (!sDir.Contains(".git"))
                        files.Add(f);
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    if (!sDir.Contains(".git"))
                        files.AddRange(DirSearch(d));
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine("Error Occured :" + excpt.Message.ToString());
                Console.ReadLine();
            }

            return files;
        }

        public void cleanupContacts()
        {
            DateTime DateFilter = DateTime.Now.AddDays(-1);
            QueryExpression query = new QueryExpression("account");
            IOrganizationService service = CRMHelper.ConnectToMSCRM1();
            Console.WriteLine("IOrganization Service Object Created");
            EntityCollection EntityList = CRMHelper.RetrieveMultiple(query, service);
            ExecuteMultipleRequest mulitRequest = null;

            int offset = 1;
            Console.WriteLine("Total Record to be Deleted are : " + EntityList.Entities.Count);
            mulitRequest = getNewObject();
            for (int count = 0; count < EntityList.Entities.Count; count++)
            {
                if (offset % 200 == 0)
                {
                    // Execute all the requests in the request collection using a single web method call.
                    ExecuteMultipleResponse multipleResponse = (ExecuteMultipleResponse)service.Execute(mulitRequest);
                    Console.WriteLine("Bunch of 200 Record Deleted  & Total record Proceed are : " + count);
                    mulitRequest = getNewObject();
                }
                Entity Item = EntityList.Entities[count];
                DeleteRequest deleteRequest = new DeleteRequest { Target = Item.ToEntityReference() };
                mulitRequest.Requests.Add(deleteRequest);
                offset = count + 1;
            }
        }


        public void SerializeQueryExpression()
        {
            QueryExpression query = new QueryExpression("contact");
            query.ColumnSet = new ColumnSet("firstname", "lastname", "fullname");
            query.Criteria.AddCondition("contacttype", ConditionOperator.Equal, 453);
            query.Orders.Add(new OrderExpression("firstname", OrderType.Ascending));

            string outPut = string.Empty;
            System.Xml.Serialization.XmlSerializer SerObje = new System.Xml.Serialization.XmlSerializer(query.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                SerObje.Serialize(textWriter, query);
                outPut = textWriter.ToString();
            }

            QueryExpression queryNew = new QueryExpression();
            queryNew = (QueryExpression)SerObje.Deserialize(new StringReader(outPut));

            queryNew.Criteria.Conditions.RemoveAt(0);
            queryNew.Criteria.AddCondition("fullname", ConditionOperator.NotNull);


        }


        public void UpdateConditionExpress()
        {
            QueryExpression query = new QueryExpression("contact");
            query.Criteria.AddCondition("name", ConditionOperator.Equal, "Ankit");
            query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);
            List<string> stringList = new List<string>();
            for (int count = 1; count <= 10; count++)
                stringList.Add("A" + count.ToString());
            query.Criteria.AddCondition("type", ConditionOperator.In, stringList.ToArray());



            /* Get All ConditionExpression Of Query*/

            List<ConditionExpression> ListCOnd = new List<ConditionExpression>();

            if (query.Criteria.Conditions.Count > 0)
            {
                foreach (ConditionExpression filter in query.Criteria.Conditions)                
                    ListCOnd.Add(filter);
            }
            ConditionExpression oldExpression = null;
            ConditionExpression newExpression = null;
            foreach (ConditionExpression cond in ListCOnd)
            {
                
                List<string> ABCList = new List<string>();
                ABCList.Add("AB");
                ABCList.Add("CD");
                if (cond.Operator == ConditionOperator.In)
                {
                    oldExpression = cond;
                    newExpression = new ConditionExpression("type", ConditionOperator.In, ABCList.ToArray());
                    break;
                }
            }
            oldExpression = newExpression;
        }

        public void deactiveContacts(string EntityName)
        {
            DateTime DateFilter = DateTime.Now.AddDays(-1);
            QueryExpression query = new QueryExpression(EntityName);
            query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);
            IOrganizationService service = CRMHelper.ConnectToMSCRM1();
            Console.WriteLine("IOrganization Service Object Created");
            EntityCollection EntityList = CRMHelper.RetrieveMultiple(query, service);
            ExecuteMultipleRequest mulitRequest = null;

            int offset = 1;
            Console.WriteLine("Total Record to be Deleted are : " + EntityList.Entities.Count);
            mulitRequest = getNewObject();
            for (int count = 0; count < EntityList.Entities.Count; count++)
            {
                if (offset % 200 == 0)
                {
                    // Execute all the requests in the request collection using a single web method call.
                    ExecuteMultipleResponse multipleResponse = (ExecuteMultipleResponse)service.Execute(mulitRequest);
                    Console.WriteLine("Bunch of 200 Record Deleted  & Total record Proceed are : " + count);
                    mulitRequest = getNewObject();
                }
                Entity Item = EntityList.Entities[count];
                //StateCode = 1 and StatusCode = 2 for deactivating Account or Contact
                SetStateRequest setStateRequest = new SetStateRequest()
                {
                    EntityMoniker = new EntityReference
                    {
                        Id = Item.Id,
                        LogicalName = Item.LogicalName
                    },
                    State = new OptionSetValue(1),
                    Status = new OptionSetValue(2)
                };
                mulitRequest.Requests.Add(setStateRequest);
                offset = count + 1;
            }
        }

        private ExecuteMultipleRequest getNewObject()
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

            return multipleRequest;

        }


        public void updateContactOwner()
        {
            QueryExpression query = new QueryExpression("contact");
            query.ColumnSet = new ColumnSet(new string[] { "new_parentaccount", "ownerid", "fullname" });
            LinkEntity contactLink = new LinkEntity("contact", "account", "new_parentaccount", "accountid", JoinOperator.Inner);
            contactLink.EntityAlias = "Company";
            contactLink.Columns = new ColumnSet(new string[] { "ownerid" });
            query.LinkEntities.Add(contactLink);
            IOrganizationService service = CRMHelper.ConnectToMSCRM1();
            EntityCollection contactList = CRMHelper.RetrieveMultiple(query, service);
            ExecuteMultipleRequest mulitRequest = null;
            int offset = 1;
            Console.WriteLine("Total Record to be Deleted are : " + contactList.Entities.Count);
            mulitRequest = getNewObject();
            for (int count = 0; count < contactList.Entities.Count; count++)
            {
                Guid OwnerId = Guid.Empty;
                string fullName = string.Empty;
                string accountName = string.Empty;
                if (contactList.Entities[count].Attributes.Contains("fullname"))
                    fullName = contactList.Entities[count].Attributes["fullname"].ToString();

                AliasedValue val = (AliasedValue)contactList.Entities[count].Attributes["Company.ownerid"];
                EntityReference ownerIdRef = (EntityReference)val.Value;
                OwnerId = ownerIdRef.Id;

                accountName = ownerIdRef.Name;
                Guid ContactOwner = contactList.Entities[count].GetAttributeValue<EntityReference>("ownerid").Id;
                Entity contact = new Entity("contact");
                if (ContactOwner != OwnerId)
                {
                    Console.WriteLine("Miss-Match Owner of For Contact : " + fullName + " And Account is : " + accountName);
                    contact.Id = contactList.Entities[count].Id;
                    contact.Attributes.Add("ownerid", new EntityReference("systemuser", OwnerId));
                    UpdateRequest updateRequest = new UpdateRequest { Target = contact };
                    mulitRequest.Requests.Add(updateRequest);
                }

            }
            ExecuteMultipleResponse multipleResponse = (ExecuteMultipleResponse)service.Execute(mulitRequest);
            Console.ReadLine();
        }


    }
}
