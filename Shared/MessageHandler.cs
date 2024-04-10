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
            if (message.MessageNumber == 1)
            {
                Console.WriteLine("Start receiving messages ...");
            }
            // Console.WriteLine(
            //     $"{message.MessageNumber}. Message | Sent: {evaluation.SentTimestamp} | Received: {evaluation.ReceivedTimestamp} | Latency: {evaluation.Latency}ms"
            // );
            if (message.MessageNumber == message.NumberOfMessages)
            {
                Console.WriteLine("Received 100%");
                Console.WriteLine("---");
                Console.WriteLine($"Number of Messages: {message.NumberOfMessages}");
                Console.WriteLine($"Total Duration: {evaluations.TotalDuration()}ms");
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