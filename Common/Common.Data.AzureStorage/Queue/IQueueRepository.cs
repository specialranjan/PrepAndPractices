using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Common.Data.AzureStorage.Queue
{
    using Microsoft.WindowsAzure.Storage;

    /// <summary>
    /// Repository interface for Windows Azure Queue
    /// </summary>
    public interface IQueueRepository
    {
        /// <summary>
        /// Get a reference of the Queue or Create new one if it does not exists
        /// </summary>
        /// <param name="queueName">The name of the Queue to Get or Create</param>
        /// <returns></returns>
        Task<CloudQueue> CreateIfNotExistsAsync(string queueName);

        /// <summary>
        /// Deletes the current queue
        /// </summary>
        /// <returns></returns>
        Task DeleteQueueAsync(string queueName);

        /// <summary>
        /// Creates a new queue message with the given content and adds it to the queue
        /// </summary>
        /// <param name="content">The content to add to the queue message</param>
        Task EnQueueAsync(byte[] content, string queueName);

        /// <summary>
        /// Peek at the message in front of the queue without removing it
        /// </summary>
        /// <returns>CloudQueueMessage</returns>
        Task<CloudQueueMessage> PeekAsync(string queueName);

        /// <summary>
        /// Creates a new queue message with the given content and adds it to the queue
        /// </summary>
        /// <param name="content">The content to add to the queue message</param>
        Task EnQueueAsync(string content, string queueName);

        /// <summary>
        /// Creates a new queue message with the given content and adds it to the queue
        /// </summary>
        /// <param name="content">
        /// The content to add to the queue message
        /// </param>
        /// <param name="queueName">
        /// The queue Name.
        /// </param>
        /// <param name="timeToLive">
        /// The time To Live.
        /// </param>
        /// <param name="initialVisibilityDelay">
        /// The initial Visibility Delay.
        /// </param>
        /// <param name="queueRequestOptions">
        /// The queue Request Options.
        /// </param>
        /// <param name="operationContext">
        /// The operation Context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task EnQueueAsync(
            string content,
            string queueName,
            TimeSpan? timeToLive,
            TimeSpan? initialVisibilityDelay,
            QueueRequestOptions queueRequestOptions,
            OperationContext operationContext);

        /// <summary>
        /// Returns the next item on the queue. Note that this will not delete the message from the queue.
        /// </summary>
        /// <returns>The queue message</returns>
        Task<CloudQueueMessage> DeQueueAsync(string queueName);

        /// <summary>
        /// Returns the batch of messages on the queue. Note that this will not delete the message from the queue.
        /// </summary>
        /// <returns>The queue messages</returns>
        Task<IEnumerable<CloudQueueMessage>> DeQueueMessagesAsync(string queueName, int batchSize, TimeSpan? visibiltyTimeOut, CancellationToken cancellationToken);

        /// <summary>
        /// change the contents of a message in-place in the queue
        /// </summary>
        /// <param name="content"></param>
        Task UpdateQueueMessageAsync(string content, string queueName);

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
        Task RequeueMessageWithVisibilityAsync(CloudQueueMessage message, string queueName, TimeSpan visibility);

        /// <summary>
        /// Deletes the given queue message from the queue
        /// </summary>
        /// <param name="cloudQueueMessage">The queue message to delete</param>
        Task DeleteMessageAsync(CloudQueueMessage cloudQueueMessage, string queueName);

        /// <summary>
        /// Clears all messages from the current queue
        /// </summary>
        /// <returns></returns>
        Task ClearQueueAsync(string queueName);

        /// <summary>
        /// The Count of message in the current Queue
        /// </summary>
        /// <returns></returns>
        int MessageCount(string queueName);
    }
}
