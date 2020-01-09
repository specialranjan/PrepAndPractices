
namespace Asp.Net.Mvc.Operations.Crud.Sdk.DbEntities
{
    using Newtonsoft.Json;

    public class Employee
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
                
        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }

        
        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }
    }
}