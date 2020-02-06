using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Service.Bus.Receiver
{
    class Program
    {
        static ISubscriptionClient subscriptionClient;
        static void Main(string[] args)
        {
            Console.WriteLine("Listener started...");
            GetMessagesAsync().GetAwaiter().GetResult();
            Console.ReadLine();
        }

        private static async Task GetMessagesAsync()
        {
            var serviceBusConnectionString = ConfigurationManager.AppSettings["ServiceBusTopicSubscriptionSenderSharedAccessPolicy"];
            var topicName = ConfigurationManager.AppSettings["ServiceBusTopicName"];
            var subscriptionName = ConfigurationManager.AppSettings["ServiceBusTopicSubscriptionName"];
            subscriptionClient = new SubscriptionClient(serviceBusConnectionString, topicName, subscriptionName);
            RegisterOnMessageHandlerAndReceiveMessages();
            //await subscriptionClient.CloseAsync().ConfigureAwait(false);
        }

        private static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            //var messageHandlerOption = new MessageHandlerOptions(ExceptionReceivedHandlerAsync);
            //subscriptionClient.RegisterMessageHandler(ProcessMessageAsync, messageHandlerOption);
            var sessionHandlerOptions = new SessionHandlerOptions(ExceptionReceivedHandlerAsync)
            {
                MaxConcurrentSessions = 100,
                AutoComplete = true
            };
            subscriptionClient.RegisterSessionHandler(ProcessMessagesInSessionAsync, sessionHandlerOptions);            
        }

        private static async Task ProcessMessagesInSessionAsync(IMessageSession messageSession, Message message, CancellationToken token)
        {
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");
            await Task.CompletedTask;
        }

        private static async Task ExceptionReceivedHandlerAsync(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var exception = exceptionReceivedEventArgs.Exception;
            await Task.CompletedTask;
        }

        static async Task ProcessMessageAsync(Message message, CancellationToken cancellationToken)
        {
            var messageBody = Encoding.UTF8.GetString(message.Body);
            var serviceBusMessage = JsonConvert.DeserializeObject<ServiceBusMessage>(messageBody);
            Console.WriteLine($"Received message: UserData: {messageBody}");
            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        class ServiceBusMessage
        {
            public string Id { get; set; }
            public string Type { get; set; }
            public string Content { get; set; }
        }
    }
}
