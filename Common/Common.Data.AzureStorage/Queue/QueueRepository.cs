using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace Common.Data.AzureStorage.Queue
{
    public class QueueRepository : IQueueRepository
    {
        private readonly CloudQueueClient cloudQueueClient;

        /// <summary>
        /// Constructor that Create Cloud Queue Client
        /// </summary>
        /// <param name="queueName">The name of the queue to be managed</param>
        /// <param name="storageConnectionString">The connection string pointing to the storage account (this can be local or hosted in Windows Azure</param>
        public QueueRepository(string storageConnectionString)
        {
            //Validate.String(storageConnectionString, nameof(storageConnectionString));

            var cloudStorageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var requestOptions = new QueueRequestOptions
            {
                RetryPolicy = new ExponentialRetry(TimeSpan.FromMilliseconds(200), 3)
            };

            cloudQueueClient = cloudStorageAccount.CreateCloudQueueClient();
            cloudQueueClient.DefaultRequestOptions = requestOptions;
        }

        /// <summary>
        /// Get a reference of the Queue or Create new one if it does not exists
        /// </summary>
        /// <param name="queueName">The name of the Queue to Get or Create</param>
        /// <returns></returns>
        public async Task<CloudQueue> CreateIfNotExistsAsync(string queueName)
        {
            //Validate.QueueName(queueName, nameof(queueName));

            var cloudQueue = cloudQueueClient.GetQueueReference(queueName);
            await cloudQueue.CreateIfNotExistsAsync().ConfigureAwait(false);
            return cloudQueue;
        }

        /// <summary>
        /// Deletes the current queue
        /// </summary>
        /// <returns></returns>
        public async Task DeleteQueueAsync(string queueName)
        {
            //Validate.QueueName(queueName, nameof(queueName));

            // Get Queue reference
            var cloudQueue = cloudQueueClient.GetQueueReference(queueName);
            await cloudQueue.DeleteIfExistsAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new queue message with the given content and adds it to the queue
        /// </summary>
        /// <param name="content">The content to add to the queue message</param>
        public async Task EnQueueAsync(byte[] content, string queueName)
        {
            //Validate.Null(content, nameof(content));

            var cloudQueueMessage = new CloudQueueMessage(content);
            // Get Queue reference
            var cloudQueue = cloudQueueClient.GetQueueReference(queueName);
            await cloudQueue.AddMessageAsync(cloudQueueMessage).ConfigureAwait(false);
        }

        /// <summary>
        /// Peek at the message in front of the queue without removing it
        /// </summary>
        /// <returns>CloudQueueMessage</returns>
        public async Task<CloudQueueMessage> PeekAsync(string queueName)
        {
            // Get Queue reference
            var cloudQueue = cloudQueueClient.GetQueueReference(queueName);
            return await cloudQueue.PeekMessageAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new queue message with the given content and adds it to the queue
        /// </summary>
        /// <param name="content">The content to add to the queue message</param>
        public async Task EnQueueAsync(string content, string queueName)
        {
            //Validate.String(content, nameof(content));

            var cloudQueueMessage = new CloudQueueMessage(content);
            // Get Queue reference
            var cloudQueue = cloudQueueClient.GetQueueReference(queueName);
            await cloudQueue.AddMessageAsync(cloudQueueMessage).ConfigureAwait(false);
        }

        /// <summary>
        /// The en queue async with time to live and visibility support
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <param name="queueName">
        /// The queue name.
        /// </param>
        /// <param name="timeToLive">
        /// The time to live.
        /// </param>
        /// <param name="initialVisibilityDelay">
        /// The initial visibility delay.
        /// </param>
        /// <param name="queueRequestOptions">
        /// The queue request options.
        /// </param>
        /// <param name="operationContext">
        /// The operation context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task EnQueueAsync(string content, string queueName, TimeSpan? timeToLive, TimeSpan? initialVisibilityDelay, QueueRequestOptions queueRequestOptions, OperationContext operationContext)
        {
            //Validate.String(content, nameof(content));

            var cloudQueueMessage = new CloudQueueMessage(content);
            // Get Queue reference
            var cloudQueue = cloudQueueClient.GetQueueReference(queueName);
            await cloudQueue.AddMessageAsync(cloudQueueMessage, timeToLive, initialVisibilityDelay, queueRequestOptions, operationContext).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns the next item on the queue. Note that this will not delete the message from the queue.
        /// </summary>
        /// <returns>The queue message</returns>
        public async Task<CloudQueueMessage> DeQueueAsync(string queueName)
        {
            // Get Queue reference
            var cloudQueue = cloudQueueClient.GetQueueReference(queueName);
            return await cloudQueue.GetMessageAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Returns the batch of messages on the queue. Note that this will not delete the message from the queue.
        /// </summary>
        /// <returns>The queue messages</returns>
        public async Task<IEnumerable<CloudQueueMessage>> DeQueueMessagesAsync(string queueName, int batchSize, TimeSpan? visibiltyTimeOut, CancellationToken cancellationToken)
        {
            // Get Queue reference
            var cloudQueue = cloudQueueClient.GetQueueReference(queueName);
            return await cloudQueue.GetMessagesAsync(batchSize, visibiltyTimeOut, cloudQueueClient.DefaultRequestOptions, null, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// change the contents of a message in-place in the queue
        /// </summary>
        /// <param name="content"></param>
        public async Task UpdateQueueMessageAsync(string content, string queueName)
        {
            // Get Queue reference
            var cloudQueue = cloudQueueClient.GetQueueReference(queueName);
            var message = cloudQueue.GetMessage();
            message.SetMessageContent(content);
            await cloudQueue.UpdateMessageAsync(message, TimeSpan.FromSeconds(0.0), MessageUpdateFields.Content | MessageUpdateFields.Visibility).ConfigureAwait(false);
        }

        /// <summary>
        /// The update queue message async.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="queueName">
        /// The queue name.
        /// </param>
        /// <param name="visibility">
        /// The visibility.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task RequeueMessageWithVisibilityAsync(CloudQueueMessage message, string queueName, TimeSpan visibility)
        {
            // Get Queue reference
            var cloudQueue = this.cloudQueueClient.GetQueueReference(queueName);
            await cloudQueue.UpdateMessageAsync(message, visibility, MessageUpdateFields.Content | MessageUpdateFields.Visibility).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes the given queue message from the queue
        /// </summary>
        /// <param name="cloudQueueMessage">The queue message to delete</param>
        public async Task DeleteMessageAsync(CloudQueueMessage cloudQueueMessage, string queueName)
        {
            //Validate.Null(cloudQueueMessage, nameof(cloudQueueMessage));

            // Get Queue reference
            var cloudQueue = cloudQueueClient.GetQueueReference(queueName);
            await cloudQueue.DeleteMessageAsync(cloudQueueMessage).ConfigureAwait(false);
        }

        /// <summary>
        /// Clears all messages from the current queue
        /// </summary>
        /// <returns></returns>
        public async Task ClearQueueAsync(string queueName)
        {
            // Get Queue reference
            var cloudQueue = cloudQueueClient.GetQueueReference(queueName);
            await cloudQueue.ClearAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// The Count of message in the current Queue
        /// </summary>
        /// <returns></returns>
        public int MessageCount(string queueName)
        {
            // Get Queue reference
            var cloudQueue = cloudQueueClient.GetQueueReference(queueName);
            return cloudQueue.ApproximateMessageCount ?? 0;
        }
    }
}
