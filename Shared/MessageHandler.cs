using System;
using System.Collections.Generic;

namespace Shared
{
    public class MessageHandler : IMessageHandler<Message>
    {
        private readonly List<MessageEvaluation> evaluations = [];

        public void Invoke(Message message)
        {
            MessageEvaluation evaluation = new(message.Timestamp, DateTime.Now);
            evaluations.Add(evaluation);
            Console.WriteLine(
                $"{message.MessageNumber}. Message | Sent: {evaluation.SentTimestamp} | Received: {evaluation.ReceivedTimestamp} | Latency: {evaluation.Latency}ms"
            );
            if (message.MessageNumber == message.NumberOfMessages)
            {
                Console.WriteLine($"Latency Lower Quartile: {evaluations.LatencyLowerQuartile()}ms");
                Console.WriteLine($"Latency Median: {evaluations.LatencyMedian()}ms");
                Console.WriteLine($"Latency Upper Quartile: {evaluations.LatencyUpperQuartile()}ms");
                Console.WriteLine($"Latency Average: {evaluations.LatencyAverage()}ms");
                Console.WriteLine($"Latency Standard Deviation: {evaluations.LatencyStandardDeviation()}ms");
                Console.WriteLine($"Throughput: {evaluations.Throughput()} Messages / s");
                Console.WriteLine("---");
                evaluations.Clear();
            }
        }
    }
}