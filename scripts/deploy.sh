#!/bin/sh

SCRIPT_DIR=$(realpath $(dirname "$0"))
ROOT_DIR=${SCRIPT_DIR}/../

docker-compose -f ${ROOT_DIR}/docker-compose.yaml up -d --build

echo ""
echo "[*] Done"
echo ""
echo "open http://localhost:9000/swagger/index.html"
echo ""