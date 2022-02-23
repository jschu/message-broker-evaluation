#!/bin/sh

SCRIPT_DIR=$(realpath $(dirname "$0"))
PUBLISHER_SREVICE_DIR=${SCRIPT_DIR}/../PublisherService
CONSUMER_SREVICE_DIR=${SCRIPT_DIR}/../ConsumerService

docker-compose -f ${PUBLISHER_SREVICE_DIR}/docker-compose.yaml down
docker-compose -f ${CONSUMER_SREVICE_DIR}/docker-compose.yaml down

docker network rm message-broker-evaluation-network || true

echo ""
echo "[*] Done"
echo ""