using System;

namespace RabbitMQConsumerService
{
    public record MessageEvaluation(DateTime Sent, DateTime Received)
    {
        private readonly string timeFormat = "hh:mm:ss.ffffff";
        public double Latency => Received.Subtract(Sent).Ticks / (double) TimeSpan.TicksPerMillisecond;

        public string SentTimestamp => Sent.ToString(timeFormat);
        public string ReceivedTimestamp => Received.ToString(timeFormat);
    }
}