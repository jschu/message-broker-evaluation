#!/bin/sh

SCRIPT_DIR=$(realpath $(dirname "$0"))
PUBLISHER_SERVICE_DIR=${SCRIPT_DIR}/../PublisherService
RABBITMQ_CONSUMER_SERVICE_DIR=${SCRIPT_DIR}/../RabbitMQConsumerService

docker-compose -f ${PUBLISHER_SREVICE_DIR}/docker-compose.yaml down
docker-compose -f ${RABBITMQ_CONSUMER_SERVICE_DIR}/docker-compose.yaml down

docker network rm message-broker-evaluation-network || true

echo ""
echo "[*] Done"
echo ""