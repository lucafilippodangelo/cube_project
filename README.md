Overall picture
![aspnetrun-microservices](https://user-images.githubusercontent.com/1147445/79753821-34b93800-831f-11ea-86fc-617654557084.png)

There are a couple of microservices which implemented **e-commerce** modules over **Product, Basket** and **Ordering** microservices with **NoSQL (MongoDB, Redis)** and **Relational databases (Sql Server)** with communicating over **RabbitMQ Event Driven Communication** and using **Ocelot API Gateway**.

**Verticalization of -> https://medium.com/aspnetrun/microservices-architecture-on-net-3b4865eea03f**
![Whiteboard](https://github.com/lucafilippodangelo/aspnetcore-microservices-playground/blob/master/Images/board.jpg)

### To run locally
1. At the root directory which include **docker-compose.yml** files, run below command run "docker compose up" then ensure to have running containers for 
* orderdb
* rabbitmq
* basketdb
2. set as startup projects both basket and order, then setup ports and hope page from projects properties.

### To run only in containers

1. Clone the repository
2. At the root directory which include **docker-compose.yml** files, run below command:
```csharp
docker-compose -f docker-compose.yml -f docker-compose.override.yml up –d
```
3. You can **launch microservices** as below urls:
* **RabbitMQ -> http://localhost:15672/**
* **Catalog API -> http://localhost:8000/swagger/index.html**
* **Basket API -> http://localhost:8001/swagger/index.html**
* **Order API -> http://localhost:8002/swagger/index.html**
* **API Gateway -> http://localhost:7000/Order?username=swn**
* **Web UI -> http://localhost:8003/**

5. Launch http://localhost:8003/ in your browser to view the Web UI. You can use Web project in order to **call microservices over API Gateway**. When you **checkout the basket** you can follow **queue record on RabbitMQ dashboard**.
