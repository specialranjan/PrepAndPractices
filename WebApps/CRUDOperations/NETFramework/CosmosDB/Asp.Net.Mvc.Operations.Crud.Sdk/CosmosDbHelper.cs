namespace Asp.Net.Mvc.Operations.Crud.Sdk
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net;
    using System.Threading.Tasks;
    using Asp.Net.Mvc.Operations.Crud.Sdk.DbEntities;
    using Microsoft.Azure.Cosmos;

    public class CosmosDbHelper
    {
        CosmosClient cosmosClient;
        Database db;
        Container container;
        private static readonly string databaseId = "EmployeeDb", containerId = "EmployeeContacts";


        public CosmosDbHelper()
        {
            cosmosClient = new CosmosClient(ConfigurationManager.AppSettings["CosmosDb:Uri"],
                ConfigurationManager.AppSettings["CosmosDb:PrimaryKey"],
                new CosmosClientOptions()
                {
                    ApplicationRegion = Regions.WestUS
                });
            this.db = this.cosmosClient.GetDatabase(databaseId);
            this.container = this.db.GetContainer(containerId);
        }

        public async Task<List<Employee>> QueryItemsAsync(string queryText)
        {
            QueryDefinition queryDefinition = new QueryDefinition(queryText);
            FeedIterator<Employee> feedIterator = this.container.GetItemQueryIterator<Employee>(queryDefinition);
            List<Employee> employees = new List<Employee>();
            while (feedIterator.HasMoreResults)
            {
                FeedResponse<Employee> employeeFeedResponse = await feedIterator.ReadNextAsync();
                foreach (Employee employee in employeeFeedResponse)
                {
                    employees.Add(employee);
                }
            }

            return employees;
        }

        public async Task<bool> CreateItemAsync(Employee employee)
        {
            bool itemCreated;
            try
            {
                await this.container.ReadItemAsync<Employee>(employee.Id, new PartitionKey(employee.Id));
                itemCreated = false;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                await this.container.CreateItemAsync<Employee>(employee, new PartitionKey(employee.Id));
                itemCreated = true;
            }

            return itemCreated;
        }
    }
}