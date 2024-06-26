using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public static class MessageEvaluationExtensions
    {
        private static readonly int precision = 2;
        private static readonly int throughputPrecision = 2;

        public static double LatencyMedian(this IEnumerable<MessageEvaluation> evaluations)
        {
            var sortedLatencies = evaluations.Select(eval => eval.Latency).OrderBy(l => l).ToList();
            int size = sortedLatencies.Count();
            int mid = (size / 2);
            double median = (size % 2 == 1)
                ? sortedLatencies.ElementAt(mid)
                : (sortedLatencies.ElementAt(mid) + sortedLatencies.ElementAt(mid - 1)) / 2;
            return Math.Round(median, precision);
        }

        public static double LatencyLowerQuartile(this IEnumerable<MessageEvaluation> evaluations)
        {
            double median = evaluations.LatencyMedian();
            if (evaluations.Count() == 1)
            {
                return median;
            }
            double lowerQuartile = evaluations.Where(eval => eval.Latency < median).LatencyMedian();
            return Math.Round(lowerQuartile, precision);
        }

        public static double LatencyUpperQuartile(this IEnumerable<MessageEvaluation> evaluations)
        {
            double median = evaluations.LatencyMedian();
            if (evaluations.Count() == 1)
            {
                return median;
            }
            double lowerQuartile = evaluations.Where(eval => eval.Latency > median).LatencyMedian();
            return Math.Round(lowerQuartile, precision);
        }

        public static double LatencyAverage(this IEnumerable<MessageEvaluation> evaluations)
        {
            double average = evaluations.Select(eval => eval.Latency).Sum() / evaluations.Count();
            return Math.Round(average, precision);
        }

        public static double LatencyStandardDeviation(this IEnumerable<MessageEvaluation> evaluations)
        {
            if (evaluations.Count() < 2) 
            {
                return 0.0;
            }
            double latencyAverage = evaluations.LatencyAverage();
            double sumOfSquares = evaluations.Select(eval => Math.Pow(eval.Latency - latencyAverage, 2)).Sum();
            double standardDeviation = Math.Sqrt(sumOfSquares / (evaluations.Count() - 1));
            return Math.Round(standardDeviation, precision);
        }

        public static double Throughput(this IEnumerable<MessageEvaluation> evaluations)
        {
            DateTime firstMessageSent = evaluations.Select(eval => eval.Sent).Min();
            DateTime lastMessageReceived = evaluations.Select(eval => eval.Received).Max();
            double durationAllMessages = lastMessageReceived.Subtract(firstMessageSent).Ticks / (double) TimeSpan.TicksPerSecond;
            double throughput = evaluations.Count() / durationAllMessages;
            return Math.Round(throughput, throughputPrecision);
        }

        public static double TotalDuration(this IEnumerable<MessageEvaluation> evaluations)
        {
            DateTime firstMessageSent = evaluations.Select(eval => eval.Sent).Min();
            DateTime lastMessageReceived = evaluations.Select(eval => eval.Received).Max();
            double durationAllMessages = lastMessageReceived.Subtract(firstMessageSent).Ticks / (double) TimeSpan.TicksPerSecond;
            return Math.Round(durationAllMessages * 1000, precision);
        }
    }
}