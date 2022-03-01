namespace Shared.Kafka
{
    public static class Constants
    {
        public static string Topic => "kafkaTopic";
        public static int MessageMaxBytes = 128 * 1024 * 1024;
    }
}