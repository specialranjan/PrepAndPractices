namespace Common.Data.AzureStorage.Blob
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    public sealed class BlobLeaseClient : IDisposable
    {
        private readonly ICloudBlob blob;
        private readonly string leaseId;
        private readonly AutoResetEvent canProceedStop;
        private readonly CancellationTokenSource cancellationTokenSource;
        public BlobLeaseClient(ICloudBlob blob, string leaseId)
        {
            //Requires.NotNull(blob, nameof(blob));
            //Requires.NotNullOrWhiteSpace(leaseId, nameof(leaseId));

            this.blob = blob;
            this.leaseId = leaseId;

            this.canProceedStop = new AutoResetEvent(false);
            this.cancellationTokenSource = new CancellationTokenSource();

            Task.Factory.StartNew(() => this.RenewLease(this.cancellationTokenSource.Token),
                TaskCreationOptions.LongRunning);
        }
        public void Dispose()
        {
            this.cancellationTokenSource.Cancel();

            const int TimeWait = 5;
            this.canProceedStop.WaitOne(TimeSpan.FromSeconds(TimeWait));

            this.blob.ReleaseLease(AccessCondition.GenerateLeaseCondition(this.leaseId));

            this.cancellationTokenSource.Dispose();
            this.canProceedStop.Dispose();
        }
        private void RenewLease(CancellationToken token)
        {
            var nextRenew = DateTime.UtcNow.AddSeconds(18);
            while (!token.IsCancellationRequested)
            {
                if (nextRenew <= DateTime.UtcNow)
                {
                    this.blob.RenewLease(AccessCondition.GenerateLeaseCondition(this.leaseId));
                    const int AddSecondsUtc = 18;
                    nextRenew = DateTime.UtcNow.AddSeconds(AddSecondsUtc);
                }

                const int ThreadSleepTime = 200;
                Thread.Sleep(ThreadSleepTime);
            }

            this.canProceedStop.Set();
        }
    }
}
