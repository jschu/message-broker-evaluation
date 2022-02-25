using System;
using System.Text.Json;
using Confluent.Kafka;

namespace Shared.Kafka
{
    public class MessageDeserializer : IDeserializer<Message>
    {
        public Message Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return JsonSerializer.Deserialize<Message>(data);
        }
    }
}