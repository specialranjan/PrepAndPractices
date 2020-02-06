using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;

namespace CSharpQuestions.Web
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var json = this.GetJsonContentFromRestApi().Result;
            if (json != null)
                lblAsyncAwaitCallMessage.Text = "The json is loaded now without blocking UI thread with ConfigureAwait.";
            else
                lblAsyncAwaitCallMessage.Text = "The json is still loading but UI thread is not blocked  with ConfigureAwait.";


        }

        private async Task<string> GetJsonContentFromRestApi()
        {
            lblAsyncAwaitCallMessage.Text = "Loading json from a rest api...";
            var requestUri = "https://api.github.com/users/mralexgray/repos";
            using (var httpClient = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri))
                {
                    request.Headers.Add("user-agent", "asp.net");
                    using (var httpResonse = await httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        var response = await httpResonse.Content.ReadAsStringAsync().ConfigureAwait(false);
                        return JValue.Parse(response).ToString(Newtonsoft.Json.Formatting.Indented);
                    }
                }
            }
        }
    }
}