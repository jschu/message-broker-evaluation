using System.Text.Json;

namespace Shared.Redis
{
    public static class MessageSerializer
    {
        public static string Serialize(Message data)
        {
            return JsonSerializer.Serialize(data);
        }
    }
}