using System;
using EasyNetQ;

namespace Shared.RabbitMQ
{
    public class EasyNetQMessageBus : IMessageBus
    {
        private readonly string subscriptionId = "rabbitmq";
        private readonly IBus bus;

        public EasyNetQMessageBus(IBus bus)
        {
            this.bus = bus;
        }

        public void Publish<TMessage>(TMessage message)
        {
            bus.PubSub.Publish(message);
        }

        public IDisposable Subscribe<TMessage, TMessageHandler>(TMessageHandler messageHandler) where TMessageHandler : IMessageHandler<TMessage>
        {
            return bus.PubSub.Subscribe<TMessage>(subscriptionId, messageHandler.Invoke);
        }
    }
}
