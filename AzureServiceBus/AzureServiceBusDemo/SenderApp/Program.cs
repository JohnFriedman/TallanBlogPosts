using ServiceBusDemo;
using System;
using System.Threading.Tasks;

namespace SenderApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            Console.WriteLine("Enter your message to send to the other app: ");
            var message = Console.ReadLine();

            await AzureServiceBus.SendMessageAsync(message, "insert queue name here");
        }
    }
}
