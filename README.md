# Message Broker Evaluation

## Setup
This project requires [Docker](https://docs.docker.com/get-docker/).

## Start services
To start all services run the deploy script.
```bash
sh ./scripts/deploy.sh
```

## Usage
You can access the PublisherService to start sending messages at [http://127.0.0.1:9000/swagger/index.html](http://127.0.0.1:9000/swagger/index.html).  
The results are logged in the respective consumer service.
```bash
docker logs -f rabbitmq-consumer-service
docker logs -f kafka-consumer-service
```

## Teardown
To teardown all services run the teardown script.
```bash
sh ./scripts/teardown.sh
```