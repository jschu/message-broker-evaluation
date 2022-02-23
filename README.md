# Message Broker Evaluation

## Getting Started
With this project different message brokers can be evaluated. Each message broker is implemented in a separate branch.  
<br>

## Install Docker
This project requires [Docker](https://docs.docker.com/get-docker/).  
<br>

## Checkout Branch
Check out the branch of the message broker you want to evaluate. For example:
```bash
git checkout rabbitmq
```  
<br>

## Start services

To start all services run the deploy script.
```bash
sh ./scripts/deploy.sh
```  
<br>

## Usage
You can access the PublisherService to start sending messages at [http://127.0.0.1:9000/swagger/index.html](http://127.0.0.1:9000/swagger/index.html)  
The results are logged at the consumer service.
```bash
docker logs -f consumer-service
```  
<br>

## Teardown
To teardown all services run the teardown script.
```bash
sh ./scripts/teardown.sh
```