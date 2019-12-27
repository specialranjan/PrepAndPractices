using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using CredentialBasedTokenAuthDemo.Api.Models;

namespace CredentialBasedTokenAuthDemo.Api.Controllers
{
    //[Authorize]
    [EnableCors(origins: "https://localhost:44305", headers:"*", methods:"*")]
    [RoutePrefix("api/product")]
    public class ProductController : ApiController
    {
        [Route("list")]
        public async Task<HttpResponseMessage> GetProductList()
        {
            var products = new List<Product>() { new Product { Id = "1", Name = "Coffee Dust" }, new Product { Id = "2", Name = "Suger" } };
            return this.Request.CreateResponse(HttpStatusCode.OK, products);
        }
    }
}
