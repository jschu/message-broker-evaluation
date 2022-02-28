using System.Text.Json;

namespace Shared.Redis
{
    public static class MessageDeserializer
    {
        public static Message Deserialize(string data)
        {
            return JsonSerializer.Deserialize<Message>(data);
        }
    }
}