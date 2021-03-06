#!/bin/sh

KAFKA_TOPIC=kafkaTopic

SCRIPT_DIR=$(realpath $(dirname "$0"))
ROOT_DIR=${SCRIPT_DIR}/../

usage()
{
  echo "Usage:"
  echo "  e.g: sh $0 [--no-rabbitmq]"
  echo ""
  echo "  --no-rabbitmq starts without required services to evaluate RabbitMQ"
  echo "  --no-kafka starts without required services to evaluate Apache Kafka"
  echo "  --no-redis starts without required services to evaluate Redis"
  echo "  --no-publisher-service starts without Publisher Service"
  echo ""
}

rabbitmq_consumer_instances=1
kafka_consumer_instances=1
redis_consumer_instances=1

for arg in "$@"
do
  case $arg in
    --no-rabbitmq)
      no_rabbitmq="true"
      shift
      ;;
    --no-kafka)
      no_kafka="true"
      shift
      ;;
    --no-redis)
      no_redis="true"
      shift
      ;;
    --no-publisher-service)
      no_publisher_service="true"
      shift
      ;;
    --rabbitmq-consumer-instances=*)
      rabbitmq_consumer_instances="${arg#*=}"
      shift
      ;;
    --kafka-consumer-instances=*)
      kafka_consumer_instances="${arg#*=}"
      shift
      ;;
    --redis-consumer-instances=*)
      redis_consumer_instances="${arg#*=}"
      shift
      ;;
    -h|--help)
      usage
      exit 0
      ;;
    *)
      OTHER_ARGUMENTS+=("$1")
      shift # Remove generic argument from processing
      ;;
  esac
done

docker network create rabbitmq || true
docker network create kafka || true
docker network create redis || true

if [ "${no_publisher_service}" != "true" ]; then
    docker-compose -f ${ROOT_DIR}/docker-compose.publisher-service.yaml up -d --build
fi

if [ "${no_rabbitmq}" != "true" ]; then
    docker-compose -f ${ROOT_DIR}/docker-compose.rabbitmq.yaml up -d --build
    docker-compose -f ${ROOT_DIR}/docker-compose.rabbitmq-consumer-service.yaml up -d --build --scale rabbitmq-consumer-service=${rabbitmq_consumer_instances}
fi

if [ "${no_redis}" != "true" ]; then
    docker-compose -f ${ROOT_DIR}/docker-compose.redis.yaml up -d --build
    docker-compose -f ${ROOT_DIR}/docker-compose.redis-consumer-service.yaml up -d --build --scale redis-consumer-service=${redis_consumer_instances}
fi

if [ "${no_kafka}" != "true" ]; then
    docker-compose -f ${ROOT_DIR}/docker-compose.kafka.yaml up -d --build
    echo ""
    echo "Waiting for Kafka broker to start"
    docker logs -f kafka 2>&1 | grep -q "started (kafka.server.KafkaServer)"
    echo "Kafka container started"
    echo ""
    echo "Creating kafka topic: ${KAFKA_TOPIC}"
    docker exec -it kafka /bin/kafka-topics --create --if-not-exists --bootstrap-server localhost:9092 --replication-factor 1 --partitions 1 --topic ${KAFKA_TOPIC}
    echo ""
    echo "Starting Kafka Consumer Service"
    echo ""
    docker-compose -f ${ROOT_DIR}/docker-compose.kafka-consumer-service.yaml up -d --build --scale kafka-consumer-service=${kafka_consumer_instances}
fi

echo ""
echo "[*] Done"
echo ""

if [ "${no_publisher_service}" != "true" ]; then
    echo "open http://localhost:9000/swagger/index.html"
    echo ""
fi