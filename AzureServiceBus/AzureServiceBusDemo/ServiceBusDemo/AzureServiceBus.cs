using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System.Threading.Tasks;
using System.Text;

namespace ServiceBusDemo
{
    public class AzureServiceBus
    {
        const string serviceBusConnectionString = "insert connection string here";

        public static async Task SendMessageAsync(string message, string queueName)
        {
            var queueClient = new QueueClient(serviceBusConnectionString, queueName);

            var encodedMessage = new Message(Encoding.UTF8.GetBytes(message));

            await queueClient.SendAsync(encodedMessage);

            await queueClient.CloseAsync();
        }

        public static async Task<string> ReadMessageAsync(string queueName)
        {
            var messageReceiver = new MessageReceiver(serviceBusConnectionString, 
                queueName, ReceiveMode.ReceiveAndDelete);

            var message = await messageReceiver.ReceiveAsync();

            await messageReceiver.CloseAsync();

            return Encoding.UTF8.GetString(message.Body);
        }
    }
}
