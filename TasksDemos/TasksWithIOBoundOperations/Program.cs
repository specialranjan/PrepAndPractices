namespace TasksWithIOBoundOperations
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    class Program
    {
        static void Main(string[] args)
        {
            Task<string> task = Task.Factory.StartNew<string>(() => GetPosts("https://jsonplaceholder.typicode.com/posts"));
            SomethingElse();
            
            try
            {
                task.Wait();
                Console.WriteLine(task.Result);
            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }

        private static void SomethingElse()
        {
            //Do something
        }

        private static string GetPosts(string url)
        {
            if (url == null)
            {
                throw null;
            }
            using (var client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }
}
