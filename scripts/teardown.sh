#!/bin/sh

SCRIPT_DIR=$(realpath $(dirname "$0"))
ROOT_DIR=${SCRIPT_DIR}/../

docker-compose -f ${ROOT_DIR}/docker-compose.publisher-service.yaml down
docker-compose -f ${ROOT_DIR}/docker-compose.rabbitmq.yaml down
docker-compose -f ${ROOT_DIR}/docker-compose.rabbitmq-consumer-service.yaml down
docker-compose -f ${ROOT_DIR}/docker-compose.kafka.yaml down
docker-compose -f ${ROOT_DIR}/docker-compose.kafka-consumer-service.yaml down

docker network rm rabbitmq || true
docker network rm kafka || true

echo ""
echo "[*] Done"
echo ""