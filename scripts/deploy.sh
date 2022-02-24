#!/bin/sh

docker-compose up -d --build

echo ""
echo "[*] Done"
echo ""
echo "open http://localhost:9000/swagger/index.html"
echo ""