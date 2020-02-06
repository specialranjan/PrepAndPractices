using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace Service.Bus.Sender
{
    class Program
    {
        private static ITopicClient topicClient;
        static void Main(string[] args)
        {
            var serviceBusConnectionString = ConfigurationManager.AppSettings["ServiceBusTopicSubscriptionSenderSharedAccessPolicy"];
            var topicName = ConfigurationManager.AppSettings["ServiceBusTopicName"];
            topicClient = new TopicClient(serviceBusConnectionString, topicName);
            SendMessageAsync().GetAwaiter().GetResult();
            Console.ReadKey();
        }

        static async Task SendMessageAsync()
        {
            User user = null;
            for (int i = 1; i <= 5; i++)
            {
                user = new User() { Id = 1, Name = string.Format("User{0}", i) };

                var serializeUsers = JsonConvert.SerializeObject(user);
                var messageType = "UserData";
                var messageId = Guid.NewGuid().ToString();
                var message = new ServiceBusMessage()
                {
                    Id = messageId,
                    Type = messageType,
                    Content = serializeUsers
                };
                var serializeMessage = JsonConvert.SerializeObject(message);
                var busMessage = new Message(Encoding.UTF8.GetBytes(serializeMessage));
                busMessage.UserProperties.Add("Type", messageType);
                busMessage.SessionId = Guid.NewGuid().ToString();
                await topicClient.SendAsync(busMessage).ConfigureAwait(false);
                Console.WriteLine("{0} Message has been sent.", i);
            }
            
            await topicClient.CloseAsync().ConfigureAwait(false);
            
        }

        class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        class ServiceBusMessage
        {
            public string Id { get; set; }
            public string Type { get; set; }
            public string Content { get; set; }
        }
    }
}
