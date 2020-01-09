namespace Employee.App.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Employee.App.Api.Infrastructure.Filters.Authentication;
    using Employee.App.Common.Models;

    //[TokenAuthentication]
    [Authorize]
    [RoutePrefix("api/employee/directory")]
    public class EmployeeDirectoryController : ApiController
    {
        [HttpGet]
        [Route("allcontacts")]
        [ResponseType(typeof(List<EmployeeContacts>))]
        public async Task<HttpResponseMessage> GetAllContact()
        {
            var contacts = new List<EmployeeContacts>
            {
                new EmployeeContacts{
                    Id=Guid.NewGuid(),
                    Name="Ranjan Tiwari",
                    Type="Personal",
                    Mobile="9919965523",
                    Phone="044233289",
                    Address="RVM"
                },
                new EmployeeContacts{
                    Id=Guid.NewGuid(),
                    Name="Ankit Rahevar",
                    Type="Office",
                    Mobile="87868986869",
                    Phone="044233289",
                    Address="B1, Microsoft Campus, Gachibowli"
                },

                new EmployeeContacts{
                    Id=Guid.NewGuid(),
                    Name="Jitendra Kumar",
                    Type="Personal",
                    Mobile="9919965523",
                    Phone="044233289",
                    Address="RVM"
                },

                new EmployeeContacts{
                    Id=Guid.NewGuid(),
                    Name="Rajeev Kumar",
                    Type="Office",
                    Mobile="98903890284",
                    Phone="044233289",
                    Address="SSKC, Microsoft, Hitech City"
                }
            };

            return this.Request.CreateResponse(HttpStatusCode.OK, contacts);
        }
    }
}
