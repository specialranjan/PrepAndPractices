using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.Net.Mvc.Operations.Crud.Ado.Models;

namespace Asp.Net.Mvc.Operations.Crud.Ado.Controllers
{    
    public class EmployeeController : Controller
    {
        SqlHelper sqlHelper;
        
        // GET: Employee
        public ActionResult Index()
        {
            this.sqlHelper = new SqlHelper();
            var employees = this.sqlHelper.GetTable("SELECT * FROM EMPLOYEE");
            return View("Index", employees);
        }
        
        // GET: Employee/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            this.sqlHelper = new SqlHelper();
            var employee = this.sqlHelper.GetTable("SELECT * FROM EMPLOYEE WHERE ID=@ID", new SqlParameter("@ID", id));
            return View("Details", employee);
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
                SqlParameter[] parameters = new SqlParameter[3] {
                    new SqlParameter("@NAME", collection["Name"]),
                    new SqlParameter("@AGE", Convert.ToInt32(collection["Age"])),
                    new SqlParameter("@GENDER", Enum.GetName(typeof(Asp.Net.Mvc.Operations.Crud.Ado.Models.Gender), Convert.ToInt16(collection["Gender"])))
                };
                this.sqlHelper = new SqlHelper();
                this.sqlHelper.UpdateTable("INSERT INTO EMPLOYEE(NAME,AGE,GENDER) VALUES(@NAME,@AGE,@GENDER)", parameters);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            this.sqlHelper = new SqlHelper();
            var employeeTable = this.sqlHelper.GetTable("SELECT * FROM EMPLOYEE WHERE ID=@ID", new SqlParameter("@ID", id));
            var employees = employeeTable.AsEnumerable().Select(
                row => new Employee
                {
                    Id = row.Field<int>("Id"),
                    Name = row.Field<string>("Name"),
                    Age = row.Field<int>("Age"),
                    Gender = (Gender)Enum.Parse(typeof(Gender), row.Field<string>("Gender"))
                }).ToList();
            Employee employee = employees[0];
            return View("Edit", employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[4] {
                    new SqlParameter("@ID", id),
                    new SqlParameter("@NAME", collection["Name"]),
                    new SqlParameter("@AGE", Convert.ToInt32(collection["Age"])),
                    new SqlParameter("@GENDER", Enum.GetName(typeof(Asp.Net.Mvc.Operations.Crud.Ado.Models.Gender), Convert.ToInt16(collection["Gender"])))
                };
                this.sqlHelper = new SqlHelper();
                this.sqlHelper.UpdateTable("UPDATE EMPLOYEE SET NAME=@NAME,AGE=@AGE,GENDER=@GENDER WHERE ID=@ID", parameters);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        // POST: Employee/Delete/5
        [Route("delete/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                this.sqlHelper = new SqlHelper();
                this.sqlHelper.UpdateTable("DELETE FROM EMPLOYEE WHERE ID=@ID", new SqlParameter("@ID", id));
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        ~EmployeeController()
        {
            this.sqlHelper = null;
        }
    }
}
