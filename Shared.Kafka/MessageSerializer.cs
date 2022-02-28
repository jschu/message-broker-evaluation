using System.Text.Json;
using Confluent.Kafka;

namespace Shared.Kafka
{
    public class MessageSerializer : ISerializer<Message>
    {
        public byte[] Serialize(Message data, SerializationContext context)
        {
            return JsonSerializer.SerializeToUtf8Bytes(data);
        }
    }
}