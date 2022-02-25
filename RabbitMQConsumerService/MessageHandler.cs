using System;
using Shared;

namespace RabbitMQConsumerService
{
    public class MessageHandler : IMessageHandler<Message>
    {
        private readonly string timeFormat = "hh:mm:ss.ffffff";
        private int messageNumber = 1;

        public void Invoke(Message message)
        {
            Console.WriteLine(
                $"{messageNumber}. Message | Sent: {message.Timestamp.ToString(timeFormat)} | Received: {DateTime.Now.ToString(timeFormat)}"
            );
            if (message.LastMessage) {
                Console.WriteLine("---");
            }
            messageNumber = message.LastMessage ? 1 : messageNumber + 1;
        }
    }
}