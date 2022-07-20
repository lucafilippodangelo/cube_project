Overall picture

![Whiteboard](Images/20220612_170319.jpg)

There are a couple of microservices which implemented **e-commerce** modules over **Product, Basket** and **Ordering** microservices with **NoSQL (MongoDB, Redis)** and **Relational databases (Sql Server)** with communicating over **RabbitMQ Event Driven Communication** and using **Ocelot API Gateway**.

**Verticalization of -> https://medium.com/aspnetrun/microservices-architecture-on-net-3b4865eea03f**


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
* **RabbitMQ -> http://localhost:15672/** guest-guest
* **Web UI "cube_bidsignalr" -> http://localhost:9008/**
* **Web UI "cube_bidapi" -> http://localhost:8008/swagger/index.html**
* **Web UI "cube_auctionapi" -> http://localhost:8007/swagger/index.html**

4. Microservides need to run in background:
* **mongo** 
* **redis**
* **orderdb**

