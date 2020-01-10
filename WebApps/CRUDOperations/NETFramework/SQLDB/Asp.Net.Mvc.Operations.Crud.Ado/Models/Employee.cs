using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Web.Mvc;
using System.Linq;

namespace Asp.Net.Mvc.Operations.Crud.Ado.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]        
        [Range(0,int.MaxValue,ErrorMessage = "Invalid value range for field age.")]
        public int? Age { get; set; }
        
        [Required(ErrorMessage ="The Gender field is required")]        
        public string SelectedGender { get; set; }
        public IEnumerable<SelectListItem> Gender { get { return this.GetGenders(); } }

        public List<Employee> GetEmployee(string filterBy = null, string filterByValue = null)
        {
            SqlHelper sqlHelper = new SqlHelper();
            try
            {
                var employeeTable =  sqlHelper.ExecuteProcedure(filterBy, filterByValue);
                return this.ConvertDataTableToList(employeeTable);
            }
            catch (Exception ex)
            {
                return new List<Employee>();
            }
        }

        public int UpdateEmployee(string operationsType, int? employeeId = null, FormCollection collection = null)
        {
            SqlHelper sqlHelper = new SqlHelper();
            SqlParameter[] parameters;

            try
            {
                if (operationsType == "DELETE")
                {
                    parameters = new SqlParameter[2] {
                        new SqlParameter("@OPERATION_TYPE",operationsType),
                        new SqlParameter("@ID", employeeId)
                    };
                }
                else if (operationsType == "INSERT")
                {
                    parameters = new SqlParameter[4] {
                        new SqlParameter("@OPERATION_TYPE",operationsType),
                        new SqlParameter("@NAME", collection["Name"]),
                        new SqlParameter("@AGE", Convert.ToInt32(collection["Age"])),
                        new SqlParameter("@GENDER", Convert.ToString(collection["SelectedGender"]))
                    };
                }
                else
                {
                    parameters = new SqlParameter[5] {
                        new SqlParameter("@OPERATION_TYPE",operationsType),
                        new SqlParameter("@ID", employeeId),
                        new SqlParameter("@NAME", collection["Name"]),
                        new SqlParameter("@AGE", Convert.ToInt32(collection["Age"])),
                        new SqlParameter("@GENDER", Convert.ToString(collection["SelectedGender"]))
                    };
                }
                
                return sqlHelper.ExecuteNonQuery(parameters);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        private List<Employee> ConvertDataTableToList(DataTable table)
        {
            List<Employee> employees = (from DataRow row in table.Rows
                         select new Employee()
                         {
                             Id = Convert.ToInt32(row["Id"]),
                             Name = Convert.ToString(row["Name"]),
                             Age = Convert.ToInt32(row["Age"]),
                             SelectedGender = Convert.ToString(row["Gender"])
                         }).ToList();
            return employees;
        }

        private IEnumerable<SelectListItem> GetGenders()
        {
            List<SelectListItem> genders = new List<SelectListItem>()
            {
                new SelectListItem{ Value=null, Text="-- Select Gender --"},
                new SelectListItem{ Value="Male", Text="Male"},
                new SelectListItem{ Value="Female", Text="Female"}
            };

            return new SelectList(genders, "Value", "Text");
        }
    }
}