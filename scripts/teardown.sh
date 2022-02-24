#!/bin/sh

SCRIPT_DIR=$(realpath $(dirname "$0"))
ROOT_DIR=${SCRIPT_DIR}/../

docker-compose -f ${ROOT_DIR}/docker-compose.yaml down

echo ""
echo "[*] Done"
echo ""