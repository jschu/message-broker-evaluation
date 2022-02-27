#!/bin/sh

SCRIPT_DIR=$(realpath $(dirname "$0"))
ROOT_DIR=${SCRIPT_DIR}/../

docker-compose -f ${ROOT_DIR}/docker-compose.yaml down
docker-compose -f ${ROOT_DIR}/docker-compose.kafka-consumer-service.yaml down

docker network rm rabbitmq || true
docker network rm kafka || true

echo ""
echo "[*] Done"
echo ""