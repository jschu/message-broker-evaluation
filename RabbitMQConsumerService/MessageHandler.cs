using System;
using System.Collections.Generic;
using Shared;

namespace RabbitMQConsumerService
{
    public class MessageHandler : IMessageHandler<Message>
    {
        private int messageNumber = 1;
        private DateTime firstMessageSent;
        private List<MessageEvaluation> evaluations = new List<MessageEvaluation>();

        public void Invoke(Message message)
        {
            MessageEvaluation evaluation = new MessageEvaluation(message.Timestamp, DateTime.Now);
            evaluations.Add(evaluation);
            Console.WriteLine(
                $"{messageNumber}. Message | Sent: {evaluation.SentTimestamp} | Received: {evaluation.ReceivedTimestamp} | Latency: {evaluation.Latency}ms"
            );
            if (messageNumber == 1)
            {
                firstMessageSent = message.Timestamp;
            }
            if (message.LastMessage)
            {
                Console.WriteLine($"Latency Median: {evaluations.LatencyMedian()}ms");
                Console.WriteLine($"Latency Average: {evaluations.LatencyAverage()}ms");
                Console.WriteLine($"Latency Standard Deviation: {evaluations.LatencyStandardDeviation()}ms");
                Console.WriteLine($"Throughput: {evaluations.Throughput(firstMessageSent, message.Timestamp)} Messages / s");
                evaluations.Clear();
                Console.WriteLine("---");
            }
            messageNumber = message.LastMessage ? 1 : messageNumber + 1;
        }
    }
}