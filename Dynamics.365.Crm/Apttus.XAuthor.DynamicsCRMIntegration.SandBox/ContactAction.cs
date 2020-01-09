using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Xml.Linq;

namespace Apttus.XAuthor.DynamicsCRMIntegration.SandBox
{
    public class ContactAction
    {
        //public string getAllContactBySearch(string searchStr)
        //{
        //    string serviceURL = ConfigurationManager.AppSettings["CRMServiceURL"].ToString();
        //    // Build and send the HTTP request.  
        //    string url = serviceURL + "/ContactSet" + "?$select=FirstName,LastName,FullName,EMailAddress1,Telephone1,Address1_Country,ParentCustomerId,ContactId";

        //    if (searchStr.Length > 0)
        //    {
        //        url = url + "&$filter=substringof('" + searchStr + "',FullName)";
        //    }
        //    //string values = CRMHelper.RetrieveRecord(url);
        //    List<Contact> List = new List<Contact>();
        //    List = ParseResponseToContact(values);
        //    return values;
        //}


        private static List<Contact> ParseResponseToContact(string data)
        {
            List<Contact> contactList = new List<Contact>();

            //feed namespace
            XNamespace rss = "http://www.w3.org/2005/Atom";
            // d namespace
            XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            // m namespace
            XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

            try
            {
                XDocument xdoc;
                xdoc = XDocument.Parse(data, LoadOptions.None);

                //For Socail Activity
                foreach (var entry in xdoc.Root.Descendants(rss + "entry").Descendants(rss + "content").Descendants(m + "properties"))
                {
                    Contact contactItem = new Contact();
                    XElement element;

                    element = entry.Element(d + "ContactId");
                    if (null != element)
                    {
                        contactItem.ContactId = new Guid(element.Value);
                    }
                    element = entry.Element(d + "FullName");
                    if (null != element)
                    {
                        contactItem.FirstName = element.Value;
                    }

                    element = entry.Element(d + "EMailAddress1");
                    if (null != element)
                    {
                        contactItem.EmailAddress = element.Value;
                    }



                    //contactItem.ContactUrl = CreateURL("Contact", contactItem.ContactId.ToString());

                    //contactItem.ParentCustomerUrl = CreateURL(contactItem.ParentCustomer.LogicalName, contactItem.ParentCustomer.Id.ToString());
                    contactList.Add(contactItem);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return contactList;
        }

    }





    public class Contact
    {
        public Guid ContactId { get; set; }
        public string FirstName { get; set; }
        public string EmailAddress { get; set; }        
    }
}
