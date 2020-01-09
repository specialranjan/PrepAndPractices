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
namespace Apttus.XAuthor.DynamicsCRMIntegration.SandBox
{
    public class QuoteAction
    {

        public void updateQuote()
        {
            Guid quoteId = new Guid("83BCE3DA-26D7-E511-80EF-3863BB369D50");
            IOrganizationService service = CRMHelper.ConnectToMSCRM();

            Entity quote = new Entity("quote");
            quote.Id = quoteId;
            // update Discount %
            //decimal discPercentage = 10;            
            //quote.Attributes.Add("discountpercentage", discPercentage);
            


            //update Discount Amount
            Money currency = new Money(110);
            quote.Attributes.Add("totaltax", currency);

            service.Update(quote);


            Entity item = service.Retrieve("quote", quoteId, new ColumnSet(new string[] { "totaltax"}));
        }
    }
}
