#!/bin/sh

KAFKA_TOPIC=kafkaTopic

SCRIPT_DIR=$(realpath $(dirname "$0"))
ROOT_DIR=${SCRIPT_DIR}/../

docker network create rabbitmq || true
docker network create kafka || true

docker-compose -f ${ROOT_DIR}/docker-compose.publisher-service.yaml up -d --build
docker-compose -f ${ROOT_DIR}/docker-compose.rabbitmq.yaml up -d --build
docker-compose -f ${ROOT_DIR}/docker-compose.rabbitmq-consumer-service.yaml up -d --build
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
docker-compose -f ${ROOT_DIR}/docker-compose.kafka-consumer-service.yaml up -d --build

echo ""
echo "[*] Done"
echo ""
echo "open http://localhost:9000/swagger/index.html"
echo ""