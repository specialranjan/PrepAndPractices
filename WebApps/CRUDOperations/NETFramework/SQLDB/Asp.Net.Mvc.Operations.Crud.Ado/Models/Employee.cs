using System;
using System.ComponentModel.DataAnnotations;

namespace Asp.Net.Mvc.Operations.Crud.Ado.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]        
        [Range(0,int.MaxValue,ErrorMessage = "Invalid value range for field age.")]
        public int Age { get; set; }
        
        [Required]  
        [EnumDataType(typeof(Gender),ErrorMessage ="Invalid value for field gender.")]
        public Gender Gender { get; set; }
    }
}