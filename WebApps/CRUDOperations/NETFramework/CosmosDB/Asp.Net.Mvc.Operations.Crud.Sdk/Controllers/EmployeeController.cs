using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.Net.Mvc.Operations.Crud.Sdk.DbEntities;

namespace Asp.Net.Mvc.Operations.Crud.Sdk.Controllers
{
    public class EmployeeController : Controller
    {
        CosmosDbHelper db = new CosmosDbHelper();
        // GET: Employee
        public ActionResult Index()
        {
            var employees = db.QueryItemsAsync("SELECT * FROM C").GetAwaiter().GetResult();
            return View("Index", employees);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Employee employee = new Employee()
                {
                    Name = collection["Name"],
                    Age = Convert.ToInt32(collection["Age"]),
                    Gender = Enum.GetName(typeof(Models.Gender), Convert.ToInt16(collection["Gender"]))
                };
                db.CreateItemAsync(employee).GetAwaiter().GetResult();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}