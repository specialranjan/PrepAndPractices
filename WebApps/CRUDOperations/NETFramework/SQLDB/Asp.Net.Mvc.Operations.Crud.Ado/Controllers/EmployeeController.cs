using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asp.Net.Mvc.Operations.Crud.Ado.Models;
using PagedList;

namespace Asp.Net.Mvc.Operations.Crud.Ado.Controllers
{    
    public class EmployeeController : Controller
    {
        Employee employee = new Employee();
        
        // GET: Employee
        public ActionResult Index(int? page, string sortOrder)
        {
            int pageSize = 3, pageNumber = (page ?? 1);            
            ViewBag.CurrentSortOrder = string.IsNullOrEmpty(sortOrder) ? "ID" : sortOrder;
            IEnumerable<Employee> employees = null;
            if (Session["EmployeesData"] == null)
            {
                employees = this.employee.GetEmployee();
                Session["EmployeesData"] = employees;
            }
            else
            {
                employees = (IEnumerable<Employee>)Session["EmployeesData"];                
            }

            switch (sortOrder)
            {
                case "ID":
                    employees = employees.OrderByDescending(e => e.Id);
                    break;
                case "Name":
                    employees = employees.OrderByDescending(e => e.Name);
                    break;
                case "Age":
                    employees = employees.OrderByDescending(e => e.Age);
                    break;
                case "Gender":
                    employees = employees.OrderByDescending(e => e.SelectedGender);
                    break;
            }

            return View("Index", employees.ToPagedList(pageNumber, pageSize));
        }
        
        // GET: Employee/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            Employee employeeData = null;
            if (Session["EmployeesData"] == null)
            {
                employeeData = this.employee.GetEmployee(filterBy: "ID", filterByValue: id.ToString()).FirstOrDefault();
            }
            else
            {
                employeeData = ((IEnumerable<Employee>)Session["EmployeesData"]).FirstOrDefault(e => e.Id == id);
            }

            return View("Details", employeeData);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View("Create", new Employee());
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                this.employee.UpdateEmployee("INSERT", collection: collection);
                Session["EmployeesData"] = null;
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            Employee employeeData = null;
            if (Session["EmployeesData"] == null)
            {
                employeeData = this.employee.GetEmployee(filterBy: "ID", filterByValue: id.ToString()).FirstOrDefault();
            }
            else
            {
                employeeData = ((IEnumerable<Employee>)Session["EmployeesData"]).FirstOrDefault(e => e.Id == id);
            }
            
            return View("Edit", employeeData);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                this.employee.UpdateEmployee("UPDATE", id, collection);
                Session["EmployeesData"] = null;
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
                this.employee.UpdateEmployee("DELETE", id);
                Session["EmployeesData"] = null;
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
