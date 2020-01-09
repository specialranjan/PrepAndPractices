using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Asp.Net.Mvc.Operations.Crud.Sdk.Models
{
    public class Employee
    {
        [JsonProperty(PropertyName ="id")]
        public Guid Id { get; set; }

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