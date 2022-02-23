#!/bin/sh

SCRIPT_DIR=$(realpath $(dirname "$0"))
PUBLISHER_SREVICE_DIR=${SCRIPT_DIR}/../PublisherService
CONSUMER_SREVICE_DIR=${SCRIPT_DIR}/../ConsumerService

docker network create message-broker-evaluation-network || true

docker-compose -f ${PUBLISHER_SREVICE_DIR}/docker-compose.yaml up -d --build
docker-compose -f ${CONSUMER_SREVICE_DIR}/docker-compose.yaml up -d --build

echo ""
echo "[*] Done"
echo ""
echo "open http://localhost:9000/swagger/index.html"
echo ""