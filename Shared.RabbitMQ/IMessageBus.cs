using System;

namespace Shared.RabbitMQ
{
     public interface IMessageBus
    {
        public void Publish<TMessage>(TMessage message);

        public IDisposable Subscribe<TMessage, TMessageHandler>(string subscriptionId, TMessageHandler messageHandler) where TMessageHandler : IMessageHandler<TMessage>;
    }
}
