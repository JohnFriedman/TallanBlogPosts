using ServiceBusDemo;
using System;
using System.Threading.Tasks;

namespace ReaderApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var message = await AzureServiceBus.ReadMessageAsync("insert queue name here");
            Console.WriteLine($"Message from Sender App: {message}");
            Console.ReadKey();
        }
    }
}
