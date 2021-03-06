# Message Broker Evaluation

## Prerequisites
This project requires [Docker](https://docs.docker.com/get-docker/) and [Docker Compose](https://docs.docker.com/compose/install/).

## Start services
To start all services run the deploy script.
```bash
sh ./scripts/deploy.sh
```

Options for deploy.sh:
|   |   |
|---|---|
| --no-rabbitmq | starts without required services to evaluate RabbitMQ |
| --no-kafka | starts without required services to evaluate Apache Kafka |
| --no-redis | starts without required services to evaluate Redis |
| --no-publisher-service | starts without Publisher Service |
| --rabbitmq-consumer-instances=NUM | scale rabbitmq-consumer-service to NUM instances |
| --kafka-consumer-instances=NUM | scale kafka-consumer-service to NUM instances |
| --redis-consumer-instances=NUM | scale redis-consumer-service to NUM instances |

## Usage
You can access the PublisherService to start sending messages at [http://127.0.0.1:9000/swagger/index.html](http://127.0.0.1:9000/swagger/index.html).  
The results are logged in the respective consumer service.
```bash
docker logs -f SERVICE
```

## Teardown
To teardown all services run the teardown script.
```bash
sh ./scripts/teardown.sh
```