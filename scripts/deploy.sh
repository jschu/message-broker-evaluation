#!/bin/sh

SCRIPT_DIR=$(realpath $(dirname "$0"))
PUBLISHER_SERVICE_DIR=${SCRIPT_DIR}/../PublisherService
RABBITMQ_CONSUMER_SERVICE_DIR=${SCRIPT_DIR}/../RabbitMQConsumerService

docker network create message-broker-evaluation-network || true

docker-compose -f ${PUBLISHER_SERVICE_DIR}/docker-compose.yaml up -d --build
docker-compose -f ${RABBITMQ_CONSUMER_SERVICE_DIR}/docker-compose.yaml up -d --build

echo ""
echo "[*] Done"
echo ""
echo "open http://localhost:9000/swagger/index.html"
echo ""